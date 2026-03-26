using UnityEngine;

namespace Obstacle.Obstacles
{
    public class ThundercloudWaterShadow : MonoBehaviour
    {
        [SerializeField] private LayerMask waterLayerMask;
        [SerializeField] private Transform cloud;
        public float heightOffset = 0.05f;
        private static readonly Vector3 down = new (0, -1, 0);

        void LateUpdate()
        {
            if (cloud is null ||
                !Physics.Raycast(cloud.position, cloud.position + down, out var hit, 100f,
                    waterLayerMask)) return;
            
            
            var targetPos = new Vector3(cloud.position.x, hit.transform.position.y + heightOffset, cloud.position.z);
            transform.position = targetPos;

            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}