using Common.Interfaces;
using Zenject;

namespace Common.Installers
{
    public class DamageableInstaller : MonoInstaller<DamageableInstaller>
    {
        override public void InstallBindings()
        {
            Container.Bind<IHealth>().To<HealthComponent>().AsSingle();
        }
    }
}