using UnityEngine;
using Zenject;

namespace Chargeable.Wheel
{
    public class ChargeWheelInstaller : MonoInstaller
    {
        [SerializeField] private ChargeWheelUIManager chargeWheelUIManager;

        public override void InstallBindings()
        {
            Container.Bind<ChargeWheelUIManager>().FromInstance(chargeWheelUIManager).AsSingle().NonLazy();
        }
    }
}