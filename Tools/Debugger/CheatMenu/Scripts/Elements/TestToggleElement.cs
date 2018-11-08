using UnityEngine.UI;

namespace TEDCore.UnitTesting
{
    public class TestToggleElement : BaseUnitTestingElement
    {
        public override void SetData(CheatMenuOptions cheatMenuOptions, UnitTestingData unitTestingData)
        {
            m_cheatMenuOptions = cheatMenuOptions;
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
            if (null == m_cheatMenuOptions)
            {
                return;
            }

            object[] data = { value };

            m_cheatMenuOptions.RunTestMethod(m_unitTestingData.MethodName, data);
        }
    }
}