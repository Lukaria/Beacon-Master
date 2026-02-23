using Common.Interfaces;
using Game.Score;
using Game.Signals;
using Leaderboard;
using Lighthouse;
using Lighthouse.Dto;
using Location;
using Persistence.Interfaces;
using Persistence.Repository;
using Player.Signals;
using UnityEngine;
using Zenject;

namespace Save
{
    public class SaveManagerInstaller : MonoInstaller
    {
        [SerializeField] private SaveManager saveManager;

        public override void InstallBindings()
        {
            InstallDependencies();
            InstallRepositoryBindings();
            
            Container.Bind<SaveManager>().FromComponentInNewPrefab(saveManager).AsSingle().NonLazy();


            InstallSaveGameSignals();
        }

        private void InstallSaveGameSignals()
        {
            Container.DeclareSignalWithInterfaces<GameOverSignal>();
            Container.DeclareSignalWithInterfaces<CashUpdatedSignal>();
        }

        private void InstallDependencies()
        {
            Container.Bind<IDataService<LocationDataDto>>().To<LocationDataService>().AsSingle().NonLazy();
            Container.Bind<IDataService<LighthouseDataDto>>().To<LighthouseDataService>().AsSingle().NonLazy();
            Container.Bind<IDataService<LeaderboardDataDto>>().To<LeaderboardDataService>().AsSingle().NonLazy();
        }

        private void InstallRepositoryBindings()
        {
            //LighthouseData
            Container.Bind(
                typeof(ICreateRepository<LighthouseDataDto>),
                typeof(IReadRepository<LighthouseDataDto>)).To<LighthouseRepository>().AsSingle().NonLazy();
            
            //LocationsData
            Container.Bind(
                typeof(ICreateRepository<LocationDataDto>),
                typeof(IReadRepository<LocationDataDto>)).To<LocationsRepository>().AsSingle().NonLazy();
            
            //Leaderboard
            Container.Bind(
                typeof(ICreateRepository<LeaderboardDataDto>),
                typeof(IReadRepository<LeaderboardDataDto>)).To<LeaderboardRepository>().AsSingle().NonLazy();
        }
        
    }
}