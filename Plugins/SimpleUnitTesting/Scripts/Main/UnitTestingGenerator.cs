﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.UnitTesting
{
    public class UnitTestingGenerator : MonoBehaviour
    {
        [SerializeField] private Button m_closeButton;
        [SerializeField] private GameObject m_unitTestingObject;
        [SerializeField] private Transform m_contentRoot;

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
            if (null != m_closeButton)
            {
                if (null != GetComponentInParent<UnitTestingDragHelper>())
                {
                    m_closeButton.onClick.AddListener(GetComponentInParent<UnitTestingDragHelper>().OnFullScreenCloseButtonClicked);
                }
            }

            m_spaceWidth = m_templateParent.GetComponent<GridLayoutGroup>().spacing.x;

            StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            Clear();

            if (null == m_unitTestingObject)
            {
                TEDDebug.LogError("[UnitTestingGenerator] m_unitTestingObject is null.");
                yield break;
            }

            m_unitTestings = m_unitTestingObject.GetComponents<BaseUnitTesting>();
            if (null == m_unitTestings ||
                m_unitTestings.Length < 1)
            {
                TEDDebug.LogError("[UnitTestingGenerator] No unit testing scripts in \"" + m_unitTestingObject.name + "\"");
                yield break;
            }

            GameObject cacheInstance = null;

            for (int i = 0; i < m_unitTestings.Length; i++)
            {
                if (!m_unitTestings[i].enabled)
                {
                    continue;
                }

                cacheInstance = GenerateTitle(m_unitTestings[i].GetType().Name);

                if (m_maxWidth == 0)
                {
                    yield return new WaitForEndOfFrame();

                    m_maxWidth = cacheInstance.GetComponent<RectTransform>().sizeDelta.x;
                    m_maxWidth -= m_contentRoot.GetComponent<VerticalLayoutGroup>().padding.left * 2;
                }

                GameObject cacheParent = GenerateLayoutGroup(2);
                GenerateElements<TestButton>(m_unitTestings[i], m_templateTestButtonElement, cacheParent.transform);
                GenerateElements<TestToggle>(m_unitTestings[i], m_templateTestToggleElement, cacheParent.transform);

                if (cacheParent.transform.childCount == 0)
                {
                    Destroy(cacheParent);
                }

                cacheParent = GenerateLayoutGroup(1);
                GenerateElements<TestInputField>(m_unitTestings[i], m_templateTestInputFieldElement, cacheParent.transform);
                GenerateElements<TestSlider>(m_unitTestings[i], m_templateTestSliderElement, cacheParent.transform);
                GenerateElements<TestDropdown>(m_unitTestings[i], m_templateTestDropdownElement, cacheParent.transform);

                if (cacheParent.transform.childCount == 0)
                {
                    Destroy(cacheParent);
                }

                GenerateEmptySpace();
            }
        }

        private void Clear()
        {
            foreach (Transform child in m_contentRoot)
            {
                Destroy(child.gameObject);
            }
        }

        private GameObject GenerateTitle(string title)
        {
            GameObject cacheInstance = Instantiate<GameObject>(m_templateTitle.gameObject, m_contentRoot);
            cacheInstance.GetComponent<Text>().text = title;
            cacheInstance.SetActive(true);

            return cacheInstance;
        }

        private GameObject GenerateLayoutGroup(int division)
        {
            GameObject cacheInstance = Instantiate<GameObject>(m_templateParent.gameObject, m_contentRoot);
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

        private void GenerateEmptySpace()
        {
            GameObject cacheInstance = Instantiate<GameObject>(m_templateSpace.gameObject, m_contentRoot);
            cacheInstance.SetActive(true);
        }
    }
}
