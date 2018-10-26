using UnityEngine;
using TEDCore.Resource;

namespace TEDCore.Audio
{
    public class BGMManager : MonoSingleton<BGMManager>
    {
        private float m_volume = 1f;
        private AudioSource m_audioSource;

        public void SetVolume(float volume)
        {
            m_volume = volume;
            m_audioSource.volume = m_volume;
        }


        private float m_duration;
        private bool m_fade;
        private float m_currentVolume;
        private float m_targetVolume;
        private float m_lerp;
        public void SetVolume(float volume, float duration)
        {
            m_fade = true;
            m_currentVolume = m_volume;
            m_targetVolume = volume;
            m_duration = duration;
            m_lerp = 0;
        }


        private void Update()
        {
            if(!m_fade)
            {
                return;
            }

            m_lerp += Time.deltaTime;
            if (m_lerp > m_duration)
            {
                m_lerp = m_duration;
                m_fade = false;
            }

            SetVolume(Mathf.Lerp(m_currentVolume, m_targetVolume, m_lerp / m_duration));
        }


        public void Play(string assetName)
        {
            if(string.IsNullOrEmpty(assetName))
            {
                TEDDebug.LogError("[BGMManager] - The asset name is null or empty.");
                return;
            }

            if (null != m_audioSource && m_audioSource.name.Contains(assetName))
            {
                return;
            }

            ResourceSystem.Instance.LoadAsync<AudioClip>(assetName, OnAssetLoaded);
        }


        public void Play(string bundleName, string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                TEDDebug.LogError("[BGMManager] - The asset name is null or empty.");
                return;
            }

            if (null != m_audioSource && m_audioSource.name.Contains(assetName))
            {
                return;
            }

            ResourceSystem.Instance.LoadAsync<AudioClip>(bundleName, assetName, OnAssetLoaded);
        }


        private void OnAssetLoaded(AudioClip audioClip)
        {
            if(audioClip == null)
            {
                TEDDebug.LogError("[BGMManager] - The AudioClip is null.");
                return;
            }

            Stop();
            Create();

            m_audioSource.name = string.Format("[BGM] - {0}", audioClip.name);
            m_audioSource.clip = audioClip;
            m_audioSource.volume = m_volume;
            m_audioSource.loop = true;
            m_audioSource.Play();
        }


        public void Stop()
        {
            if (null == m_audioSource)
            {
                return;
            }

            m_audioSource.Stop();
            m_audioSource.clip = null;

            ResourceSystem.Instance.Release();
        }


        private void Create()
        {
            if(m_audioSource != null)
            {
                return;
            }

            GameObject bgmObject = new GameObject();
            bgmObject.transform.SetParent(transform);
            bgmObject.transform.position = Vector3.zero;

            m_audioSource = bgmObject.AddComponent<AudioSource>();
        }
    }
}

