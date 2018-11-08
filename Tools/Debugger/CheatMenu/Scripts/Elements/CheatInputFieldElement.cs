using UnityEngine.UI;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatInputFieldElement : CheatElement
    {
        private string m_inputFieldValue;

        public override void SetData(CheatMenuOptions cheatMenuOptions, CheatMenuData unitTestingData)
        {
            m_cheatMenuOptions = cheatMenuOptions;
            m_unitTestingData = unitTestingData;

            gameObject.transform.Find("TitleText").GetComponent<Text>().text = m_unitTestingData.MethodName.ToScriptName();

            GetComponentInChildren<UnityEngine.UI.InputField>().onValueChanged.RemoveAllListeners();
            GetComponentInChildren<UnityEngine.UI.InputField>().onValueChanged.AddListener(this.OnValueChanged);

            GetComponentInChildren<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
            GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(this.OnApplyButtonClick);
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