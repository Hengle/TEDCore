using UnityEngine;
using TEDCore.ObjectPool;
using TEDCore.Resource;
using TEDCore.Timer;

namespace TEDCore.Audio
{
    public class SFXManager : Singleton<SFXManager>
    {
        private const string OBJECT_POOL_KEY = "SFXManager";
        private float m_volume = 1f;

        private void Awake()
        {
            GameObject referenceAsset = new GameObject();
            referenceAsset.AddComponent<AudioSource>();

            ObjectPoolManager.Instance.RegisterPool(OBJECT_POOL_KEY, referenceAsset, 10);
        }

        public void SetVolume(float volume)
        {
            m_volume = volume;
        }

        public void Play(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("[SFXManager] - The asset name is null or empty.");
                return;
            }

            ResourceSystem.Instance.LoadAsync<AudioClip>(assetName, OnAssetLoaded);
        }

        public void Play(string bundleName, string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("[SFXManager] - The asset name is null or empty.");
                return;
            }

            ResourceSystem.Instance.LoadAsync<AudioClip>(bundleName, assetName, OnAssetLoaded);
        }

        private void OnAssetLoaded(AudioClip audioClip)
        {
            if(audioClip == null)
            {
                Debug.LogError("[SFXManager] - The AudioClip is null.");
                return;
            }

            AudioSource audioSource = ObjectPoolManager.Instance.Get(OBJECT_POOL_KEY).GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.volume = m_volume;
            audioSource.Play();

            BaseTimer baseTimer = new BaseTimer(audioClip.length, OnPlayFinished, audioSource);
            TimerManager.Instance.Add(baseTimer);
        }

        private void OnPlayFinished(object timerData)
        {
            AudioSource audioSource = (AudioSource)timerData;
            audioSource.clip = null;
            ObjectPoolManager.Instance.Recycle(OBJECT_POOL_KEY, audioSource.gameObject);

            ResourceSystem.Instance.Release();
        }
    }
}
