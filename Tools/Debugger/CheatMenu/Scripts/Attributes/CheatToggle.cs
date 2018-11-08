using System;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatToggle : Attribute
    {
        public bool IsOn;

        public CheatToggle(bool isOn = false)
        {
            IsOn = isOn;
        }
    }
}