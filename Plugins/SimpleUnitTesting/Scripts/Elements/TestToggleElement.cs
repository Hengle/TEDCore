using UnityEngine.UI;

namespace TEDCore.UnitTesting
{
    public class TestToggleElement : BaseUnitTestingElement
    {
        public override void SetData(BaseUnitTesting baseUnitTesting, UnitTestingData unitTestingData)
        {
            m_baseUnitTesting = baseUnitTesting;
            m_unitTestingData = unitTestingData;

            gameObject.transform.Find("TitleText").GetComponent<Text>().text = m_unitTestingData.MethodName.ToScriptName();

            TestToggle attribute = m_unitTestingData.Attribute as TestToggle;

            Toggle toggle = GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(OnToggleChanged);
            toggle.isOn = attribute.IsOn;
        }

        public void OnToggleChanged(bool value)
        {
            if (null == m_baseUnitTesting)
            {
                return;
            }

            object[] data = new object[]{ value };

            m_baseUnitTesting.RunTestMethod(m_unitTestingData.MethodName, data);
        }
    }
}