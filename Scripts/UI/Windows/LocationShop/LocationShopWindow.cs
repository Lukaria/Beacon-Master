using Location;
using Player;
using Save;
using TMPro;
using UI.Common;
using UI.Windows.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using LocationService = Location.LocationService;

namespace UI.Windows.LocationShop
{
    public class LocationShopWindow : WindowBase
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private IntValueView playerCash;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private IntValueView buyButtonText;
        
        private LocationService _service;
        private PlayerDataService _playerData;

        public override WindowId Id => WindowId.LocationShop;
        

        [Inject]
        public void Construct(LocationService locationService, PlayerDataService playerData)
        {
            _service = locationService;
            _playerData = playerData;
        }

        private void OnEnable()
        {
            nextButton.onClick.AddListener(NextButtonClicked);
            prevButton.onClick.AddListener(PrevButtonClicked);
            buyButton.onClick.AddListener(BuyButtonClicked);
            playerCash.Show(_playerData.GetCash());
            UpdateView(_service.UnlockedLocationId);
        }

        private void BuyButtonClicked()
        {
            if (!_playerData.TrySubtractCash(_service.GetActiveLocationConfig().Price)) return;
            
            _service.UnlockLocation(_service.GetActiveLocationConfig().LocationId);
            playerCash.Show(_playerData.GetCash());
            UpdateView(_service.UnlockedLocationId);
        }

        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(NextButtonClicked);
            prevButton.onClick.RemoveListener(PrevButtonClicked);
            buyButton.onClick.RemoveListener(BuyButtonClicked);
            _service.EnableLocation(_service.UnlockedLocationId);
            _service.ResetChosenLocation();
        }

        private void NextButtonClicked()
        {
            UpdateView(_service.SetNextLocation());
        }

        private void PrevButtonClicked()
        {
            UpdateView(_service.SetPreviousLocation());
        }
        
        private void UpdateView(LocationId id)
        {
            _service.EnableLocation(id);

            var chosenLocation = _service.GetActiveLocationConfig();
            title.text = chosenLocation.Title;
            description.text = chosenLocation.Description;

            if (_service.IsChosenLocationLocked())
            {
                buyButton.gameObject.SetActive(true);
                buyButtonText.Show(chosenLocation.Price);
            }
            else
            {
                buyButton.gameObject.SetActive(false);
            }
            
            buyButton.interactable = _playerData.GetCash() >= chosenLocation.Price;
        }

    }    
}
