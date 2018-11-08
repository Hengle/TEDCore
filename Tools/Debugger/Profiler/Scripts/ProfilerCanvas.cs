using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.Debugger.Profiler
{
    public class ProfilerCanvas : MonoBehaviour
    {
        private const int HIGH_FPS = 60;
        private const int MIDIUM_FPS = 30;

        private const string MINI_FORMAT = "FPS: {0}\nMono: {1}MB\nMemory: {2}MB";

        private const string FULL_FPS_FORMAT = "Cur FPS: \t<color={0}>{1}</color>\nMin FPS: \t<color={2}>{3}</color>\nMax FPS: \t<color={4}>{5}</color>\nAvg FPS: \t<color={6}>{7}</color>";
        private const string FULL_MONO_FORMAT = "Used Size: \t{0:0.0}MB\nHeap Size: \t{1:0.0}MB";
        private const string FULL_MEMORY_FORMAT = "Allocated Size: \t\t{0:0.0}MB\nReserved Size: \t{1:0.0}MB";

        private const string COLOR_GREEN_FORMAT = "#00ff00";
        private const string COLOR_YELLOW_FORMAT = "#ffff00";
        private const string COLOR_RED_FORMAT = "#ff0000";

        private const int MEGA_BYTE = 1048576;

        [SerializeField] private float m_updateDuration = 1.0f;

        [Header("Mini Screen")]
        [SerializeField] private bool m_updateColor;
        [SerializeField] private Image m_colorBackground;
        [SerializeField] private Text m_miniText;

        [Header("Full Screen")]
        [SerializeField] private Text m_fullScreenFpsText;
        [SerializeField] private Text m_fullScreenMonoText;
        [SerializeField] private Button m_gcCollectButton;
        [SerializeField] private Text m_fullScreenMemoryText;
        [SerializeField] private Button m_cleanButton;

        private float m_monoUsedSize;
        private float m_monoHeapSize;
        private float m_memoryAllocatedSize;
        private float m_memoryReservedSize;

        private ProfilerManager m_performanceManager;
        private Color m_color;

        private void Awake()
        {
            m_performanceManager = ProfilerManager.Instance;
            m_performanceManager.SetUpdateDuration(m_updateDuration);
            m_performanceManager.SetUpdateCallback(OnUpdate);
            m_gcCollectButton.onClick.AddListener(() => System.GC.Collect());
            m_cleanButton.onClick.AddListener(() =>
            {
                System.GC.Collect();
                Resources.UnloadUnusedAssets();
            });
        }

        private void OnUpdate()
        {
            if (m_updateColor)
            {
                if (m_performanceManager.FpsData.CurValue >= HIGH_FPS)
                {
                    m_color = Color.green;
                }
                else if (m_performanceManager.FpsData.CurValue >= MIDIUM_FPS)
                {
                    m_color = Color.yellow;
                }
                else
                {
                    m_color = Color.red;
                }
            }
            else
            {
                m_color = Color.white;
            }

            m_monoUsedSize = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() / MEGA_BYTE;
            m_monoHeapSize = UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong() / MEGA_BYTE;
            m_memoryAllocatedSize = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / MEGA_BYTE;
            m_memoryReservedSize = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / MEGA_BYTE;

            #region Mini Screen
            m_colorBackground.color = m_color * 0.5f;
            m_miniText.text = string.Format(MINI_FORMAT, m_performanceManager.FpsData.CurValue, m_monoUsedSize, m_memoryAllocatedSize);
            #endregion

            #region Mull Screen
            m_fullScreenFpsText.text = GetFullScreenFpsText(m_performanceManager.FpsData);
            m_fullScreenMonoText.text = string.Format(FULL_MONO_FORMAT, m_monoUsedSize, m_monoHeapSize);
            m_fullScreenMemoryText.text = string.Format(FULL_MEMORY_FORMAT, m_memoryAllocatedSize, m_memoryReservedSize);
            #endregion
        }

        private string GetFullScreenFpsText(ProfilerData profilerData)
        {
            return string.Format(FULL_FPS_FORMAT
                                 , GetFpsColorFormat(profilerData.CurValue), profilerData.CurValue
                                 , GetFpsColorFormat(profilerData.MinValue), profilerData.MinValue
                                 , GetFpsColorFormat(profilerData.MaxValue), profilerData.MaxValue
                                 , GetFpsColorFormat(profilerData.AvgValue), profilerData.AvgValue);
        }

        private string GetFpsColorFormat(float fps)
        {
            string color = string.Empty;
            if(fps >= HIGH_FPS)
            {
                color = COLOR_GREEN_FORMAT;
            }
            else if(fps >= MIDIUM_FPS)
            {
                color = COLOR_YELLOW_FORMAT;
            }
            else
            {
                color = COLOR_RED_FORMAT;
            }

            return color;
        }
    }
}
