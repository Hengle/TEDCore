using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;
using TEDCore;

namespace TEDCore.Http
{
    public class WebRequest
    {
        private const float MAX_TIME_OUT = 15f;
        private static int m_nextRequestId = 1;

        public delegate void OnDataCallback(int requestId, string data, object userData);
        public delegate void OnTextureCallback(int requestId, Texture data, object userData);

        public int RequestId { get { return m_requestId; } }
        private int m_requestId;
        private UnityWebRequest m_request;
        private OnDataCallback m_onDataCallback;
        private OnTextureCallback m_onTextureCallback;
        private object m_userData;
        private float m_startTime;

        private WebRequest(OnDataCallback callback, object userData)
        {
            m_requestId = m_nextRequestId;
            m_nextRequestId++;

            m_onDataCallback = callback;
            m_userData = userData;
        }


        private WebRequest(OnTextureCallback callback, object userData)
        {
            m_requestId = m_nextRequestId;
            m_nextRequestId++;

            m_onTextureCallback = callback;
            m_userData = userData;
        }


        public void Dispose()
        {
            if (null == m_request)
            {
                return;
            }

            m_request.Dispose();
        }


        public static WebRequest Post(string url, Dictionary<string, string> headers, object jsonObject, OnDataCallback callback, object userData)
        {
            byte[] postData = null;
            if (null != jsonObject)
            {
                postData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(jsonObject));
            }
            UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(postData);
            uploadHandlerRaw.contentType = "application/json";

            UnityWebRequest unityWebRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            unityWebRequest.uploadHandler = uploadHandlerRaw;
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            foreach (KeyValuePair<string, string> kvp in headers)
            {
                unityWebRequest.SetRequestHeader(kvp.Key, kvp.Value);
            }

            WebRequest webRequest = new WebRequest(callback, userData);
            webRequest.m_request = unityWebRequest;

            return webRequest;
    	}


        public static WebRequest Get(string url, Dictionary<string, string> headers, OnDataCallback callback, object userData)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);

            foreach (KeyValuePair<string, string> kvp in headers)
            {
                unityWebRequest.SetRequestHeader(kvp.Key, kvp.Value);
            }

            WebRequest webRequest = new WebRequest(callback, userData);
            webRequest.m_request = unityWebRequest;

            return webRequest;
        }


        public static WebRequest Delete(string url, Dictionary<string, string> headers, OnDataCallback callback, object userData)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Delete(url);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            foreach (KeyValuePair<string, string> kvp in headers)
            {
                unityWebRequest.SetRequestHeader(kvp.Key, kvp.Value);
            }

            WebRequest webRequest = new WebRequest(callback, userData);
            webRequest.m_request = unityWebRequest;

            return webRequest;
        }


        public static WebRequest GetTexture(string url, OnTextureCallback callback, object userData)
        {
            UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url);

            WebRequest webRequest = new WebRequest(callback, userData);
            webRequest.m_request = unityWebRequest;

            return webRequest;
        }


        public void Send()
        {
            if (null == m_request)
            {
                return;
            }

            m_request.SendWebRequest();
            m_startTime = Time.realtimeSinceStartup;
        }


        public bool IsDone()
        {
            if (null == m_request)
            {
                return true;
            }

            return m_request.isDone;
        }


        public bool IsOverTime()
        {
            if (Time.realtimeSinceStartup - m_startTime > MAX_TIME_OUT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool IsError()
        {
            if (null == m_request)
            {
                return true;
            }

            return m_request.isNetworkError;
        }


        public string GetError()
        {
            if (null == m_request)
            {
                return string.Empty;
            }

            return m_request.error;
        }


        public void Finish()
        {
            if (null != m_onTextureCallback)
            {
                OnTextureLoaded();
            }
            else
            {
                OnDataLoaded();
            }

            Dispose();
        }


        private void OnTextureLoaded()
        {
            TEDDebug.LogFormat("[WebRequest] OnTextureLoaded - requst id = {0}, url = {1}", m_requestId, m_request.url);
            m_onTextureCallback.Invoke(m_requestId, DownloadHandlerTexture.GetContent(m_request), m_userData);
        }


        private void OnDataLoaded()
        {
            TEDDebug.LogFormat("[WebRequest] OnDataLoaded - requst id = {0}, url = {1}, data = {2}", m_requestId, m_request.url, DownloadHandlerBuffer.GetContent(m_request));

            if (null == m_onDataCallback)
            {
                return;
            }

            m_onDataCallback.Invoke(m_requestId, DownloadHandlerBuffer.GetContent(m_request), m_userData);
        }
    }
}