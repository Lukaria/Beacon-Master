using Input.Handlers;
using Input.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class InputManager : MonoBehaviour,
        IInputHandler
    {
        private InputHandlerFactory _inputHandlerFactory;
        private IInputHandler _inputHandler;
        private PlayerInput _playerInput;

        [Inject]
        public void Construct(InputHandlerFactory inputHandlerFactory)
        {
            _inputHandlerFactory = inputHandlerFactory;
            _inputHandler = _inputHandlerFactory.Create();
            _playerInput = new PlayerInput();

            Utils.Assertions.IsTrueAssert(_inputHandler is not null, "Input Handler is empty!");
        }
        private void Awake()
        {

            _playerInput.Gameplay.Click.started += OnClick;
            _playerInput.Gameplay.Click.performed += OnClick;
            _playerInput.Gameplay.Click.canceled += OnClick;
            
            _playerInput.Gameplay.Swipe.started += OnSwipe;
            _playerInput.Gameplay.Swipe.performed += OnSwipe;
            _playerInput.Gameplay.Swipe.canceled += OnSwipe;
            
            _playerInput.Gameplay.Zoom.started += OnZoom;
            _playerInput.Gameplay.Zoom.performed += OnZoom;
            _playerInput.Gameplay.Zoom.canceled += OnZoom;

        }

        private void OnDestroy()
        {
            _playerInput.Gameplay.Click.started -= OnClick;
            _playerInput.Gameplay.Click.performed -= OnClick;
            _playerInput.Gameplay.Click.canceled -= OnClick;
            
            _playerInput.Gameplay.Swipe.started -= OnSwipe;
            _playerInput.Gameplay.Swipe.performed -= OnSwipe;
            _playerInput.Gameplay.Swipe.canceled -= OnSwipe;
            
            _playerInput.Gameplay.Zoom.started -= OnZoom;
            _playerInput.Gameplay.Zoom.performed -= OnZoom;
            _playerInput.Gameplay.Zoom.canceled -= OnZoom;
        }

        private void OnEnable()
        {
            _playerInput?.Enable();
        }

        private void OnDisable()
        {
            _playerInput?.Disable();
        }
        
        public void OnClick(InputAction.CallbackContext ctx)
        {
            _inputHandler.OnClick(ctx);
        }

        public void OnSwipe(InputAction.CallbackContext ctx)
        {
            _inputHandler.OnSwipe(ctx);
        }

        public void OnZoom(InputAction.CallbackContext ctx)
        {
            _inputHandler.OnZoom(ctx);
        }

        public void FixedUpdate()
        {
            _inputHandler?.FixedUpdate();
        }
        
        public Vector2 GetLastPosition() => _inputHandler.GetLastPosition();
    }
}