using Camera;
using Input.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Input.Handlers
{
    public class ComputerInputHandler : IInputHandler
    {

        private readonly CameraConfigData _cameraConfig;
        private UnityEngine.Camera _camera;
        private Vector2 _lastPosition = Vector2.zero;


        public ComputerInputHandler(CameraConfigData cameraConfig)
        {
            _cameraConfig = cameraConfig;
            _camera = UnityEngine.Camera.main;
        }

        public Vector2 GetLastPosition() => _lastPosition;

        public void OnClick(InputAction.CallbackContext ctx)
        {
            var mousePos = ctx.ReadValue<Vector2>();
            if (!ctx.started)
            {
                return;
            }
            
            var origin = _camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(origin, out var hit, 100f, _cameraConfig.LayerMask)
                && hit.transform.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.OnInteract();
            }
        }

        public void OnSwipe(InputAction.CallbackContext ctx)
        {
            if(ctx.canceled) return;
            
            var mousePos = ctx.ReadValue<Vector2>();
            
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
    }
}