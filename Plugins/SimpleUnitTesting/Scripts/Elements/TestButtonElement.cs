using UnityEngine.UI;

namespace TEDCore.UnitTesting
{
    public class TestButtonElement : BaseUnitTestingElement
    {
        public override void SetData(BaseUnitTesting baseUnitTesting, UnitTestingData unitTestingData)
        {
            m_baseUnitTesting = baseUnitTesting;
            m_unitTestingData = unitTestingData;

            gameObject.transform.Find("TitleText").GetComponent<Text>().text = m_unitTestingData.MethodName.ToScriptName();

            Button button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClicked);
        }

        public void OnButtonClicked()
        {
            if (null == m_baseUnitTesting)
            {
                return;
            }

            m_baseUnitTesting.RunTestMethod(m_unitTestingData.MethodName);
        }
    }
}