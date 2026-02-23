using System;
using UnityEngine;

namespace Lighthouse.Stats
{
    [Serializable]
    public struct StatData
    {
        [SerializeField, Range(1, 8)] private int baseLevel;
        [SerializeField] private int basePrice;
        [SerializeField] private float baseValue;
        [SerializeField, Range(0, 1)] private float levelBonusPercent;
        
        [Header("Progression Curves")]
        [SerializeField] private AnimationCurve upgradeCostCurve;

        public float GetCalculatedValue(int level) =>
            baseValue * (1 + levelBonusPercent * Math.Clamp(0, level - 1, 7));

        public int GetPrice(int level) =>
            Mathf.RoundToInt(basePrice + upgradeCostCurve.Evaluate(level));
        
        public int BaseLevel => baseLevel;
        public int BasePrice => basePrice;

    }
}