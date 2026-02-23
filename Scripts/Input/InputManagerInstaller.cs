using Input.Handlers;
using Input.Interfaces;
using UnityEngine;
using Zenject;

namespace Input
{
    public class InputManagerInstaller : MonoInstaller
    {
        [SerializeField] private InputManager inputManager;

        public override void InstallBindings()
        {
            Container.Bind<InputManager>().FromInstance(inputManager).AsSingle().NonLazy();
            Container.BindFactory<IInputHandler, InputHandlerFactory>().To<InputManager>().AsCached();
        }
    }
}