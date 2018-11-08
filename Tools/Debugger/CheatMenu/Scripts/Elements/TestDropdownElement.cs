using UnityEngine.UI;
using System.Collections.Generic;

namespace TEDCore.UnitTesting
{
    public class TestDropdownElement : BaseUnitTestingElement
    {
        private List<Dropdown.OptionData> m_optionData;

        public override void SetData(CheatMenuOptions cheatMenuOptions, UnitTestingData unitTestingData)
        {
            m_cheatMenuOptions = cheatMenuOptions;
            m_unitTestingData = unitTestingData;

            gameObject.transform.Find("TitleText").GetComponent<Text>().text = m_unitTestingData.MethodName.ToScriptName();

            TestDropdown attribute = m_unitTestingData.Attribute as TestDropdown;
            m_optionData = attribute.OptionData;

            Dropdown dropdown = GetComponentInChildren<Dropdown>();
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.onValueChanged.AddListener(OnValueChange);
            dropdown.ClearOptions();
            dropdown.AddOptions(m_optionData);

            OnValueChange(0);
        }

        public void OnValueChange(int value)
        {
            if (null == m_cheatMenuOptions)
            {
                return;
            }

            object[] data = new object[]{ m_optionData[value] };

            m_cheatMenuOptions.RunTestMethod(m_unitTestingData.MethodName, data);
        }
    }
}