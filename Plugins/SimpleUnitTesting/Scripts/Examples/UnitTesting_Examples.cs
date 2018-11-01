using TEDCore;
using UnityEngine.UI;
using TEDCore.UnitTesting;

public class UnitTesting_Examples : BaseUnitTesting
{
    [TestButton]
    public void TestButton()
    {
        TEDDebug.LogError("UnitTesting_Examples.TestButton");
    }

    [TestDropdown("A", "B", "C", "D")]
    public void TestDropDown(Dropdown.OptionData data)
    {
        TEDDebug.LogError("UnitTesting_Example.TestDropDown = " + data.text);
    }

    [TestInputField]
    public void TestInputField(string value)
    {
        TEDDebug.LogError("UnitTesting_Example.TestInputField = " + value);
    }

    [TestSlider]
    public void TestSlider(float value)
    {
        TEDDebug.LogError("UnitTesting_Example.TestSlider = " + value);
    }

    [TestSlider(0, 10, 5)]
    public void TestSliderWithInitialValue(float value)
    {
        TEDDebug.LogError("UnitTesting_Example.TestSliderWithInitialValue = " + value);
    }

    [TestToggle]
    public void TestToggle(bool value)
    {
        TEDDebug.LogError("UnitTesting_Example.TestToggle = " + value);
    }

    [TestToggle(true)]
    public void TestToggleWithInitialTrue(bool value)
    {
        TEDDebug.LogError("UnitTesting_Example.TestToggleWithInitialTrue = " + value);
    }
}
