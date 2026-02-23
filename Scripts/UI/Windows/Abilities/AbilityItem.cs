using System;
using Abilities;
using Abilities.Configs;
using Lighthouse.Pool;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.Abilities
{
    public class AbilityItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Button elementButton;
        public Action<AbilityConfigBase> OnAbilityButtonClicked;
        private AbilityConfigBase _ability;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            elementButton.onClick.AddListener(AbilityElementClicked);
        }
        
        private void OnDisable()
        {
            elementButton.onClick.RemoveListener(AbilityElementClicked);
        }

        private void AbilityElementClicked()
        {
            OnAbilityButtonClicked?.Invoke(_ability);
        }

        public void Show(AbilityConfigBase ability)
        {
            _ability = ability;
            icon.sprite = ability.Sprite;
            title.text = ability.Title;
            description.text = ability.Description;
            gameObject.SetActive(true);
        }
    }
}