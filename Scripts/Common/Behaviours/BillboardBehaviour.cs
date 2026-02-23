using UnityEngine;

namespace Common.Behaviours
{
    public class BillboardBehaviour : MonoBehaviour
    {
        private UnityEngine.Camera _camera;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            transform.LookAt(_camera.transform);
        }
    }
}