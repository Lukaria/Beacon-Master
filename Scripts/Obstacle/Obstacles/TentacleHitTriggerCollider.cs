using System;
using Boat.Boats;
using Common.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Obstacle.Obstacles
{
    [RequireComponent(typeof(Collider))]
    public class TentacleHitTriggerCollider : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] private Transform damagePoint;
        [SerializeField] private float attackRadius;
        [SerializeField] private LayerMask attackLayer;
        [Header("Animation")]
        [SerializeField] private string animationTriggerName;
        private Animator _animator;

        private float _damage;
        
        public void SetDamage(float damage) => _damage = damage;
        
        public void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<BoatBase>(out _))
            {
                _animator.SetTrigger(animationTriggerName);
            }
        }

        //invokes by Attack animation
        public void OnTentacleAttack()
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