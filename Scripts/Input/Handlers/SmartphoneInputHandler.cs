using System.Collections.Generic;
using Camera;
using Input.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Input.Handlers
{
    public class SmartphoneInputHandler : IInputHandler
    {
        private readonly CameraConfigData _cameraConfig;
        private UnityEngine.Camera _camera;
        private Vector2 _lastPosition = Vector2.zero;


        public SmartphoneInputHandler(CameraConfigData cameraConfig)
        {
            _cameraConfig = cameraConfig;
            _camera = UnityEngine.Camera.main;
        }

        public Vector2 GetLastPosition() => _lastPosition;

        public void OnClick(InputAction.CallbackContext ctx)
        {
            
            var mousePos = ctx.ReadValue<Vector2>();
            if (!ctx.started || IsPointerOverUI(mousePos)) return;
            
            var origin = _camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(origin, out var hit, 100f, _cameraConfig.LayerMask)
                && hit.transform.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.OnInteract();
            }
            else
            {
                _lastPosition.x = hit.point.x;
                _lastPosition.y = hit.point.z;
            }
        }

        public void OnSwipe(InputAction.CallbackContext ctx)
        {
            
            var mousePos = ctx.ReadValue<Vector2>();
            
            if(ctx.canceled || IsPointerOverUI(mousePos)) return;
            
            var origin = _camera.ScreenPointToRay(mousePos);
            
            if (!Physics.Raycast(origin, out var hit, 100f, _cameraConfig.LayerMask)) return;
            
            _lastPosition.x = hit.point.x;
            _lastPosition.y = hit.point.z;
        }

        public void OnZoom(InputAction.CallbackContext ctx)
        {
        }

        public void FixedUpdate()
        {
        }
        
        private bool IsPointerOverUI(Vector2 screenPos)
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPos
            };

            var results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
    }
}