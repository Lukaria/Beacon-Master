using Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Common
{
    [RequireComponent(typeof(Button))]
    public class PlayUISoundOnClick : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        
        private SoundManager _soundManager;
        private Button _button;

        [Inject]
        public void Construct(SoundManager soundManager)
        {
            _soundManager = soundManager;
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(PlayUISound);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PlayUISound);
        }

        private void PlayUISound()
        {
            _soundManager.PlayUI(clip);
        }
    }
}