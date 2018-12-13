using UnityEngine;
using TEDCore.Event;
using InControl;

namespace TEDCore.Control
{
    public class InControlEventManager : MonoBehaviour
    {
        private KeyboardVirtualDevice m_virtualDevice;
        private InputDevice m_inputDevice;
        private EventManager m_eventManager;
        private EventListener m_eventListener;

        private void Awake()
        {
            CacheEventManager();
        }

        private void OnEnable()
        {
            m_virtualDevice = new KeyboardVirtualDevice();
            InputManager.OnSetup += () => InputManager.AttachDevice(m_virtualDevice);
        }

        private void OnDisable()
        {
            InputManager.DetachDevice(m_virtualDevice);
            m_virtualDevice = null;
        }

        private void Update()
        {
            m_inputDevice = InputManager.ActiveDevice;

            CacheEventManager();
            m_eventManager.SendEvent((int)InputControlType.LeftStickX, m_inputDevice.LeftStickX);
            m_eventManager.SendEvent((int)InputControlType.LeftStickY, m_inputDevice.LeftStickY);

            m_inputDevice = null;
        }

        private void CacheEventManager()
        {
            if (m_eventManager != null)
            {
                return;
            }

            m_eventManager = EventManager.Instance;
        }
    }
}