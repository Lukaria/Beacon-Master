using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace VFX
{
    public class ExplosionManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem prefab;
        private ObjectPool<ParticleSystem> _pool;
        
        private void Awake()
        {
            _pool = new ObjectPool<ParticleSystem>(
                createFunc: () => Instantiate(prefab, transform),
                actionOnGet: ps => ps.gameObject.SetActive(true),
                actionOnRelease: ps => ps.gameObject.SetActive(false),
                actionOnDestroy: ps => Destroy(ps.gameObject),
                defaultCapacity: 10
            );
        }
    
        public async UniTaskVoid PlayAt(Vector3 position)
        {
            var ps = _pool.Get();
            ps.transform.position = position;
            ps.Play();
        
            await ReleaseAfterPlay(ps);
        }
    
        private async UniTask ReleaseAfterPlay(ParticleSystem ps)
        {
            await UniTask.WaitForSeconds(ps.main.duration, 
                cancellationToken: this.GetCancellationTokenOnDestroy());
            
            _pool.Release(ps);
        }
    }
}