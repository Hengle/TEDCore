using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatMenuGenerator : MonoBehaviour
    {
        [SerializeField] private CheatMenuCategory m_cheatMenuCategory;
        [SerializeField] private Transform m_cheatMenuPageParent;

        [Header("Template References")]
        [SerializeField] private CheatMenuPage m_templateCheatMenuPage;
        [SerializeField] private GameObject m_templateParent;
        [SerializeField] private GameObject m_templateTitle;
        [SerializeField] private GameObject m_templateSpace;

        [Header("Template Cheat Elements")]
        [SerializeField] private CheatButtonElement m_templateCheatButtonElement;
        [SerializeField] private CheatDropdownElement m_templateCheatDropdownElement;
        [SerializeField] private CheatInputFieldElement m_templateCheatInputFieldElement;
        [SerializeField] private CheatSliderElement m_templateCheatSliderElement;
        [SerializeField] private CheatToggleElement m_templateCheatToggleElement;

        private CheatMenuOptions m_cheatMenuOptions;
        private List<CheatMenuPage> m_cheatMenuPages;
        private float m_maxWidth;
        private float m_spaceWidth;

        private void Awake()
        {
            m_spaceWidth = m_templateParent.GetComponent<GridLayoutGroup>().spacing.x;

            m_cheatMenuOptions = new CheatMenuOptions();
            m_cheatMenuPages = new List<CheatMenuPage>();
            string[] categories = m_cheatMenuOptions.GetAllCategories();
            for (int i = 0; i < categories.Length; i++)
            {
                CheatMenuPage unitTestingPage = Instantiate(m_templateCheatMenuPage);
                unitTestingPage.transform.SetParent(m_cheatMenuPageParent, false);
                unitTestingPage.Title = categories[i];
                unitTestingPage.gameObject.SetActive(true);
                m_cheatMenuPages.Add(unitTestingPage);
            }

            m_cheatMenuCategory.SetData(m_cheatMenuPages);

            StartCoroutine(GeneratePages());
        }

        private IEnumerator GeneratePages()
        {
            for (int i = 0; i < m_cheatMenuPages.Count; i++)
            {
                yield return StartCoroutine(Generate(m_cheatMenuPages[i]));
            }
        }

        private IEnumerator Generate(CheatMenuPage cheatMenuPage)
        {
            if (null == cheatMenuPage)
            {
                TEDDebug.LogError("[UnitTestingGenerator] unitTestingObject is null.");
                yield break;
            }

            Clear(cheatMenuPage);

            GameObject cacheInstance = GenerateTitle(cheatMenuPage);

            if (m_maxWidth == 0)
            {
                yield return new WaitForEndOfFrame();

                m_maxWidth = cacheInstance.GetComponent<RectTransform>().sizeDelta.x;
                m_maxWidth -= cheatMenuPage.ContentRoot.GetComponent<VerticalLayoutGroup>().padding.left * 2;
            }

            GameObject cacheParent = GenerateLayoutGroup(2, cheatMenuPage);
            GenerateElements<CheatButton>(cheatMenuPage, m_templateCheatButtonElement, cacheParent.transform);
            GenerateElements<CheatToggle>(cheatMenuPage, m_templateCheatToggleElement, cacheParent.transform);

            if (cacheParent.transform.childCount == 0)
            {
                Destroy(cacheParent);
            }

            cacheParent = GenerateLayoutGroup(1, cheatMenuPage);
            GenerateElements<CheatInputField>(cheatMenuPage, m_templateCheatInputFieldElement, cacheParent.transform);
            GenerateElements<CheatSlider>(cheatMenuPage, m_templateCheatSliderElement, cacheParent.transform);
            GenerateElements<CheatDropdown>(cheatMenuPage, m_templateCheatDropdownElement, cacheParent.transform);

            if (cacheParent.transform.childCount == 0)
            {
                Destroy(cacheParent);
            }

            GenerateEmptySpace(cheatMenuPage);
        }

        private void Clear(CheatMenuPage cheatMenuPage)
        {
            foreach (Transform child in cheatMenuPage.ContentRoot)
            {
                Destroy(child.gameObject);
            }
        }

        private GameObject GenerateTitle(CheatMenuPage cheatMenuPage)
        {
            GameObject cacheInstance = Instantiate(m_templateTitle.gameObject, cheatMenuPage.ContentRoot);
            cacheInstance.SetActive(true);

            return cacheInstance;
        }

        private GameObject GenerateLayoutGroup(int division, CheatMenuPage cheatMenuPage)
        {
            GameObject cacheInstance = Instantiate(m_templateParent.gameObject, cheatMenuPage.ContentRoot);
            cacheInstance.GetComponent<GridLayoutGroup>().cellSize = new Vector2(m_maxWidth / division - m_spaceWidth, 100);
            cacheInstance.SetActive(true);

            return cacheInstance;
        }

        private void GenerateElements<T>(CheatMenuPage cheatMenuPage, CheatElement baseUnitTestingElement, Transform parent) where T : System.Attribute
        {
            CheatMenuData[] unitTestingDatas = m_cheatMenuOptions.GetUnitTestingData<T>(cheatMenuPage.Title);

            if (null == unitTestingDatas ||
                unitTestingDatas.Length < 1)
            {
                return;
            }

            CheatElement cacheData = null;
            for (int j = 0; j < unitTestingDatas.Length; j++)
            {
                cacheData = Instantiate(baseUnitTestingElement, parent);
                cacheData.gameObject.SetActive(true);
                cacheData.SetData(m_cheatMenuOptions, unitTestingDatas[j]);
            }
        }

        private void GenerateEmptySpace(CheatMenuPage cheatMenuPage)
        {
            GameObject cacheInstance = Instantiate(m_templateSpace.gameObject, cheatMenuPage.ContentRoot);
            cacheInstance.SetActive(true);
        }
    }
}
