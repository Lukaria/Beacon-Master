using System.Collections.Generic;
using Chargeable;
using Chargeable.Wheel;
using Common.Interfaces;
using Player;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Cash
{
    public class CashScreenUIManager : MonoBehaviour
    {
        [Header("Setup")] 
        [SerializeField] private Canvas sharedCanvas;

        [SerializeField] private CashShowUp showUpPrefab;
        [SerializeField] private CashCollectableUI collectablePrefab;

        [Header("Settings")] 
        [SerializeField] private Vector3 showUpWorldOffset = new(0, 2, 0);
        [SerializeField] private Vector3 collectableWorldOffset = new(0, 2, 0);

        private List<CashCollectableUI> _activeCollectables = new();
        private PlayerDataService _playerData;
        private ObjectPool<CashShowUp> _pool;

        [Inject]
        public void Construct(PlayerDataService playerData)
        {
            _playerData = playerData;
            
            _pool = new ObjectPool<CashShowUp>(
                createFunc: () => Instantiate(showUpPrefab, transform),
                actionOnGet: ps => ps.gameObject.SetActive(true),
                actionOnRelease: ps => ps.gameObject.SetActive(false),
                actionOnDestroy: ps => Destroy(ps.gameObject),
                defaultCapacity: 10
            );
            
            
        }

        public void AddCashWithShowUp(Vector3 position, int cashAmount)
        {
            _playerData.AddCash(cashAmount);
            var showUp = _pool.Get();
            showUp.transform.parent = sharedCanvas.transform;
            showUp.Show(position +  showUpWorldOffset, cashAmount);
        }

        private void CashCollected(CashCollectableUI obj)
        {
            obj.OnCollected -= CashCollected;
            var containingCash = obj.GetContainingCash();
            AddCashWithShowUp(obj.transform.position, containingCash);
            _activeCollectables.Remove(obj);
        }
    }
}

