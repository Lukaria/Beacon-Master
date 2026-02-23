using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using Settings;
using UnityEngine;
using Zenject;

namespace Sound
{
    public class SoundManager : MonoBehaviour, IFixedTickable
    {
        [Header("SFX")]
        [SerializeField] private SfxEmitter sfxPrefab;
        [SerializeField] private Transform sfxParent;
        [SerializeField, Range(1, 10)] private int sfxPoolSize = 5;

        [Header("Other")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource uiSource;

        private readonly Stack<SfxEmitter> _sfxEmitters = new();

        private readonly List<SfxEmitter> _activeEmitters = new();

        private float _musicVolume = 1.0f;
        private float _sfxVolume = 1.0f;
        private float _uiVolume = 1.0f;
        private SettingsService _settings;

        [Inject]
        public void Construct(SettingsService settings)
        {
            _settings = settings;
            _settings.OnMusicVolumeChanged += UpdateMusicVolume;
            _settings.OnSfxVolumeChanged += UpdateSfxVolume;
        }

        private void UpdateMusicVolume(float value)
        {
            _musicVolume = value;
            musicSource.volume = _musicVolume;
        }
        
        private void UpdateSfxVolume(float value)
        {
            _sfxVolume = value;
            foreach (var activeEmitter in _activeEmitters)
            {
                activeEmitter.SetSfxVolume(_sfxVolume);
            }
        }

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            for (var i = 0; i < sfxPoolSize; ++i)
            {
                
                _sfxEmitters.Push(CreatePoolSfxObject());
            }
        }

        public void PlaySfx(AudioClip clip, Vector3 position)
        {
            var source = GetSfxFromPool();
            _activeEmitters.Add(source);
            source.OnSoundPlayed += SfxPlayed;
            source.PlaySfx(clip, position, _sfxVolume);
        }

        private void SfxPlayed(SfxEmitter emitter)
        {
            emitter.OnSoundPlayed -= SfxPlayed;
            _activeEmitters.Remove(emitter);
            _sfxEmitters.Push(emitter);
            emitter.gameObject.SetActive(false);

        }

        private SfxEmitter CreatePoolSfxObject()
        {
            var source = Instantiate(sfxPrefab, sfxParent);
            source.gameObject.SetActive(false);
            return source;
        }

        private SfxEmitter GetSfxFromPool()
        {
            if (_sfxEmitters.Count != 0) return _sfxEmitters.Pop();
            
            return CreatePoolSfxObject();
        }
        

        public void PlayUI(AudioClip clip)
        {
            uiSource.PlayOneShot(clip, _uiVolume);
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (musicSource.clip == clip) return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = _musicVolume;
            musicSource.Play();
        }
        

        public void PauseMusic()
        {
            musicSource.Pause();
        }

        public void UnpauseMusic()
        {
            musicSource.UnPause();
        }

        public void FixedTick()
        {
            foreach (var activeEmitter in _activeEmitters.ToList())
            {
                activeEmitter.PerformUpdate(Time.deltaTime);
            }
        }
    }
}