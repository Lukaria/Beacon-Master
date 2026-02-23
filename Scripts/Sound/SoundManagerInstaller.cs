using UnityEngine;
using Zenject;

namespace Sound
{
    public class SoundManagerInstaller : MonoInstaller
    {
        [SerializeField] private SoundManager soundManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SoundManager>().FromInstance(soundManager).AsSingle().NonLazy();
        }
    }
}