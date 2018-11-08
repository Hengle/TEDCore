using UnityEngine.UI;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatButtonElement : CheatElement
    {
        public override void SetData(CheatMenuOptions cheatMenuOptions, CheatMenuData unitTestingData)
        {
            m_cheatMenuOptions = cheatMenuOptions;
            m_unitTestingData = unitTestingData;

            gameObject.transform.Find("TitleText").GetComponent<Text>().text = m_unitTestingData.MethodName.ToScriptName();

            Button button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClicked);
        }

        public void OnButtonClicked()
        {
            if (null == m_cheatMenuOptions)
            {
                return;
            }

            m_cheatMenuOptions.RunTestMethod(m_unitTestingData.MethodName);
        }
    }
}