using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abilities;
using Abilities.Configs;
using Abilities.Interfaces;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using Game.Gameplay;
using Lighthouse.Pool;
using Sound;
using UI.Windows.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.Abilities
{
    public class ChooseAbilityWindow : WindowBase
    {
        public override WindowId Id => WindowId.ChooseAbility;
        [SerializeField] private HorizontalLayoutGroup abilityElementsLayout;
        [SerializeField, Range(0, 4)] private int abilityElementsPerRow;
        [SerializeField] private AbilityItem abilityItemPrefab;
        
        
        [Header("Animation")]
        [SerializeField] private float itemAppearanceDuration = 0.5f;
        [SerializeField] private float delayBetweenItems = 0.2f;
        [SerializeField] private Ease animationEase = Ease.OutBack;
        [SerializeField, Space] private float hideAnimationDuration = 0.2f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip itemAppearanceClip;
        
        
        private List<AbilityItem> _abilities = new();
        private IAbilityContainer _abilityContainer;
        private GameplayController _gameplayController;
        private DiContainer _container;
        private SoundManager _soundManager;
        private Vector2 _initialLayoutPosition;

        [Inject]
        public void Construct(
            DiContainer container, 
            GameplayController controller,
            SoundManager soundManager,
            IAbilityContainer abilityContainer)
        {
            _soundManager = soundManager;
            _container = container;
            _gameplayController = controller;
            _abilityContainer = abilityContainer;
            _initialLayoutPosition = abilityElementsLayout.GetComponent<RectTransform>().anchoredPosition;
        }

        private void OnEnable()
        {
            abilityElementsLayout.GetComponent<RectTransform>().anchoredPosition = _initialLayoutPosition;
        }

        protected override void OnShown()
        {
            DestroyItems();
            ShowItems();
        }

        private void DestroyItems()
        {
            foreach (var abilityItem in _abilities)
            {
                Destroy(abilityItem.gameObject);
                abilityItem.OnAbilityButtonClicked -= AbilityElementClicked;
            }
            _abilities.Clear();
        }

        private void ShowItems()
        {
            
            var sequence = DOTween.Sequence();
            
            HashSet<AbilityId> uniqueItems = new();
            
            for (var i = 0; i < abilityElementsPerRow; i++)
            {
                var abilityItem = _container.InstantiatePrefabForComponent<AbilityItem>(abilityItemPrefab,
                    abilityElementsLayout.transform);
                _abilities.Add(abilityItem);
                
                abilityItem.transform.localScale = Vector3.zero;
                var ability = _abilityContainer.GetRandomUniqueAbility(uniqueItems);
                uniqueItems.Add(ability.Id);
                abilityItem.Show(ability);
                abilityItem.OnAbilityButtonClicked += AbilityElementClicked;
                
                var startTime = i * delayBetweenItems;
            
                sequence.Insert(startTime, 
                    abilityItem.transform.DOScale(Vector3.one, itemAppearanceDuration)
                        .OnStart(() => _soundManager.PlayUI(itemAppearanceClip))
                    .SetEase(animationEase));
            }
            sequence.Play();
        }

        private void AbilityElementClicked(AbilityConfigBase ability)
        {
            _abilityContainer.AddAbility(ability);
            CloseAsync();
        }

        protected override void OnBeforeShow()
        {
            _gameplayController.PauseGame();
        }

        protected override async UniTask AnimateHideAsync()
        {
            var layoutTransform = abilityElementsLayout.GetComponent<RectTransform>();
            await DOTween.Sequence()
                .Append(layoutTransform.DOAnchorPosY(20, hideAnimationDuration))
                .Append(layoutTransform.DOAnchorPosY(-500, hideAnimationDuration))
                .Play()
                .WithCancellation(this.GetCancellationTokenOnDestroy())
            ;
        }

        protected override void OnHidden()
        {
            _gameplayController.UnpauseGame();
        }
    }
}