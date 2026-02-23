using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Interfaces
{
    public interface IInputHandler
    {
        public Vector2 GetLastPosition();
        public void OnClick(InputAction.CallbackContext ctx);

        public void OnSwipe(InputAction.CallbackContext ctx);
        
        public void OnZoom(InputAction.CallbackContext ctx);

        public void FixedUpdate();
    }
}