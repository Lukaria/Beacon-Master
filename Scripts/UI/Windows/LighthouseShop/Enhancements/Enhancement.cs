using System;
using TMPro;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.LighthouseShop.Enhancements
{
    public class Enhancement : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private Button enhancementButton;
        [SerializeField] private IntValueView price;
        [SerializeField] private Image progressImage;
        
        public Action EnhancementButtonClicked { get; set; }
        
        public void Awake()
        {
            Hide();
        }

        public void Hide()
        {
            price.Hide();
            title.gameObject.SetActive(false);
            progressImage.gameObject.SetActive(false);
            enhancementButton.gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            enhancementButton.onClick.AddListener(OnButtonClicked);
        }

        public void OnDisable()
        {
            enhancementButton.onClick.RemoveListener(OnButtonClicked);
        }
        
        private void OnButtonClicked()
        {
            EnhancementButtonClicked?.Invoke();
        }

        public void Show(EnhancementDto dto)
        {
            title.gameObject.SetActive(true);
            progressImage.gameObject.SetActive(true);
            progressImage.fillAmount = (float) dto.Level / dto.MaxLevel;

            return;
            if (dto.Level == dto.MaxLevel)
            {
                price.Hide();
                enhancementButton.gameObject.SetActive(false);
            }
            else
            {
                price.Show(dto.Price);
                enhancementButton.gameObject.SetActive(true);
            }
        }
    }
}