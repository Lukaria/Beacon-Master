using UnityEngine;

namespace Obstacle.Obstacles
{
    public class ThundercloudShadowFulfill : MonoBehaviour
    {
        [SerializeField] private float duration = 1.5f;
    
        [SerializeField] private bool playOnStart = true;

        private SpriteRenderer _spriteRenderer;
        private MaterialPropertyBlock _propBlock;
        private float _timer;
        private bool _isAnimating;
    
        private static readonly int ProgressProp = Shader.PropertyToID("_Progress");

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _propBlock = new MaterialPropertyBlock();
        }

        void Start()
        {
            SetRadiusProgress(0f);

            if (playOnStart)
            {
                Play();
            }
        }

        void Update()
        {
            if (!_isAnimating) return;

            _timer += Time.deltaTime;
        
            float t = Mathf.Clamp01(_timer / duration);
        
            // easing: t = t * t * (3f - 2f * t);
        
            SetRadiusProgress(t);

            if (_timer >= duration)
            {
                _isAnimating = false;
            }
        }

        public void Play()
        {
            _timer = 0f;
            _isAnimating = true;
        }

        public void ResetCircle()
        {
            _timer = 0f;
            _isAnimating = false;
            SetRadiusProgress(0f);
        }

        private void SetRadiusProgress(float value)
        {
            _spriteRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(ProgressProp, value);
            _spriteRenderer.SetPropertyBlock(_propBlock);
        }
    }
}