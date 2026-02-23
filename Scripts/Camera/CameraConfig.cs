using System;
using UnityEngine;

namespace Camera
{
    [CreateAssetMenu(fileName ="CameraConfig", menuName = "Configs/CameraConfig")]
    public class CameraConfig : Zenject.ScriptableObjectInstaller
    {
        public CameraConfigData CameraConfigData;

        public override void InstallBindings()
        {
            Container.BindInstance(CameraConfigData).AsSingle();
        }
    }

    [Serializable]
    public class CameraConfigData
    {
        [SerializeField] private LayerMask _mask;

        public LayerMask LayerMask => _mask;
    }
}