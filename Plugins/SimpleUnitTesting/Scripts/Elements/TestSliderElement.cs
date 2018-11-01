using UnityEngine.UI;

namespace TEDCore.UnitTesting
{
    public class TestSliderElement : BaseUnitTestingElement
    {
        private Text m_valueText;

        public override void SetData(BaseUnitTesting baseUnitTesting, UnitTestingData unitTestingData)
        {
            m_baseUnitTesting = baseUnitTesting;
            m_unitTestingData = unitTestingData;

            gameObject.transform.Find("TitleText").GetComponent<Text>().text = m_unitTestingData.MethodName.ToScriptName();
            m_valueText = gameObject.transform.Find("ValueText").GetComponent<Text>();

            TestSlider attribute = m_unitTestingData.Attribute as TestSlider;

            Slider slider = GetComponentInChildren<Slider>();
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(OnValueChanged);
            slider.minValue = attribute.MinValue;
            slider.maxValue = attribute.MaxValue;
            slider.value = attribute.InitValue;
            m_valueText.text = attribute.InitValue.ToString();
        }

        public void OnValueChanged(float value)
        {
            if (null == m_baseUnitTesting)
            {
                return;
            }

            object[] data = new object[]{ value };

            m_baseUnitTesting.RunTestMethod(m_unitTestingData.MethodName, data);
            m_valueText.text = value.ToString();
        }
    }
}