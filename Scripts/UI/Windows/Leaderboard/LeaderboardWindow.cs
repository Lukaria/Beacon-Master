using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Leaderboard;
using Lighthouse.Pool;
using R3;
using TMPro;
using UI.BottomBar;
using UI.Common;
using UI.Windows.Abilities;
using UI.Windows.Common;
using UI.Windows.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using LocationService = Location.LocationService;

namespace UI.Windows.Leaderboard
{
    public class LeaderboardWindow : WindowBase
    {
        [SerializeField] private FloatValueView scoreText;
        [SerializeField] private TimeValueView timeText;
        [SerializeField] private TMP_Text lighthouseText;
        [SerializeField] private TMP_Text locationText;
        [SerializeField] private AbilitiesGridBehaviour abilitiesGridBehaviour;
        [SerializeField] private Button closeButton;
        
        private IDataService<LeaderboardDataDto> _dataService;
        private BottomBarController _bottomBar;
        private LighthouseService _lighthouseService;
        private LocationService _locationService;
        private IUiNavigation _navigation;
        public override WindowId Id => WindowId.Leaderboard;

        [Inject]
        public void Construct(
            IUiNavigation navigation,
            IDataService<LeaderboardDataDto> dataService,
            LighthouseService lighthouseService,
            LocationService locationService,
            BottomBarController bottomBar)
        {
            _navigation = navigation;
            _dataService = dataService;
            _lighthouseService = lighthouseService;
            _locationService = locationService;
            _bottomBar = bottomBar;
            
            closeButton.OnClickAsObservable()
                .SubscribeAwait(async (_, ct) => 
                {
                    await _navigation.GoBackAsync();
                })
                .AddTo(this);
        }

        private void OnEnable()
        {
            _bottomBar.Hide();
            UpdateLeaderboard();
        }
        
        private void OnDisable()
        {
            _bottomBar.Show();
        }


        private void UpdateLeaderboard()
        {
            var data = _dataService.GetData();
            scoreText.Show(data.Score);
            timeText.Show(data.Time);
            lighthouseText.text = $"{_lighthouseService.GetLighthouseConfig(data.Lighthouse).Title}";
            locationText.text = $"{_locationService.GetLocationConfig(data.Location).Title}";
            abilitiesGridBehaviour.RedrawItems(data.Abilities);
        }
    }
}