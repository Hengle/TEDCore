using InControl;
using UnityEngine;

namespace TEDCore.Control
{
    public class KeyboardVirtualDevice : InputDevice
    {
        private const float SENSITIVITY = 0.1f;

        private float m_kx;
        private float m_ky;

        public KeyboardVirtualDevice() : base("Keyboard Controller")
        {
            AddControl(InputControlType.LeftStickLeft, "Left Stick Left");
            AddControl(InputControlType.LeftStickRight, "Left Stick Right");
            AddControl(InputControlType.LeftStickUp, "Left Stick Up");
            AddControl(InputControlType.LeftStickDown, "Left Stick Down");
        }

        public override void Update(ulong updateTick, float deltaTime)
        {
            var leftStickVector = GetVectorFromKeyboard(deltaTime, true);
            UpdateLeftStickWithValue(leftStickVector, updateTick, deltaTime);

            Commit(updateTick, deltaTime);
        }


        private Vector2 GetVectorFromKeyboard(float deltaTime, bool smoothed)
        {
            if (smoothed)
            {
                m_kx = ApplySmoothing(m_kx, GetXFromKeyboard(), deltaTime, SENSITIVITY);
                m_ky = ApplySmoothing(m_ky, GetYFromKeyboard(), deltaTime, SENSITIVITY);
            }
            else
            {
                m_kx = GetXFromKeyboard();
                m_ky = GetYFromKeyboard();
            }
            return new Vector2(m_kx, m_ky);
        }

        private float GetXFromKeyboard()
        {
            return UnityEngine.Input.GetAxis("Horizontal"); ;
        }

        private float GetYFromKeyboard()
        {
            return UnityEngine.Input.GetAxis("Vertical");
        }

        private float ApplySmoothing(float lastValue, float thisValue, float deltaTime, float sensitivity)
        {
            sensitivity = Mathf.Clamp(sensitivity, 0.001f, 1.0f);

            if (Mathf.Approximately(sensitivity, 1.0f))
            {
                return thisValue;
            }

            return Mathf.Lerp(lastValue, thisValue, deltaTime * sensitivity * 100.0f);
        }
    }
}

