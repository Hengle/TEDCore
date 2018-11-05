using UnityEngine.UI;
using System.Collections.Generic;

namespace TEDCore.UnitTesting
{
    public class TestDropdownElement : BaseUnitTestingElement
    {
        private List<Dropdown.OptionData> m_optionData;

        public override void SetData(BaseUnitTesting baseUnitTesting, UnitTestingData unitTestingData)
        {
            m_baseUnitTesting = baseUnitTesting;
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
            if (null == m_baseUnitTesting)
            {
                return;
            }

            object[] data = new object[]{ m_optionData[value] };

            m_baseUnitTesting.RunTestMethod(m_unitTestingData.MethodName, data);
        }
    }
}