using UnityEngine;
using Zenject;

namespace Camera
{
    public class CameraControllerInstaller : MonoInstaller
    {
        [SerializeField] private CameraController cameraController;

        public override void InstallBindings()
        {
            Container.Bind<CameraController>().FromInstance(cameraController).AsSingle().NonLazy();
        }
    }
}