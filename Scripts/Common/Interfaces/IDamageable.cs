using R3;

namespace Common.Interfaces
{
    public interface IDamageable
    {
        Observable<float> OnDamageTaken { get; }
        public void TakeDamage(float amount);
    }
}