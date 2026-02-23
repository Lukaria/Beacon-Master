using Game.Score;
using Game.Signals;
using GameResources.Sprites;
using Haptic;
using Leaderboard;
using ObjectPath;
using Settings;
using UnityEngine;
using Zenject;

namespace Game
{
    [CreateAssetMenu(menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LeaderboardService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.Bind<PathGenerator>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<HapticManager>().AsSingle().NonLazy();
            Container.Bind<SpriteAtlasManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SettingsService>().AsSingle().NonLazy();
            
            
            Container.DeclareSignalWithInterfaces<GameRestartedSignal>();
        }
    }
}