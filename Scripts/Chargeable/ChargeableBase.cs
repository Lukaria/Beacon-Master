using Abilities;
using Chargeable.Wheel;
using Common.Interfaces;
using CustomEditor;
using Cysharp.Threading.Tasks;
using Game.Score;
using JetBrains.Annotations;
using Lightzone.Interfaces;
using Sound;
using UnityEngine;
using Zenject;

namespace Chargeable
{
    [RequireComponent(typeof(Collider))]
    public abstract class ChargeableBase : MonoBehaviour, IUpdateable
    {
        [SerializeField] private AudioClip onChargeSound;
        [SerializeField, Readonly] protected ChargeableStats chargeStats;
        [SerializeField, Readonly] private bool charged;
        [SerializeField, Readonly] private bool inZone;
        [SerializeField, Readonly] private float chargeTimer;

        private float _scaledTimeToCharge = 1f;
        [CanBeNull] private ChargeWheelUI _wheelUI;
        private ChargeWheelUIManager _wheelUIManager;
        private AbilityManager _abilityManager;
        protected SoundManager _soundManager;
        protected float _chargePercent;
        private IScoreService _scoreService;

        [Inject]
        public void Construct(
            IScoreService scoreService,
            ChargeWheelUIManager chargeWheelUIManager, 
            AbilityManager abilityManager, 
            SoundManager soundManager)
        {
            _scoreService = scoreService;
            _wheelUIManager = chargeWheelUIManager;
            _abilityManager = abilityManager;
            _soundManager = soundManager;
        }

        public virtual void Awake()
        {
            _scaledTimeToCharge = chargeStats.timeToCharge;
        }

        public void ResetCharge()
        {
            charged = false;
            chargeTimer = 0.0f;
            _chargePercent = 0.0f;
            inZone = false;
        }
        
        public bool Charged => charged;
        
        public bool InZone => inZone;
        
        public virtual void PerformUpdate(float dt)
        {
            if (charged) return;

            _chargePercent = chargeTimer / _scaledTimeToCharge;

            if (inZone)
            {
                chargeTimer += Time.deltaTime;
                if (chargeTimer >= _scaledTimeToCharge)
                {
                    UnitCharged();
                    return;
                }
            }
            else if (!chargeStats.accumulativeUpdate && chargeTimer >= 0)
            {
                chargeTimer -= Time.deltaTime;
            }

            _wheelUI?.UpdateFill(_chargePercent);
            
            if (!inZone && chargeTimer <= 0)
            {
                ReleaseWheel();
            }
        }

        private async UniTaskVoid UnitCharged()
        {
            _soundManager.PlaySfx(onChargeSound, transform.position);
            charged = true;
            _abilityManager.AddExperience(chargeStats.experience);
            _scoreService.AddScore(chargeStats.points);
            OnCharged();

            if (_wheelUI is null) return;
            
            await _wheelUI.PlayChargedAnimation()
                .AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
            ReleaseWheel();
        }

        protected void ReleaseWheel()
        {
            if (_wheelUI is null) return;
            
            _wheelUIManager.ReleaseWheel(this);
            _wheelUI = null;
        }

        protected abstract void OnCharged();

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(charged) return;
            
            if (!other.gameObject.TryGetComponent(out ILightzone lightzone)) return;

            _scaledTimeToCharge = chargeStats.timeToCharge / lightzone.Stats.Brightness;
            
            if (!_wheelUI)
            {
                _wheelUI = _wheelUIManager.RequestWheel(this);
            }
            
            inZone = true;
        }
        
        protected virtual void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out ILightzone _)) return;
            
            inZone = false;
            
            if(!chargeStats.accumulativeTrigger) chargeTimer = 0f;
        }
    }
}