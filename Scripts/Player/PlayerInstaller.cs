using Common;
using Common.Interfaces;
using Persistence.Interfaces;
using Persistence.Repository;
using Zenject;

namespace Player
{
    //project context installer
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind(
                typeof(ICreateRepository<PlayerDataDto>),
                typeof(IReadRepository<PlayerDataDto>)).To<PlayerDataRepository>()
                .AsSingle().NonLazy();

            
            Container
                //.Bind<IDataService<PlayerDataDto>>()
                .Bind<PlayerDataService>()
                .AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<PlayerHealthComponent>().AsSingle();
            
        }
    }
}