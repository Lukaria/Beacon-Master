using Camera;
using Input.Interfaces;
using Zenject;

namespace Input.Handlers
{
    public class InputHandlerFactory : PlaceholderFactory<IInputHandler>
    {
        private CameraConfigData _cameraConfig;

        private IInputHandler _inputHandler;
        
        [Inject]
        public void Construct(CameraConfigData cameraConfig)
        {
            _cameraConfig = cameraConfig;
        }
        public override IInputHandler Create()
        {
#if UNITY_EDITOR_WIN
            return new ComputerInputHandler(_cameraConfig);
#elif UNITY_ANDROID
            return new SmartphoneInputHandler(_cameraConfig);
#endif
        }
    }
}