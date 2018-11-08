using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TEDCore.UnitTesting
{
    public class UnitTestingGenerator : MonoBehaviour
    {
        [SerializeField] private UnitTestingTitle m_unitTestingTitle;

        [Header("Template References")]
        [SerializeField] private UnitTestingPage m_templateUnitTestingPage;
        [SerializeField] private GameObject m_templateParent;
        [SerializeField] private Text m_templateTitle;
        [SerializeField] private GameObject m_templateSpace;

        [Header("Template Unit Test Elements")]
        [SerializeField] private TestButtonElement m_templateTestButtonElement;
        [SerializeField] private TestDropdownElement m_templateTestDropdownElement;
        [SerializeField] private TestInputFieldElement m_templateTestInputFieldElement;
        [SerializeField] private TestSliderElement m_templateTestSliderElement;
        [SerializeField] private TestToggleElement m_templateTestToggleElement;

        private CheatMenuOptions m_cheatMenuOptions;
        private List<UnitTestingPage> m_unitTestingPages;
        private float m_maxWidth;
        private float m_spaceWidth;

        private void Awake()
        {
            m_spaceWidth = m_templateParent.GetComponent<GridLayoutGroup>().spacing.x;

            m_cheatMenuOptions = new CheatMenuOptions();
            m_unitTestingPages = new List<UnitTestingPage>();
            string[] categories = m_cheatMenuOptions.GetAllCategories();
            for (int i = 0; i < categories.Length; i++)
            {
                UnitTestingPage unitTestingPage = Instantiate(m_templateUnitTestingPage);
                unitTestingPage.transform.SetParent(transform, false);
                unitTestingPage.Title = categories[i];
                unitTestingPage.gameObject.SetActive(true);
                m_unitTestingPages.Add(unitTestingPage);
            }

            m_unitTestingTitle.SetData(m_unitTestingPages);

            StartCoroutine(GeneratePages());
        }

        private IEnumerator GeneratePages()
        {
            for (int i = 0; i < m_unitTestingPages.Count; i++)
            {
                yield return StartCoroutine(Generate(m_unitTestingPages[i]));
            }
        }

        private IEnumerator Generate(UnitTestingPage unitTestingPage)
        {
            if (null == unitTestingPage)
            {
                TEDDebug.LogError("[UnitTestingGenerator] unitTestingObject is null.");
                yield break;
            }

            Clear(unitTestingPage);

            GameObject cacheInstance = GenerateTitle(unitTestingPage.Title, unitTestingPage);

            if (m_maxWidth == 0)
            {
                yield return new WaitForEndOfFrame();

                m_maxWidth = cacheInstance.GetComponent<RectTransform>().sizeDelta.x;
                m_maxWidth -= unitTestingPage.ContentRoot.GetComponent<VerticalLayoutGroup>().padding.left * 2;
            }

            GameObject cacheParent = GenerateLayoutGroup(2, unitTestingPage);
            GenerateElements<TestButton>(unitTestingPage, m_templateTestButtonElement, cacheParent.transform);
            GenerateElements<TestToggle>(unitTestingPage, m_templateTestToggleElement, cacheParent.transform);

            if (cacheParent.transform.childCount == 0)
            {
                Destroy(cacheParent);
            }

            cacheParent = GenerateLayoutGroup(1, unitTestingPage);
            GenerateElements<TestInputField>(unitTestingPage, m_templateTestInputFieldElement, cacheParent.transform);
            GenerateElements<TestSlider>(unitTestingPage, m_templateTestSliderElement, cacheParent.transform);
            GenerateElements<TestDropdown>(unitTestingPage, m_templateTestDropdownElement, cacheParent.transform);

            if (cacheParent.transform.childCount == 0)
            {
                Destroy(cacheParent);
            }

            GenerateEmptySpace(unitTestingPage);
        }

        private void Clear(UnitTestingPage unitTestingPage)
        {
            foreach (Transform child in unitTestingPage.ContentRoot)
            {
                Destroy(child.gameObject);
            }
        }

        private GameObject GenerateTitle(string title, UnitTestingPage unitTestingPage)
        {
            GameObject cacheInstance = Instantiate(m_templateTitle.gameObject, unitTestingPage.ContentRoot);
            cacheInstance.GetComponent<Text>().text = title;
            cacheInstance.SetActive(true);

            return cacheInstance;
        }

        private GameObject GenerateLayoutGroup(int division, UnitTestingPage unitTestingPage)
        {
            GameObject cacheInstance = Instantiate(m_templateParent.gameObject, unitTestingPage.ContentRoot);
            cacheInstance.GetComponent<GridLayoutGroup>().cellSize = new Vector2(m_maxWidth / division - m_spaceWidth, 100);
            cacheInstance.SetActive(true);

            return cacheInstance;
        }

        private void GenerateElements<T>(UnitTestingPage unitTestingPage, BaseUnitTestingElement baseUnitTestingElement, Transform parent) where T : System.Attribute
        {
            UnitTestingData[] unitTestingDatas = m_cheatMenuOptions.GetUnitTestingData<T>(unitTestingPage.Title);

            if (null == unitTestingDatas ||
                unitTestingDatas.Length < 1)
            {
                return;
            }

            BaseUnitTestingElement cacheData = null;
            for (int j = 0; j < unitTestingDatas.Length; j++)
            {
                cacheData = Instantiate(baseUnitTestingElement, parent);
                cacheData.gameObject.SetActive(true);
                cacheData.SetData(m_cheatMenuOptions, unitTestingDatas[j]);
            }
        }

        private void GenerateEmptySpace(UnitTestingPage unitTestingPage)
        {
            GameObject cacheInstance = Instantiate(m_templateSpace.gameObject, unitTestingPage.ContentRoot);
            cacheInstance.SetActive(true);
        }
    }
}
