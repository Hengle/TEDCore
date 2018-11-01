using UnityEngine;
using System;
using TEDCore.Resource;
using TEDCore.Utils;

namespace TEDCore.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private const string SHOW_UI_LAYER = "UI";
        private const string HIDE_UI_LAYER = "HideUI";

        private LayerMask m_showUILayer;
        private LayerMask m_hideUILayer;
        private Transform m_uiCanvas;

        public UIManager()
        {
            m_showUILayer = LayerMask.NameToLayer(SHOW_UI_LAYER);
            m_hideUILayer = LayerMask.NameToLayer(HIDE_UI_LAYER);
            m_uiCanvas = GameObject.Find("UICanvas").transform;
        }

        public void LoadUIAsync<T>(string assetName, Action<T> callback) where T : MonoBehaviour
        {
            LoadUIAsync<T>(string.Empty, assetName, callback);
        }

        public void LoadUIAsync<T>(string bundleName, string assetName, Action<T> callback) where T : MonoBehaviour
        {
            ResourceManager.Instance.LoadAsync<GameObject>(bundleName, assetName, delegate(GameObject obj)
                {
                    GameObject screen = GameObject.Instantiate(obj);
                    screen.transform.SetParent(m_uiCanvas, false);

                    if (null != callback)
                    {
                        callback(screen.GetComponent<T>());
                    }
                });
        }

        public void DestroyUI(GameObject ui)
        {
            GameObject.Destroy(ui);
        }

        public void SetUIActive(GameObject ui, bool active)
        {
            if (active)
            {
                ui.SetLayer(m_showUILayer);
                ui.transform.SetAsLastSibling();
            }
            else
            {
                ui.SetLayer(m_hideUILayer);
                ui.transform.SetAsFirstSibling();
            }
        }
    }
}