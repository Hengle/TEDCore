using UnityEngine.UI;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatSliderElement : CheatElement
    {
        private Text m_valueText;

        public override void SetData(CheatMenuOptions cheatMenuOptions, CheatMenuData unitTestingData)
        {
            m_cheatMenuOptions = cheatMenuOptions;
            m_unitTestingData = unitTestingData;

            gameObject.transform.Find("TitleText").GetComponent<Text>().text = m_unitTestingData.MethodName.ToScriptName();
            m_valueText = gameObject.transform.Find("ValueText").GetComponent<Text>();

            CheatSlider attribute = m_unitTestingData.Attribute as CheatSlider;

            UnityEngine.UI.Slider slider = GetComponentInChildren<UnityEngine.UI.Slider>();
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(OnValueChanged);
            slider.minValue = attribute.MinValue;
            slider.maxValue = attribute.MaxValue;
            slider.value = attribute.InitValue;
            m_valueText.text = attribute.InitValue.ToString();
        }

        public void OnValueChanged(float value)
        {
            if (null == m_cheatMenuOptions)
            {
                return;
            }

            object[] data = new object[]{ value };

            m_cheatMenuOptions.RunTestMethod(m_unitTestingData.MethodName, data);
            m_valueText.text = value.ToString();
        }
    }
}