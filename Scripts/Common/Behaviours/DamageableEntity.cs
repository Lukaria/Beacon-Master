using Common.Interfaces;
using R3;
using UnityEngine;
using Zenject;

namespace Common.Behaviours
{
    public class DamageableEntity : MonoBehaviour, IDamageable
    {
        private IHealth _healthModel;

        [Inject]
        public void Construct(IHealth healthModel)
        {
            _healthModel = healthModel;
        }
        
        public Observable<float> OnDamageTaken { get; }
        public void TakeDamage(float amount)
        {
          _healthModel.TakeDamage(amount);
        }
    }
}