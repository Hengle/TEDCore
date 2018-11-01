using UnityEngine;
using System.Collections.Generic;

namespace TEDCore.Utils
{
	public static class GameObjectUtils
	{
        public static void SetLayer(this GameObject root, int layer)
        {
            Transform[] allChildren = root.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                child.gameObject.layer = layer;
            }
        }

		public static T GetChildComponent<T>(this GameObject root, string name) where T : Component
		{
			GameObject go = FindChild(root, name);

			if(go != null)
			{
				return go.GetComponent<T>();
			}

			return null;
		}

		public static GameObject FindChild(this GameObject root, string name)
		{
			GameObject go = null;
			int cnt = 0;

			// Find the children in base level first.
			for(cnt = 0; cnt < root.transform.childCount; cnt++)
			{
				go = root.transform.GetChild(cnt).gameObject;

				if(go.name == name)
				{
					return go;
				}
			}

			// Find children's children
			for(cnt = 0; cnt < root.transform.childCount; cnt++)
			{
				go = root.transform.GetChild(cnt).gameObject;

				if(go.transform.childCount > 0)
				{
					go = FindChild(go, name);

					if(go != null)
					{
						return go;
					}
				}
			}

			return null;
		}

		public static List<GameObject> FindChildContainName(this GameObject root, string name)
		{
			GameObject go = null;
			int cnt = 0;

			List<GameObject> childs = new List<GameObject> ();
			
			// Find the children in base level first.
			for(cnt = 0; cnt < root.transform.childCount; cnt++)
			{
				go = root.transform.GetChild(cnt).gameObject;
				
				if(go.name.Contains(name))
				{
					childs.Add(go);
				}
			}
			
			return childs;
		}

		public static void FitScreen(this GameObject screen)
		{
			float ratio = 1280f / Screen.width;
			
			screen.GetComponent<RectTransform> ().sizeDelta = new Vector2(1280, Screen.height * ratio);
		}

		public static Vector3 InputToWorldPosition(Vector2 inputPosition)
		{
			Vector3 pos = new Vector3 (inputPosition.x, inputPosition.y, 0);
			
			pos.x -= Screen.width * 0.5f;
			pos.y -= Screen.height * 0.5f;
			pos.x = Mathf.Round(pos.x);
			pos.y = Mathf.Round(pos.y);

			return pos * 1280.0f / Screen.width;
		}
	}
}