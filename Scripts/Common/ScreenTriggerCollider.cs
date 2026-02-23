using Common.Interfaces;
using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(BoxCollider))]
    public class ScreenTriggerCollider : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IOutOfScreen outScreen))
            {
                outScreen.OnExitScreen();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, transform.lossyScale);
        }
    }
}