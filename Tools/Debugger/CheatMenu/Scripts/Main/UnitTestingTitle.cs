using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TEDCore.UnitTesting;

public class UnitTestingTitle : MonoBehaviour
{
    [SerializeField] private Dropdown m_dropdown;
    private List<UnitTestingPage> m_pages;
    private List<Dropdown.OptionData> m_optionDatas;

    public void SetData(List<UnitTestingPage> unitTestingPages)
    {
        m_pages = unitTestingPages;
        m_optionDatas = new List<Dropdown.OptionData>();

        for (int i = 0; i < unitTestingPages.Count; i++)
        {
            m_optionDatas.Add(new Dropdown.OptionData(unitTestingPages[i].Title));
        }

        m_dropdown.onValueChanged.RemoveAllListeners();
        m_dropdown.onValueChanged.AddListener(OnValueChange);
        m_dropdown.ClearOptions();
        m_dropdown.AddOptions(m_optionDatas);

        OnValueChange(0);
    }

    public void OnValueChange(int value)
    {
        for (int i = 0; i < m_pages.Count; i++)
        {
            m_pages[i].gameObject.SetActive(false);
        }

        m_pages[value].gameObject.SetActive(true);
    }
}
