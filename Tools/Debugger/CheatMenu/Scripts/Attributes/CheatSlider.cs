using System;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatSlider : Attribute
    {
        public float MinValue;
        public float MaxValue;
        public float InitValue;

        public CheatSlider(float minValue = 0, float maxValue = 1, float initValue = 0)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            InitValue = initValue;
        }
    }
}