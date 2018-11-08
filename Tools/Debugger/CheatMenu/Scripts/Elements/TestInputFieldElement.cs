using UnityEngine.UI;

namespace TEDCore.UnitTesting
{
    public class TestInputFieldElement : BaseUnitTestingElement
    {
        private string m_inputFieldValue;

        public override void SetData(CheatMenuOptions cheatMenuOptions, UnitTestingData unitTestingData)
        {
            m_cheatMenuOptions = cheatMenuOptions;
            m_unitTestingData = unitTestingData;

            gameObject.transform.Find("TitleText").GetComponent<Text>().text = m_unitTestingData.MethodName.ToScriptName();

            GetComponentInChildren<InputField>().onValueChanged.RemoveAllListeners();
            GetComponentInChildren<InputField>().onValueChanged.AddListener(OnValueChanged);

            GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            GetComponentInChildren<Button>().onClick.AddListener(OnApplyButtonClick);
        }

        public void OnValueChanged(string value)
        {
            if (null == m_cheatMenuOptions)
            {
                return;
            }

            m_inputFieldValue = value;
        }

        private void OnApplyButtonClick()
        {
            if (null == m_cheatMenuOptions)
            {
                return;
            }

            object[] data = new object[] { m_inputFieldValue };

            m_cheatMenuOptions.RunTestMethod(m_unitTestingData.MethodName, data);
        }
    }
}