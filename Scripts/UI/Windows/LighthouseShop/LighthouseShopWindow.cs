using System.Collections.Generic;
using Lighthouse.Pool;
using Lighthouse.Types;
using Player;
using Save;
using TMPro;
using UI.Common;
using UI.Windows.Common;
using UI.Windows.LighthouseShop.Enhancements;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.LighthouseShop
{
    public class LighthouseShopWindow : WindowBase
    {
        public override WindowId Id => WindowId.LighthouseShop;

        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private IntValueView playerCash;
        [SerializeField] private Button nextButton; 
        [SerializeField] private Button prevButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private IntValueView buyButtonText;
        [SerializeField] private EnhancementsContainer enhancements;
        
        private LighthouseService _service;
        private PlayerDataService _playerData;

        [Inject]
        public void Construct(
            PlayerDataService playerData, LighthouseService service)
        {
            _service = service;
            _playerData = playerData;
        }

        private void OnEnable()
        {
            nextButton.onClick.AddListener(NextButtonClicked);
            prevButton.onClick.AddListener(PrevButtonClicked);
            buyButton.onClick.AddListener(BuyButtonClicked);
            playerCash.Show(_playerData.GetCash());
            UpdateView(_playerData.UnlockedLighthouseId);
        }


        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(NextButtonClicked);
            prevButton.onClick.RemoveListener(PrevButtonClicked);
            buyButton.onClick.RemoveListener(BuyButtonClicked);
            _service.EnableLighthouse(_playerData.UnlockedLighthouseId);
            _playerData.ResetChosenLighthouse();
        }

        private void BuyButtonClicked()
        {
            if (!_playerData.TrySubtractCash(_service.GetActiveLighthouseConfig().Price)) return;
            
            _playerData.UnlockLighthouse(_playerData.ChosenLighthouseId);
            playerCash.Show(_playerData.GetCash());
            UpdateView(_playerData.UnlockedLighthouseId);
        }

        private void NextButtonClicked()
        {
            _playerData.SetNextLighthouse();
            UpdateView(_playerData.ChosenLighthouseId);
        }
        

        private void PrevButtonClicked()
        {
            _playerData.SetPreviousLighthouse();
            UpdateView(_playerData.ChosenLighthouseId);
        }
        
        private void UpdateView(LighthouseId id)
        {
            _service.EnableLighthouse(id);

            var chosenLighthouse = _service.GetActiveLighthouseConfig();
            title.text = chosenLighthouse.Title;
            description.text = chosenLighthouse.Description;

            if (_playerData.IsChosenLighthouseLocked())
            {
                enhancements.Hide();
                buyButton.gameObject.SetActive(true);
                buyButtonText.Show(chosenLighthouse.Price);
                
            }
            else
            {
                buyButton.gameObject.SetActive(false);
                enhancements.Show();
            }

            buyButton.interactable = _playerData.GetCash() >= chosenLighthouse.Price;
        }
    }
}
