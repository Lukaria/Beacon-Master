using System;
using Cash;
using Chargeable;
using Common;
using Common.Behaviours;
using Common.Interfaces;
using CustomEditor;
using Game.Score;
using R3;
using UnityEngine;
using VFX;
using Zenject;

namespace Boat.Boats
{
    [RequireComponent(typeof(DamageableEntity))]
    public abstract class BoatBase : ChargeableBase, IOutOfScreen, IExplosive
    {
        [SerializeField] private AudioClip explosionSound;
        [SerializeField, Readonly] protected BoatStats boatStats;

        private bool _pathCreated;
        public Action<BoatBase> BoatDestroyed;
        private float _pathProgress;
        private Vector3 _targetPosition;
        private float _stopDistance = 1.0f;
        private CashScreenUIManager _cashScreenUIManager;
        private ExplosionManager _explosionManager;
        private DisposableBag _disposableBag;
        private IHealth _healthComponent;
        private BoatStats _statsCopy;

        [Inject]
        public void Construct(
            IHealth health,
            ExplosionManager explosionManager,
            CashScreenUIManager cashScreenUIManager)
        {
            _healthComponent =  health;
            _cashScreenUIManager = cashScreenUIManager;
            _explosionManager = explosionManager;
        }
        

        public void SetStats(BoatStats stats)
        {
            _statsCopy = stats;
        }

        public void ResetStats()
        {
            boatStats = _statsCopy;
            chargeStats = boatStats.chargeable;
            _healthComponent.Set(boatStats.health);
            ResetCharge();
        }

        private void OnEnable()
        {
            _disposableBag = new();
            _healthComponent.CurrentHealth
                //.Pairwise()
                //.Subscribe(tuple => OnHealthUpdated(tuple.Current))
                .Subscribe(OnHealthUpdated)
                .AddTo(ref _disposableBag);
        }

        private void OnHealthUpdated(float obj)
        {
            if(obj <= 0) OnExplosion();
        }

        private void OnDisable()
        {
            _disposableBag.Dispose();
        }
        
        public override void PerformUpdate(float dt)
        {
            base.PerformUpdate(dt);
            
            if (!_pathCreated) return;

            var direction = Charged ? transform.position - _targetPosition : _targetPosition - transform.position;
            var distance = direction.magnitude;

            if (distance <= _stopDistance) {
                _pathCreated = false;
                return;
            }

            var targetRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, boatStats.speed * dt);

            transform.Translate(Vector3.forward * (boatStats.speed * dt));
        }

        public void SetTarget(Vector3 target) {
            _targetPosition = target;
            _pathCreated = true;
        }

        protected override void OnCharged()
        {
            _cashScreenUIManager.AddCashWithShowUp(transform.position, boatStats.cash);
        }

        public void OnExitScreen()
        {
            if (Charged)
            {
                DestroyBoat();
            }
        }

        public virtual void OnExplosion()
        {
            _explosionManager.PlayAt(transform.position);
            _soundManager.PlaySfx(explosionSound, transform.position);
            DestroyBoat();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (other.gameObject.TryGetComponent(out IDamageable otherHealth))
            {
                otherHealth.TakeDamage(boatStats.damage);
                _healthComponent.TakeDamage(_healthComponent.CurrentHealth.CurrentValue);
            }
        }


        public void DestroyBoat()
        {
            ReleaseWheel();
            BoatDestroyed?.Invoke(this);
        }

        public void SetSpeed(float newSpeed) => boatStats.speed = newSpeed;

        public float GetSpeed() => boatStats.speed;
    }
}