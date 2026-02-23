using System;
using Player;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Shared
{
    public class CashRibbonButton : MonoBehaviour
    {
        [SerializeField] private IntValueView text;
        [SerializeField] private Button button;
        private PlayerDataService _playerData;

        [Inject]
        public void Construct(PlayerDataService playerData)
        {
            _playerData = playerData;
        }

        private void OnEnable()
        {
            text.Show(_playerData.GetCash());
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            //todo
        }
    }
}