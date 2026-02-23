using UnityEngine;
using Zenject;

namespace Grid
{
    public class GridInstaller : MonoInstaller
    {
        [SerializeField] private GridManager gridManager;

        public override void InstallBindings()
        {
            Container.Bind<GridManager>().FromInstance(gridManager).AsSingle().NonLazy();
        }
    }
}