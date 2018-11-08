using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.Debugger.Profiler
{
    public class ProfilerCanvas : MonoBehaviour
    {
        private const int HIGH_FPS = 60;
        private const int MIDIUM_FPS = 30;
        private const string FPS_FORMAT = "Cur FPS: {0}\nMin FPS: {1}\nMax FPS: {2}\nAvg FPS: {3}";

        private const int MEGA_BYTE = 1048576;

        [SerializeField] private bool m_updateColor;
        [SerializeField] private float m_updateDuration = 1.0f;
        [SerializeField] private Image m_colorBackground;
        [SerializeField] private Text m_fpsText;
        [SerializeField] private Button m_resetButton;

        private const string MONO_TOTAL_TEXT = "Mono Heap Size: {0:0.0}MB";
        private const string MONO_USED_TEXT = "Mono Used Size: {0:0.0}MB";
        private const string MEMORY_TOTAL_TEXT = "Total Reserved Memory Size: {0:0.0}MB";
        private const string MEMORY_USED_TEXT = "Total Allocated Memory Size: {0:0.0}MB";
        [Header("Full Screen")]
        [SerializeField] private Text m_fullScreenFpsText;
        [SerializeField] private Text m_monoHeapSizeText;
        [SerializeField] private Text m_monoUsedSizeText;
        [SerializeField] private Slider m_monoSlider;
        [SerializeField] private Button m_gcCollectButton;
        [SerializeField] private Text m_totalReservedMemoryText;
        [SerializeField] private Text m_totalAllocatedMemoryText;
        [SerializeField] private Slider m_memorySlider;
        [SerializeField] private Button m_cleanButton;

        private float m_cacheValue1;
        private float m_cacheValue2;

        private ProfilerManager m_performanceManager;
        private Color m_color;

        private void Awake()
        {
            m_performanceManager = ProfilerManager.Instance;
            m_performanceManager.SetUpdateDuration(m_updateDuration);
            m_performanceManager.SetUpdateCallback(OnUpdate);
            m_resetButton.onClick.AddListener(() => m_performanceManager.Reset());
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

            m_colorBackground.color = m_color * 0.5f;
            m_fpsText.text = string.Format(FPS_FORMAT, m_performanceManager.FpsData.CurValue, m_performanceManager.FpsData.MinValue, m_performanceManager.FpsData.MaxValue, m_performanceManager.FpsData.AvgValue);

            m_fullScreenFpsText.text = m_fpsText.text;

            m_cacheValue1 = UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong();
            m_cacheValue2 = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
            m_monoHeapSizeText.text = string.Format(MONO_TOTAL_TEXT, m_cacheValue1 / MEGA_BYTE);
            m_monoUsedSizeText.text = string.Format(MONO_USED_TEXT, m_cacheValue2 / MEGA_BYTE);
            m_monoSlider.value = m_cacheValue2 / m_cacheValue1;

            m_cacheValue1 = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();
            m_cacheValue2 = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
            m_totalReservedMemoryText.text = string.Format(MEMORY_TOTAL_TEXT, m_cacheValue1 / MEGA_BYTE);
            m_totalAllocatedMemoryText.text = string.Format(MEMORY_USED_TEXT, m_cacheValue2 / MEGA_BYTE);
            m_memorySlider.value = m_cacheValue2 / m_cacheValue1;
        }
    }
}
