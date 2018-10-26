using UnityEngine;
using TEDCore.Resource;

namespace TEDCore.Audio
{
    public class BGMManager : Singleton<BGMManager>
    {
        private float m_volume = 1f;
        private AudioSource m_audioSource;
        private string m_previousBundleName;
        private string m_bundleName;

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


        public void PlayBGM(string bgmName)
        {
            if (null != m_audioSource && m_audioSource.name.Contains(bgmName))
            {
                return;
            }

            m_bundleName = string.Empty;
            ResourceSystem.Instance.LoadAsync<AudioClip>(bgmName, OnBGMLoaded);
        }


        public void PlayBGM(string bundleName, string bgmName)
        {
            if (null != m_audioSource && m_audioSource.name.Contains(bgmName))
            {
                return;
            }

            m_bundleName = bundleName;
            ResourceSystem.Instance.LoadAsync<AudioClip>(bundleName, bgmName, OnBGMLoaded);
        }


        private void OnBGMLoaded(AudioClip audioClip)
        {
            if(audioClip == null)
            {
                return;
            }

            StopBGM();
            CreateBGM();

            m_audioSource.name = string.Format("[BGM] - {0}", audioClip.name);
            m_audioSource.clip = audioClip;
            ResourceSystem.Instance.Unload<AudioClip>(m_bundleName, audioClip.name);
            m_audioSource.volume = m_volume;
            m_audioSource.loop = true;
            m_audioSource.Play();

            m_previousBundleName = m_bundleName;
        }


        public void StopBGM()
        {
            if (null == m_audioSource)
            {
                return;
            }

            m_audioSource.Stop();
            ResourceSystem.Instance.Unload<AudioClip>(m_previousBundleName, m_audioSource.clip.name);
        }


        private void CreateBGM()
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

