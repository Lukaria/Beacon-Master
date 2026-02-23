using UnityEngine;

namespace Common.Behaviours
{
    [RequireComponent(typeof(Collider))]
    public class DrawGizmosCollider : MonoBehaviour
    {
        [SerializeField] private bool drawOnGizmos = true;
        [SerializeField] private bool drawOnGizmosSelected = true;
        [SerializeField] private Color color = Color.green;

        private void DrawGizmos()
        {
            Gizmos.color = color;
            var objectCollider = GetComponent<Collider>();

            switch (objectCollider)
            {
                case CapsuleCollider c:
                    Gizmos.DrawWireSphere(objectCollider.bounds.center, gameObject.transform.lossyScale.x);
                    break;
            }
        }

        /*private Vector3 CalculateScale()
        {
            gameObject.transform.lossyScale
        }*/
        private void OnDrawGizmos()
        {
            if (!drawOnGizmos) return;
        
            DrawGizmos();
        }
    
        private void OnDrawGizmosSelected()
        {
            if (!drawOnGizmosSelected) return;
        
            DrawGizmos();
        }
    }
}
