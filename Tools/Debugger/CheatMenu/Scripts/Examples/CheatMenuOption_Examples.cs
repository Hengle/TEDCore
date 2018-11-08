using UnityEngine.UI;

namespace TEDCore.Debugger.CheatMenu
{
    public partial class CheatMenuOptions
    {
        [CheatCategory("Test Category"), CheatButton]
        public void TestCategoryButton()
        {
            TEDDebug.LogError("UnitTesting_Examples.TestCategoryButton");
        }

        [CheatButton]
        public void TestButton()
        {
            TEDDebug.LogError("UnitTesting_Examples.TestButton");
        }

        [CheatDropdown("A", "B", "C", "D")]
        public void TestDropDown(Dropdown.OptionData data)
        {
            TEDDebug.LogError("UnitTesting_Example.TestDropDown = " + data.text);
        }

        [CheatInputField]
        public void TestInputField(string value)
        {
            TEDDebug.LogError("UnitTesting_Example.TestInputField = " + value);
        }

        [CheatSlider]
        public void TestSlider(float value)
        {
            TEDDebug.LogError("UnitTesting_Example.TestSlider = " + value);
        }

        [CheatSlider(0, 10, 5)]
        public void TestSliderWithInitialValue(float value)
        {
            TEDDebug.LogError("UnitTesting_Example.TestSliderWithInitialValue = " + value);
        }

        [CheatToggle]
        public void TestToggle(bool value)
        {
            TEDDebug.LogError("UnitTesting_Example.TestToggle = " + value);
        }

        [CheatToggle(true)]
        public void TestToggleWithInitialTrue(bool value)
        {
            TEDDebug.LogError("UnitTesting_Example.TestToggleWithInitialTrue = " + value);
        }
    }
}