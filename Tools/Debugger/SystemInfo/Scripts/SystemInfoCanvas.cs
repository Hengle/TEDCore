using UnityEngine;
using UnityEngine.UI;

namespace TEDCore.Debugger
{
    public class SystemInfoCanvas : MonoBehaviour
    {
        [SerializeField] private Text m_systemText;
        [SerializeField] private Text m_displayText;
        [SerializeField] private Text m_graphicsText;

        private void Awake()
        {
            UpdateSystemInformations();
            UpdateDisplayInformations();
            UpdateGraphicsInformations();
        }

        private void UpdateSystemInformations()
        {
            m_systemText.text = string.Format("Operating System: {0}\n" +
                                              "Device Name: {1}\n" +
                                              "Device Type: {2}\n" +
                                              "Device Model: {3}\n" +
                                              "Device Unique Identifier: {4}\n" +
                                              "CPU Type: {5}\n" +
                                              "CPU Frequency: {6}GHz\n" +
                                              "CPU Count: {7}\n" +
                                              "System Memory Size: {8}G"
                                              ,SystemInfo.operatingSystem
                                              ,SystemInfo.deviceName
                                              ,SystemInfo.deviceType
                                              ,SystemInfo.deviceModel
                                              ,SystemInfo.deviceUniqueIdentifier
                                              ,SystemInfo.processorType
                                              ,((float)SystemInfo.processorFrequency / 1000).ToString("f2")
                                              ,SystemInfo.processorCount
                                              ,SystemInfo.systemMemorySize / 1024);
        }

        private void UpdateDisplayInformations()
        {
            m_displayText.text = string.Format("Resolution: {0} x {1}\n" +
                                               "DPI: {2}\n" +
                                               "Orientation: {3}"
                                               ,Screen.width, Screen.height
                                               ,Screen.dpi
                                               ,Screen.orientation);
        }

        private void UpdateGraphicsInformations()
        {
            m_graphicsText.text = string.Format("Graphics Device Name: {0}\n" +
                                                "Graphics Device Vendor: {1}\n" +
                                                "Graphics Device Version: {2}\n" +
                                                "Graphics Memory Size: {3}G\n" +
                                                "Max Texture Size: {4}\n" +
                                                "Max Cubemap Size: {5}\n" +
                                                "N Pot Support: {6}\n" +
                                                "Supported RenderTarget Count: {7}\n" +
                                                "Supports Compute Shaders: {8}\n" +
                                                "Supports Image Effects: {9}\n" +
                                                "Supports Render To Cubemap: {10}\n" +
                                                "Supports Shadows: {11}"
                                                , SystemInfo.graphicsDeviceName
                                                , SystemInfo.graphicsDeviceVendor
                                                , SystemInfo.graphicsDeviceVersion
                                                , SystemInfo.graphicsMemorySize
                                                , SystemInfo.maxTextureSize
                                                , SystemInfo.maxCubemapSize
                                                , SystemInfo.npotSupport
                                                , SystemInfo.supportedRenderTargetCount
                                                , SystemInfo.supportsComputeShaders
                                                , SystemInfo.supportsImageEffects
                                                , SystemInfo.supportsRenderToCubemap
                                                , SystemInfo.supportsShadows);
        }
    }
}
