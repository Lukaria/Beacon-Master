using UnityEngine;

namespace Utils
{
    public class OpenUrlBehaviour : MonoBehaviour
    {
        [SerializeField] private string url;

        public void OpenUrl()
        {
            Application.OpenURL(url);
        }
    }
}
