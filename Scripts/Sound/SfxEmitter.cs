using System;
using Common.Interfaces;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxEmitter : MonoBehaviour, IUpdateable
    {
        public Action<SfxEmitter> OnSoundPlayed;
        private AudioSource _source;
        private float _duration;
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.Stop();
        }
        

        public void PlaySfx(AudioClip clip, Vector3 position, float volume)
        {
            transform.position = position;
            _source.clip = clip;
            _source.volume = volume;
            _duration = (clip.length / Mathf.Abs(_source.pitch)) + Time.time;
            gameObject.SetActive(true);
            _source.Play();
        }
        
        public void SetSfxVolume(float volume) => _source.volume = volume;
        
        public void PerformUpdate(float dt)
        {
            _duration -= dt;

            if (_duration <= 0) SoundPlayed();
        }

        private void SoundPlayed()
        {
            _duration = 0;
            _source.Stop();
            gameObject.SetActive(false);
            OnSoundPlayed?.Invoke(this);
        }
    }
}