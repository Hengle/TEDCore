using System;
using System.Collections.Generic;
using UnityEngine;
using TEDCore.Resource;

namespace TEDCore.Audio
{
	public class AudioManager
	{		
		private GameObject _BGM = null;

		public AudioManager()
		{

		}


		public void PlaySfxOnce(string sfxName)
		{
			PreloadSfx (sfxName);
			PlaySfx (sfxName);
			UnloadSfx (sfxName);
		}
		
		
		public void PreloadSfx(string sfxName)
		{
			MonoBehaviourManager.Get<ResourceManager>().CheckOut<AudioClip>(sfxName, true);
		}


		public void UnloadSfx(string sfxName)
		{
			MonoBehaviourManager.Get<ResourceManager>().CheckIn(sfxName);
		}
		
		
		public AudioSource PlaySfx(string sfxName)
		{
			AudioSource audioSource = PlaySfxAndReturn(sfxName);

			return audioSource;
		}
		
		
        private AudioSource PlaySfxAndReturn(string sfxName)
		{			
			GameObject audioGO = new GameObject();
			audioGO.name = sfxName;
			
			AudioSource audioSource = audioGO.AddComponent<AudioSource>();
			audioSource.clip = MonoBehaviourManager.Get<ResourceManager>().CheckOut<AudioClip>(sfxName);
			audioSource.Play();
			GameObject.Destroy(audioGO, audioSource.clip.length);
			
			return audioSource;
		}
		
		
		public void PlayBGM(string musicName, float volume = 1.0f)
		{
			if (_BGM != null && _BGM.name.Contains (musicName))
				return;

			StopBGM();
			
			_BGM = new GameObject();
			_BGM.transform.position = Vector3.zero;
			_BGM.name = string.Format("BGM_{0}", musicName);
			AudioSource source = _BGM.AddComponent<AudioSource>();
			
			source.clip = MonoBehaviourManager.Get<ResourceManager>().CheckOut<AudioClip>(musicName, true);
			MonoBehaviourManager.Get<ResourceManager> ().CheckIn (musicName);
			source.volume = volume;
			source.loop = true;
			source.Play();
		}
		
		
		public void StopBGM()
		{
			if(_BGM == null)
			{
				return;
			}
			
			_BGM.GetComponent<AudioSource>().Stop();
			MonoBehaviourManager.Get<ResourceManager>().CheckInAndDestroy(_BGM);
		}
	}
}

