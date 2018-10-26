using UnityEngine;
using TEDCore;
using UnityEngine.SceneManagement;

namespace TEDCore.AssetBundle
{
    public class AssetBundleLoadSceneRequest : AssetBundleLoadRequest
    {
        private string m_assetBundleName;
        private string m_sceneName;
        private bool m_isAdditive;
        private string m_downloadingError;
        protected AsyncOperation m_request;

        public AssetBundleLoadSceneRequest(string assetBundleName, string sceneName, bool isAdditive)
        {
            m_assetBundleName = assetBundleName;
            m_sceneName = sceneName;
            m_isAdditive = isAdditive;
        }


        public override bool Update()
        {
            if (null != m_request)
            {
                return false;
            }

            var bundle = AssetBundleSystem.Instance.GetLoadedAssetBundle(m_assetBundleName, out m_downloadingError);
            if (null != bundle)
            {
                if (m_isAdditive)
                {
                    m_request = SceneManager.LoadSceneAsync(m_sceneName, LoadSceneMode.Additive);
                }
                else
                {
                    m_request = SceneManager.LoadSceneAsync(m_sceneName);
                }

                return false;
            }
            else
            {
                return true;
            }
        }


        public override bool IsDone()
        {
            if (null == m_request && null != m_downloadingError)
            {
                Debug.LogError(m_downloadingError);
                return true;
            }

            return null != m_request && m_request.isDone;
        }
    }
}