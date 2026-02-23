using System;
using Boat.Boats;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Sound;
using UnityEngine;
using Zenject;

namespace Obstacle.Obstacles
{
    [RequireComponent(typeof(Collider))]
    public class ThundercloudHitTriggerCollider : MonoBehaviour
    {
        [SerializeField] private ParticleSystem lightningParticles;
        [SerializeField] private AudioClip audioClip;
        [Header("Attack")]
        [SerializeField] private Transform damagePoint;
        [SerializeField] private float hitDelay;
        [SerializeField] private float audioDelay = 0.3f;
        [SerializeField] private float attackRadius;
        [SerializeField] private LayerMask attackLayer;

        private float _damage;
        private SoundManager _soundManager;

        public void SetDamage(float damage) => _damage = damage;

        [Inject]
        public void Construct(SoundManager manager)
        {
            _soundManager = manager;
        } 

        private void OnEnable()
        {
            lightningParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        private async void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<BoatBase>(out _))
            { 
                lightningParticles.Play(true);
                await UniTask.WaitForSeconds(audioDelay, cancellationToken: this.GetCancellationTokenOnDestroy());
                _soundManager.PlaySfx(audioClip, transform.position);
                await UniTask.WaitForSeconds(hitDelay, cancellationToken: this.GetCancellationTokenOnDestroy());
                OnLightningAttack();
            }
        }

        private void OnLightningAttack()
        {
            var results = new Collider[3];
            
            Physics.OverlapSphereNonAlloc(damagePoint.position, attackRadius, results, attackLayer);

            foreach (var result in results)
            {
                if (result != null &&
                    result.gameObject.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(_damage);
                }
            }
        }
        
        void OnDrawGizmosSelected()
        {
            if (damagePoint == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(damagePoint.position, attackRadius);
        }
    }
}