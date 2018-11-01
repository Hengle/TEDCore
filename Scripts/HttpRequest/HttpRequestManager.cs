using System.Collections.Generic;

namespace TEDCore.Http
{
    public class HttpRequestManager : MonoSingleton<HttpRequestManager>
    {
        public enum BackendServerType
        {
            General,
            Game
        }

        private const string HEADER_KEY = "Authorization";
        private const string SESSION_KEY = "X-Authentication";
        private const string CLIENT_VERSION_KEY = "X-Client-Version";
        private const string CONTENT_TYPE_KEY = "Content-Type";
        private const string CONTENT_TYPE = "application/json";

        private bool m_init = false;
        private string m_generalServerUrl = string.Empty;
        private string m_gameServerUrl = string.Empty;
        private string m_appId = string.Empty;
        private string m_apiVersion = string.Empty;
        private string m_gameVersion = string.Empty;
        private string m_session = string.Empty;
        private Queue<WebRequest> m_waitingRequests = new Queue<WebRequest>();
        private WebRequest m_progressingRequest;

        public void Init(string generalServerUrl, string gameServerUrl, string appId, string apiVersion, string gameVersion)
        {
            m_init = true;
            m_generalServerUrl = generalServerUrl;
            m_gameServerUrl = gameServerUrl;
            m_appId = appId;
            m_apiVersion = apiVersion;
            m_gameVersion = gameVersion;
        }

        public void Auth(object jsonData, WebRequest.OnDataCallback callback)
        {
            if (!m_init)
            {
                TEDDebug.LogWarning("[HttpRequestManager] - Need to do HttpRequestManager.Init() first.");
                return;
            }

            string url = "";
            #if UNITY_IOS
            url = GetUrl(BackendServerType.General, "/auth/sessions/device/ios");
            #else
            url = GetUrl(BackendServerType.General, "/auth/sessions/device/android");
            #endif

            WebRequest webRequest = WebRequest.Post(url, GetHeaders(true, true), jsonData, callback, null);

            if (null == webRequest)
            {
                if (null != callback)
                {
                    callback.Invoke(0, null, null);
                }

                return;
            }

            m_waitingRequests.Enqueue(webRequest);
        }

        public void SetSession(string session)
        {
            m_session = session;
        }

        private bool HasSession()
        {
            return !string.IsNullOrEmpty(m_session);
        }

        public void Post(BackendServerType serverType, string endPoint, object jsonData = null, WebRequest.OnDataCallback callback = null, object userData = null)
        {
            if (!m_init)
            {
                TEDDebug.LogWarning("[HttpRequestManager] - Need to do HttpRequestManager.Init() first.");
                return;
            }

            if (!HasSession())
            {
                TEDDebug.LogWarning("[HttpRequestManager] - The session didn't exist, need to do HttpRequstManager.Auth() and set the session first.");
                return;
            }

            string url = GetUrl(serverType, endPoint);

            WebRequest webRequest = WebRequest.Post(url, GetHeaders(true), jsonData, callback, userData);

            if (null == webRequest)
            {
                if (null != callback)
                {
                    callback.Invoke(0, null, userData);
                }

                return;
            }

            m_waitingRequests.Enqueue(webRequest);
        }

        public void Get(BackendServerType serverType, string endPoint, WebRequest.OnDataCallback callback = null, object userData = null)
        {
            if (!m_init)
            {
                TEDDebug.LogWarning("[HttpRequestManager] - Need to do HttpRequestManager.Init() first.");
                return;
            }

            if (!HasSession())
            {
                TEDDebug.LogWarning("[HttpRequestManager] - The session didn't exist, need to do HttpRequstManager.Auth() and set the session first.");
                return;
            }

            string url = GetUrl(serverType, endPoint);

            WebRequest webRequest = WebRequest.Get(url, GetHeaders(false), callback, userData);

            if (null == webRequest)
            {
                if (null != callback)
                {
                    callback.Invoke(0, null, userData);
                }

                return;
            }

            m_waitingRequests.Enqueue(webRequest);
        }

        public void Delete(BackendServerType serverType, string endPoint, WebRequest.OnDataCallback callback = null, object userData = null)
        {
            if (!m_init)
            {
                TEDDebug.LogWarning("[HttpRequestManager] - Need to do HttpRequestManager.Init() first.");
                return;
            }

            if (!HasSession())
            {
                TEDDebug.LogWarning("[HttpRequestManager] - The session didn't exist, need to do HttpRequstManager.Auth() and set the session first.");
                return;
            }

            string url = GetUrl(serverType, endPoint);

            WebRequest webRequest = WebRequest.Delete(url, GetHeaders(true), callback, userData);

            if (null == webRequest)
            {
                if (null != callback)
                {
                    callback.Invoke(0, null, userData);
                }

                return;
            }

            m_waitingRequests.Enqueue(webRequest);
        }

        public void GetTexture(string url, WebRequest.OnTextureCallback callback = null, object userData = null)
        {
            WebRequest webRequest = WebRequest.GetTexture(url, callback, userData);

            if (null == webRequest)
            {
                if (null != callback)
                {
                    callback.Invoke(0, null, userData);
                }

                return;
            }

            m_waitingRequests.Enqueue(webRequest);
        }

        private string GetUrl(BackendServerType serverType, string endPoint)
        {
            return string.Format("{0}/v{1}/{2}", serverType == BackendServerType.General ? m_generalServerUrl : m_gameServerUrl, m_apiVersion, endPoint);
        }

        private Dictionary<string, string> GetHeaders(bool isJson, bool isAuth = false)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(HEADER_KEY, m_appId);

            if (!isAuth)
            {
                headers.Add(SESSION_KEY, m_session);
            }

            #if UNITY_IOS
            headers.Add(CLIENT_VERSION_KEY, string.Format("unity-ios-{0}", m_gameVersion));
            #else
            headers.Add(CLIENT_VERSION_KEY, string.Format("unity-android-{0}", m_gameVersion));
            #endif

            if (isJson)
            {
                headers.Add(CONTENT_TYPE_KEY, CONTENT_TYPE);
            }

            return headers;
        }

        private void Update()
        {
            if (null == m_progressingRequest)
            {
                if (m_waitingRequests.Count != 0)
                {
                    m_progressingRequest = m_waitingRequests.Dequeue();
                    m_progressingRequest.Send();
                }
                else
                {
                    return;
                }
            }

            if (m_progressingRequest.IsDone())
            {
                if (m_progressingRequest.IsError())
                {
                    TEDDebug.LogErrorFormat("[HttpRequestManager] - {0}", m_progressingRequest.GetError());
                }

                m_progressingRequest.Finish();
                m_progressingRequest = null;
            }
            else
            {
                if (m_progressingRequest.IsOverTime())
                {
                    TEDDebug.LogErrorFormat("[HttpRequestManager] - Request Id {0} overtimed.", m_progressingRequest.RequestId);

                    m_progressingRequest.Dispose();
                    m_progressingRequest = null;
                }
            }
        }
    }
}