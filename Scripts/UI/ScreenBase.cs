using UnityEngine;
using TEDCore;
using TEDCore.Resource;
using TEDCore.Utils;
using System;

namespace TEDCore.UI
{
	public class ScreenBase : IDestroyable
	{
		public delegate void DestroyCallback ();
		private DestroyCallback m_destroyCallback;

		public GameObject Root { get { return m_root; } }
		public bool Visible
		{
			get
			{
				if(m_root != null)
				{
					return m_root.activeSelf;
				}

				return false;
			}

			set
			{
				if(m_root != null)
				{
					m_root.SetActive(value);
				}
			}
		}

		private GameObject m_root;


		public enum ScreenType
		{
			uGUI,
			NGUI
		}


		public bool LoadScreen(string name)
		{
			Destroy();
			
			return LoadScreenObject(name, "Canvas");
		}


		private bool LoadScreenObject(string name, string parentName)
		{
			m_root = Services.Get<ResourceManager>().CheckOutAndInstantiate(name, true);
			Services.Get<ResourceManager>().CheckIn(name);

			GameObject uiRoot = GameObject.Find(parentName);
			GameObjectUtils.AddChild(uiRoot, m_root);

			return m_root != null;
		}


		public void SetEventListenerOnClick(GameObject root, string name, EventListener.VoidDelegate callback)
		{
			if(m_root == null)
			{
				Debugger.LogWarning("[ScreenBase] - Root is NULL.");
				return;
			}
			
			try
			{
				EventListener.Get(root.FindChild(name)).onClick = callback;
			}
			catch(Exception e)
			{
				Debugger.LogError(string.Format("[ScreenBase] - Root name:{0}. Child name:{1}. ExceptionMsg:{2}.", root.name, name, e.Message));
			}
		}


		public void SetDestroyCallback(DestroyCallback callback)
		{
			m_destroyCallback = callback;
		}


		#region IDestroyable implementation
		public void Destroy ()
		{
			if (m_destroyCallback != null)
				m_destroyCallback ();

			Services.Get<ResourceManager>().CheckInAndDestroy(m_root);
			m_root = null;
		}
		#endregion
	}
}