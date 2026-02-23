using System;
using UnityEngine;

namespace UI.Common
{
    public class FloatValueView : ValueView<float>
    {
        [SerializeField] private float _limitValue = 1000.0f;
        [SerializeField, Range(0, 5)] private int _decimals = 2;

        private const int _engAlphabetLettersCount = 26;
        private const int _asciiTableConstant = 96;

        protected override string Convert(float value)
        {
            var divisionCount = 0;
            
            while (value >= _limitValue)
            {
                ++divisionCount;
                value /= _limitValue;
            }

            var additionalLetters = "";
            if ( divisionCount != 0 )
            {
                additionalLetters = System.Convert.ToChar(_asciiTableConstant + divisionCount).ToString();
            }

            value = (float)Math.Round(value, _decimals);

            var valueView = value + additionalLetters;
            
            return valueView;
        }
    }
}