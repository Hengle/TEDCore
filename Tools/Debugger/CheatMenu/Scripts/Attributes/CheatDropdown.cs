using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatDropdown : Attribute
    {
        public List<Dropdown.OptionData> OptionData;

        public CheatDropdown(params string[] optionText)
        {
            OptionData = new List<Dropdown.OptionData>();

            for (int i = 0; i < optionText.Length; i++)
            {
                OptionData.Add(new Dropdown.OptionData(optionText[i]));
            }
        }
    }
}