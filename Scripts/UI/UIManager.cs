using UnityEngine;
using TEDCore.Resource;

namespace TEDCore.UI
{
    public class UIManager : MonoBehaviour
    {
        private const string SHOW_UI_LAYER = "UI";
        private const string HIDE_UI_LAYER = "HideUI";

        private LayerMask m_showUILayer;
        private LayerMask m_hideUILayer;
        private Transform m_canvas;

        private void Awake()
        {
            m_showUILayer = LayerMask.NameToLayer(SHOW_UI_LAYER);
            m_hideUILayer = LayerMask.NameToLayer(HIDE_UI_LAYER);
            m_canvas = GameObject.Find("Canvas").transform;

            if (null == m_canvas)
            {
                TEDDebug.LogError("[UIManager] - Can't find Canvas.");
            }
        }


        public void LoadScreenAsync<T>(string bundleName, string assetName, System.Action<T> callback) where T : MonoBehaviour
        {
            ResourceSystem.Instance.LoadAsync<GameObject>(bundleName, assetName, delegate(GameObject obj)
                {
                    GameObject screen = GameObject.Instantiate(obj);
                    screen.transform.SetParent(m_canvas, false);

                    ResourceSystem.Instance.Unload<GameObject>(bundleName, assetName);

                    if(null != callback)
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
                screen.layer = m_showUILayer;
                screen.transform.SetAsLastSibling();
            }
            else
            {
                screen.layer = m_hideUILayer;
                screen.transform.SetAsFirstSibling();
            }
        }
    }
}