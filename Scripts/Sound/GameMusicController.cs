using System.Collections.Generic;
using Common.Extensions;
using Game;
using R3;
using UnityEngine;
using Zenject;

namespace Sound
{
    public class GameMusicController : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> mainMenuAudioClips = new();
        [SerializeField] private List<AudioClip> gameplayAudioClips = new();
        [SerializeField] private AudioClip gameOverAudioClip;
        private SoundManager _soundManager;

        [Inject]
        public void Construct(SoundManager soundManager, GameManager gameManager)
        {
            _soundManager = soundManager;
            gameManager.GameStateUpdated
                .Subscribe(OnGameStateUpdated)
                .AddTo(this);
        }

        private void OnGameStateUpdated(GameState currentState)
        {
            switch (currentState)
            {
                case GameState.MainMenu:
                    _soundManager.PlayMusic(mainMenuAudioClips.GetRandomElement());
                    break;
                case GameState.GameOver:
                    _soundManager.PlayMusic(gameOverAudioClip);
                    break;
                case GameState.Playing:
                    _soundManager.PlayMusic(gameplayAudioClips.GetRandomElement());
                    break;
            }
        }
    }
}