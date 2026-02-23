using System;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Abilities
{
    public class AbilityHUDIcon : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private IntValueView counter;

        private void Awake()
        {
            counter.SetAdditionalConversion(text => text == "1" ? "" : text);
        }

        public void Show(Sprite sprite, int count)
        {
            image.sprite = sprite;
            counter.Show(count);
        }
    }
}