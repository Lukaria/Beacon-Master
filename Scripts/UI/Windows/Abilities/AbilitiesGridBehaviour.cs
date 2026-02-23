using System;
using System.Collections.Generic;
using Abilities;
using Abilities.Configs;
using Abilities.Interfaces;
using Abilities.Signals;
using ObservableCollections;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.Abilities
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class AbilitiesGridBehaviour : MonoBehaviour
    {
        [SerializeField] private AbilityHUDIcon hudIcon;
        
        private IAbilityContainer _abilityContainer;
        private List<AbilityHUDIcon> _abilityIcons = new();
        private GridLayoutGroup _gridLayout;


        [Inject]
        public void Construct(
            SignalBus signalBus,
            IAbilityContainer abilityContainer)
        {
            _abilityContainer = abilityContainer;
            _gridLayout = GetComponent<GridLayoutGroup>();

            signalBus.Subscribe<AbilityContainerUpdatedSignal>(_ => RedrawItems());
        }

        private void OnEnable()
        {
            RedrawItems();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void RedrawItems()
        {
            if (!gameObject.activeInHierarchy) return;
            
            foreach (var icon in _abilityIcons)
            {
                Destroy(icon.gameObject);
            }
            _abilityIcons.Clear();
            
            foreach (var (id, count) in _abilityContainer.GetAppliedAbilities())
            {
                var icon = Instantiate(hudIcon, _gridLayout.transform);
                var sprite = _abilityContainer.GetAbilityConfig(id).Sprite;
                icon.Show(sprite, count);
                _abilityIcons.Add(icon);
            }
        }
        
        public void RedrawItems(Dictionary<AbilityId, int> dictionary)
        {
            if (!gameObject.activeInHierarchy) return;
            
            foreach (var icon in _abilityIcons)
            {
                Destroy(icon.gameObject);
            }
            _abilityIcons.Clear();
            
            foreach (var (abilityId, count) in dictionary)
            {
                var icon = Instantiate(hudIcon, _gridLayout.transform);
                var sprite = _abilityContainer.GetAbilityConfig(abilityId).Sprite;
                icon.Show(sprite, count);
                _abilityIcons.Add(icon);
            }
        }
    }
}