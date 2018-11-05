using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.UnitTesting
{
    public class UnitTestingGenerator : MonoBehaviour
    {
        [SerializeField] private UnitTestingPage m_unitTestingPage;

        [Header("Template References")]
        [SerializeField] private GameObject m_templateParent;
        [SerializeField] private Text m_templateTitle;
        [SerializeField] private GameObject m_templateSpace;

        [Header("Template Unit Test Elements")]
        [SerializeField] private TestButtonElement m_templateTestButtonElement;
        [SerializeField] private TestDropdownElement m_templateTestDropdownElement;
        [SerializeField] private TestInputFieldElement m_templateTestInputFieldElement;
        [SerializeField] private TestSliderElement m_templateTestSliderElement;
        [SerializeField] private TestToggleElement m_templateTestToggleElement;

        private float m_maxWidth;
        private float m_spaceWidth;
        private BaseUnitTesting[] m_unitTestings;

        private void Awake()
        {
            m_spaceWidth = m_templateParent.GetComponent<GridLayoutGroup>().spacing.x;

            StartCoroutine(Generate(m_unitTestingPage));
        }

        private IEnumerator Generate(UnitTestingPage unitTestingPage)
        {
            if (null == unitTestingPage)
            {
                TEDDebug.LogError("[UnitTestingGenerator] unitTestingObject is null.");
                yield break;
            }

            Clear(unitTestingPage);

            m_unitTestings = unitTestingPage.GetComponents<BaseUnitTesting>();
            if (null == m_unitTestings ||
                m_unitTestings.Length < 1)
            {
                TEDDebug.LogError("[UnitTestingGenerator] No unit testing scripts in \"" + unitTestingPage.name + "\"");
                yield break;
            }

            GameObject cacheInstance = null;

            for (int i = 0; i < m_unitTestings.Length; i++)
            {
                if (!m_unitTestings[i].enabled)
                {
                    continue;
                }

                cacheInstance = GenerateTitle(m_unitTestings[i].GetType().Name, unitTestingPage);

                if (m_maxWidth == 0)
                {
                    yield return new WaitForEndOfFrame();

                    m_maxWidth = cacheInstance.GetComponent<RectTransform>().sizeDelta.x;
                    m_maxWidth -= unitTestingPage.ContentRoot.GetComponent<VerticalLayoutGroup>().padding.left * 2;
                }

                GameObject cacheParent = GenerateLayoutGroup(2, unitTestingPage);
                GenerateElements<TestButton>(m_unitTestings[i], m_templateTestButtonElement, cacheParent.transform);
                GenerateElements<TestToggle>(m_unitTestings[i], m_templateTestToggleElement, cacheParent.transform);

                if (cacheParent.transform.childCount == 0)
                {
                    Destroy(cacheParent);
                }

                cacheParent = GenerateLayoutGroup(1, unitTestingPage);
                GenerateElements<TestInputField>(m_unitTestings[i], m_templateTestInputFieldElement, cacheParent.transform);
                GenerateElements<TestSlider>(m_unitTestings[i], m_templateTestSliderElement, cacheParent.transform);
                GenerateElements<TestDropdown>(m_unitTestings[i], m_templateTestDropdownElement, cacheParent.transform);

                if (cacheParent.transform.childCount == 0)
                {
                    Destroy(cacheParent);
                }

                GenerateEmptySpace(unitTestingPage);
            }
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
            GameObject cacheInstance = Instantiate<GameObject>(m_templateTitle.gameObject, unitTestingPage.ContentRoot);
            cacheInstance.GetComponent<Text>().text = title;
            cacheInstance.SetActive(true);

            return cacheInstance;
        }

        private GameObject GenerateLayoutGroup(int division, UnitTestingPage unitTestingPage)
        {
            GameObject cacheInstance = Instantiate<GameObject>(m_templateParent.gameObject, unitTestingPage.ContentRoot);
            cacheInstance.GetComponent<GridLayoutGroup>().cellSize = new Vector2(m_maxWidth / division - m_spaceWidth, 100);
            cacheInstance.SetActive(true);

            return cacheInstance;
        }

        private void GenerateElements<T>(BaseUnitTesting baseUnitTesting, BaseUnitTestingElement baseUnitTestingElement, Transform parent) where T : System.Attribute
        {
            UnitTestingData[] unitTestingDatas = baseUnitTesting.GetUnitTestingData<T>();

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
                cacheData.SetData(baseUnitTesting, unitTestingDatas[j]);
            }
        }

        private void GenerateEmptySpace(UnitTestingPage unitTestingPage)
        {
            GameObject cacheInstance = Instantiate<GameObject>(m_templateSpace.gameObject, unitTestingPage.ContentRoot);
            cacheInstance.SetActive(true);
        }
    }
}
