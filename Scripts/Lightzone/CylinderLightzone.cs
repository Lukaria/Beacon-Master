using Input;
using Lightzone.Dto;
using Lightzone.Interfaces;
using Lightzone.Thresholds;
using UnityEngine;
using Zenject;

namespace Lightzone
{
    public class CylinderLightzone : LightzoneBase
    {
        public override ILightzoneThreshold Threshold => _threshold;

        private readonly CylinderThreshold _threshold = new();
        private Vector3 _targetPosition;
        private InputManager _inputManager;
        private Vector3 _currentLookPosition;
        
        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public void OnEnable()
        {
            _targetPosition = transform.position;
        }
        
        private void Start()
        {
            _currentLookPosition = transform.position + transform.forward * ((Stats.MinAngle + Stats.MaxAngle) / 2);
        }


        public override void Initialize(Vector3 center, LightzoneDto dto)
        {
            Stats = dto;
            _threshold.Recalculate(center, Stats.MinAngle, Stats.MaxAngle, Stats.Radius);
        }

        public override void PerformUpdate(float dt)
        {
            HandleRotation();
        }

        private void HandleRotation()
        {
            var coords = _inputManager.GetLastPosition();
            
            _targetPosition = new Vector3(coords.x, 0, coords.y);
            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_targetPosition - transform.position)) < 0.5f) 
            {
                return;
            }
            
            var startPos = _currentLookPosition;
            var toTarget = _targetPosition - startPos;
            
            //todo remove unsued
            var moveDirection = Threshold.IsPathObstructed(startPos, _targetPosition, out var avoidanceTangent)
                ? avoidanceTangent.normalized
                : toTarget.normalized;

            _currentLookPosition = Threshold.GetConstrainedPosition(_currentLookPosition, _targetPosition);

            var lookDir = _currentLookPosition - transform.position;
            
            if (lookDir == Vector3.zero) return;
            var targetRot = Quaternion.LookRotation(lookDir);
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * Stats.Speed);
        }


        private void OnDrawGizmosSelected()
        {
            if (Threshold == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Threshold.CenterPosition,
                Threshold.InnerRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Threshold.CenterPosition,
                Threshold.OuterRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _targetPosition);
        }
    }
}