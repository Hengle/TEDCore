using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatMenuTitle : MonoBehaviour
    {
        [SerializeField] private Dropdown m_dropdown;
        private List<CheatMenuPage> m_cheatMenuPages;
        private List<Dropdown.OptionData> m_optionDatas;

        public void SetData(List<CheatMenuPage> cheatMenuPages)
        {
            m_cheatMenuPages = cheatMenuPages;
            m_optionDatas = new List<Dropdown.OptionData>();

            for (int i = 0; i < cheatMenuPages.Count; i++)
            {
                m_optionDatas.Add(new Dropdown.OptionData(cheatMenuPages[i].Title));
            }

            m_dropdown.onValueChanged.RemoveAllListeners();
            m_dropdown.onValueChanged.AddListener(OnValueChange);
            m_dropdown.ClearOptions();
            m_dropdown.AddOptions(m_optionDatas);

            OnValueChange(0);
        }

        public void OnValueChange(int value)
        {
            for (int i = 0; i < m_cheatMenuPages.Count; i++)
            {
                m_cheatMenuPages[i].gameObject.SetActive(false);
            }

            m_cheatMenuPages[value].gameObject.SetActive(true);
        }
    }
}
