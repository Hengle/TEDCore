using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.UnitTesting
{
    public class TestInputFieldElement : MonoBehaviour
    {
        private string m_inputFieldValue;
        private BaseUnitTesting m_unitTesting;
        private UnitTestingData m_unitTestingData;

        public void SetData(BaseUnitTesting unitTesting, UnitTestingData unitTestingData)
        {
            m_unitTesting = unitTesting;
            m_unitTestingData = unitTestingData;

            gameObject.transform.Find("TitleText").GetComponent<Text>().text = m_unitTestingData.MethodName.ToScriptName();

            GetComponentInChildren<InputField>().onValueChanged.RemoveAllListeners();
            GetComponentInChildren<InputField>().onValueChanged.AddListener(OnValueChanged);

            GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            GetComponentInChildren<Button>().onClick.AddListener(OnApplyButtonClick);
        }

        public void OnValueChanged(string value)
        {
            if (null == m_unitTesting)
            {
                return;
            }

            m_inputFieldValue = value;
        }

        private void OnApplyButtonClick()
        {
            if (null == m_unitTesting)
            {
                return;
            }

            object[] data = new object[] { m_inputFieldValue };

            m_unitTesting.RunTestMethod(m_unitTestingData.MethodName, data);
        }
    }
}