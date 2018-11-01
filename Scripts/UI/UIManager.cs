using UnityEngine;
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


        public void LoadScreenAsync<T>(string assetName, System.Action<T> callback) where T : MonoBehaviour
        {
            ResourceManager.Instance.LoadAsync<GameObject>(assetName, delegate (GameObject obj)
            {
                GameObject screen = GameObject.Instantiate(obj);
                screen.transform.SetParent(m_uiCanvas, false);

                if (null != callback)
                {
                    callback(screen.GetComponent<T>());
                }
            });
        }


        public void LoadScreenAsync<T>(string bundleName, string assetName, System.Action<T> callback) where T : MonoBehaviour
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


        public void DestoryScreen(GameObject screen)
        {
            GameObject.Destroy(screen);
        }


        public void ShowScreen(GameObject screen, bool show)
        {
            if (show)
            {
                screen.SetLayer(m_showUILayer);
                screen.transform.SetAsLastSibling();
            }
            else
            {
                screen.SetLayer(m_hideUILayer);
                screen.transform.SetAsFirstSibling();
            }
        }
    }
}