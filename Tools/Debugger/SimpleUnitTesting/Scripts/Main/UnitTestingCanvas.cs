using UnityEngine;
using UnityEngine.EventSystems;

namespace TEDCore.UnitTesting
{
    [ExecuteInEditMode]
    public class UnitTestingCanvas : MonoBehaviour
    {
        private void Awake()
        {
            if(FindObjectsOfType<EventSystem>().Length != 0)
            {
                return;
            }

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}
