using System;

namespace UI.Common
{
    public class TimeValueView : ValueView<float>
    {
        protected override string Convert(float value)
        {
            var time = TimeSpan.FromSeconds(value);
            return time.Hours > 0 ? time.ToString(@"hh\:mm\:ss") : time.ToString(@"mm\:ss");
        }
    }
}