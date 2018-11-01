using UnityEngine;
using System.Collections.Generic;
using MEC;

namespace TEDCore.Utils
{
    [AddComponentMenu("Utilities/HUDFPS")]
    public class HUDFPS : MonoBehaviour
    {
        [SerializeField] private bool m_updateColor = true;
        [SerializeField] private bool m_allowDrag = true;
        [SerializeField] private float m_updateFreq = 0.5f;

        private Rect m_startRect;
        private float m_accum = 0f;
        private int m_frames = 0;
        private Color m_color = Color.white;
        private string m_fps = "";
        private GUIStyle m_style;

        private void Awake()
        {
            m_startRect = new Rect(10, 10, Screen.width * 0.15f, Screen.height * 0.1f);
        }

        private void Start()
        {
            Timing.RunCoroutine(FPS());
        }

        private void Update()
        {
            m_accum += Time.timeScale / Time.deltaTime;
            ++m_frames;
        }

        private IEnumerator<float> FPS()
        {
            while (true)
            {
                float fps = m_accum / m_frames;
                m_fps = fps.ToString("f1");

                m_color = (fps >= 30) ? Color.green : ((fps > 10) ? Color.red : Color.yellow);

                m_accum = 0.0F;
                m_frames = 0;

                yield return Timing.WaitForSeconds(m_updateFreq);
            }
        }

        private void OnGUI()
        {
            if (null == m_style)
            {
                m_style = new GUIStyle(GUI.skin.label);
                m_style.normal.textColor = Color.white;
                m_style.alignment = TextAnchor.MiddleCenter;
                m_style.fontSize = (int)(m_startRect.height / 3);
            }

            GUI.color = m_updateColor ? m_color : Color.white;
            m_startRect = GUI.Window(0, m_startRect, DoMyWindow, "");
        }

        private void DoMyWindow(int windowID)
        {
            GUI.Label(new Rect(0, 0, m_startRect.width, m_startRect.height), string.Format("{0} FPS", m_fps), m_style);

            if (m_allowDrag)
            {
                GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
            }
        }
    }
}