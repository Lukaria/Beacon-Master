using System;
using UnityEngine;
using Zenject;

namespace Settings
{
    public class SettingsService : IInitializable, IDisposable
    {
        public Action<float> OnMusicVolumeChanged;
        public Action<float> OnSfxVolumeChanged;
        
        public float MusicVolume { get; private set; }
        public float SfxVolume { get; private set; }
        public float UIVolume { get; private set; }

        public void Initialize()
        {
            MusicVolume = PlayerPrefs.GetFloat(SettingsConstants.MUSIC_VOLUME, 1.0f);
            SfxVolume = PlayerPrefs.GetFloat(SettingsConstants.SFX_VOLUME, 1.0f);
            UIVolume = PlayerPrefs.GetFloat(SettingsConstants.UI_VOLUME, 1.0f);
            
            OnMusicVolumeChanged?.Invoke(MusicVolume);
            OnSfxVolumeChanged?.Invoke(SfxVolume);
        }

        public void SetMusicVolume(float value)
        {
            if (Mathf.Abs(MusicVolume - value) < 0.001f) return;

            MusicVolume = value;
            OnMusicVolumeChanged?.Invoke(MusicVolume);
        }

        public void SetSfxVolume(float value)
        {
            if (Mathf.Abs(SfxVolume - value) < 0.001f) return;

            SfxVolume = value;
            OnSfxVolumeChanged?.Invoke(SfxVolume);
        }



        public void Dispose()
        {
            PlayerPrefs.SetFloat(SettingsConstants.MUSIC_VOLUME, MusicVolume);
            PlayerPrefs.SetFloat(SettingsConstants.SFX_VOLUME, SfxVolume);
            PlayerPrefs.SetFloat(SettingsConstants.UI_VOLUME, UIVolume);
            
            PlayerPrefs.Save();
        }
    }
}