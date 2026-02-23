using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chargeable.Wheel
{
    [Serializable]
    public class ChargeWheelUIManager : MonoBehaviour
    {
        [Header("Setup")] 
        [SerializeField] private Canvas sharedCanvas;
        [SerializeField] private ChargeWheelUI wheelPrefab;
        [SerializeField] private int initialPoolSize = 20;

        [Header("Settings")] 
        [SerializeField] private Vector3 worldOffset = new(0, 2f, 0);

        private Queue<ChargeWheelUI> _pool = new();

        private Dictionary<ChargeableBase, ChargeWheelUI> _activeWheels = new();

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            for (var i = 0; i < initialPoolSize; i++)
            {
                CreateWheelForPool();
            }
        }

        private ChargeWheelUI CreateWheelForPool()
        {
            var wheel = Instantiate(wheelPrefab, sharedCanvas.transform);
            wheel.Unbind();
            _pool.Enqueue(wheel);
            return wheel;
        }

        private void LateUpdate()
        {
            foreach (var wheel in _activeWheels.Values)
            {
                wheel.UpdatePosition();
            }
        }

        public ChargeWheelUI RequestWheel(ChargeableBase unit)
        {
            if (_activeWheels.TryGetValue(unit, out var existingWheel))
            {
                return existingWheel;
            }

            var wheel = _pool.Count > 0 ? _pool.Dequeue() : Instantiate(wheelPrefab, sharedCanvas.transform);

            wheel.Bind(unit.transform, worldOffset);
            _activeWheels[unit] = wheel;

            return wheel;
        }
        
        public void ReleaseWheel(ChargeableBase unit)
        {
            if (!_activeWheels.TryGetValue(unit, out var wheel)) return;
            wheel.Unbind();
            _pool.Enqueue(wheel);
            _activeWheels.Remove(unit);
        }
    }
}