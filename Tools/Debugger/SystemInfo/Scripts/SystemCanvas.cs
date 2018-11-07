using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.Debugger
{
    public class SystemCanvas : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private Text m_operatingSystemText;
        [SerializeField] private Text m_deviceNameText;
        [SerializeField] private Text m_deviceTypeText;
        [SerializeField] private Text m_deviceModelText;
        [SerializeField] private Text m_deviceUniqueIdentifierText;
        [SerializeField] private Text m_cpuTypeText;
        [SerializeField] private Text m_cpuFrequencyText;
        [SerializeField] private Text m_cpuCountText;
        [SerializeField] private Text m_systemMemoryText;

        [Header("Display")]
        [SerializeField] private Text m_resolutionText;
        [SerializeField] private Text m_dpiText;
        [SerializeField] private Text m_orientationText;

        [Header("Graphics")]
        [SerializeField] private Text m_graphicsDeviceNameText;
        [SerializeField] private Text m_graphicsDeviceVendorText;
        [SerializeField] private Text m_graphicsDeviceVersionText;
        [SerializeField] private Text m_graphicsMemorySizeText;
        [SerializeField] private Text m_maxTextureSizeText;
        [SerializeField] private Text m_maxCubemapSizeText;
        [SerializeField] private Text m_npotSupportText;
        [SerializeField] private Text m_supportedRenderTargetCountText;
        [SerializeField] private Text m_supportsComputeShadersText;
        [SerializeField] private Text m_supportsImageEffectsText;
        [SerializeField] private Text m_supportsRenderToCubemapText;
        [SerializeField] private Text m_supportsShadowsText;

        private void Awake()
        {
            UpdateSystemInformations();
            UpdateDisplayInformations();
            UpdateGraphicsInformations();
        }

        private void UpdateSystemInformations()
        {
            m_operatingSystemText.text = SystemInfo.operatingSystem;
            m_deviceNameText.text = SystemInfo.deviceName;
            m_deviceTypeText.text = SystemInfo.deviceType.ToString();
            m_deviceModelText.text = SystemInfo.deviceModel;
            m_deviceUniqueIdentifierText.text = SystemInfo.deviceUniqueIdentifier;
            m_cpuTypeText.text = SystemInfo.processorType;
            m_cpuFrequencyText.text = ((float)SystemInfo.processorFrequency / 1000).ToString("f2") + "GHz";
            m_cpuCountText.text = SystemInfo.processorCount.ToString();
            m_systemMemoryText.text = (SystemInfo.systemMemorySize / 1024) + "G";
        }

        private void UpdateDisplayInformations()
        {
            m_resolutionText.text = string.Format("{0} x {1}", Screen.width, Screen.height);
            m_dpiText.text = Screen.dpi.ToString();
            m_orientationText.text = Screen.orientation.ToString();
        }

        private void UpdateGraphicsInformations()
        {
            m_graphicsDeviceNameText.text = SystemInfo.graphicsDeviceName;
            m_graphicsDeviceVendorText.text = SystemInfo.graphicsDeviceVendor;
            m_graphicsDeviceVersionText.text = SystemInfo.graphicsDeviceVersion;
            m_graphicsMemorySizeText.text = (SystemInfo.graphicsMemorySize / 1024) + "G";
            m_maxTextureSizeText.text = SystemInfo.maxTextureSize.ToString();
            m_maxCubemapSizeText.text = SystemInfo.maxCubemapSize.ToString();
            m_npotSupportText.text = SystemInfo.npotSupport.ToString();
            m_supportedRenderTargetCountText.text = SystemInfo.supportedRenderTargetCount.ToString();
            m_supportsComputeShadersText.text = SystemInfo.supportsComputeShaders ? "Yes" : "No";
            m_supportsImageEffectsText.text = SystemInfo.supportsImageEffects ? "Yes" : "No";
            m_supportsRenderToCubemapText.text = SystemInfo.supportsRenderToCubemap ? "Yes" : "No";
            m_supportsShadowsText.text = SystemInfo.supportsShadows ? "Yes" : "No";
        }
    }
}
