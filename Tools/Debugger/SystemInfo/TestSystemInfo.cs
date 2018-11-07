using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSystemInfo : MonoBehaviour
{
    private void Awake()
    {
        Debug.LogFormat("SystemInfo.batteryLevel = {0}", SystemInfo.batteryLevel);
        Debug.LogFormat("SystemInfo.batteryStatus = {0}", SystemInfo.batteryStatus);
        Debug.LogFormat("SystemInfo.copyTextureSupport = {0}", SystemInfo.copyTextureSupport);
        Debug.LogFormat("SystemInfo.deviceModel = {0}", SystemInfo.deviceModel);
        Debug.LogFormat("SystemInfo.deviceName = {0}", SystemInfo.deviceName);
        Debug.LogFormat("SystemInfo.deviceType = {0}", SystemInfo.deviceType);
        Debug.LogFormat("SystemInfo.deviceUniqueIdentifier = {0}", SystemInfo.deviceUniqueIdentifier);
        Debug.LogFormat("SystemInfo.graphicsDeviceID = {0}", SystemInfo.graphicsDeviceID);
        Debug.LogFormat("SystemInfo.graphicsDeviceName = {0}", SystemInfo.graphicsDeviceName);
        Debug.LogFormat("SystemInfo.graphicsDeviceType = {0}", SystemInfo.graphicsDeviceType);
        Debug.LogFormat("SystemInfo.graphicsDeviceVendor = {0}", SystemInfo.graphicsDeviceVendor);
        Debug.LogFormat("SystemInfo.graphicsDeviceVendorID = {0}", SystemInfo.graphicsDeviceVendorID);
        Debug.LogFormat("SystemInfo.graphicsDeviceVersion = {0}", SystemInfo.graphicsDeviceVersion);
        Debug.LogFormat("SystemInfo.graphicsMemorySize = {0}", SystemInfo.graphicsMemorySize);
        Debug.LogFormat("SystemInfo.graphicsMultiThreaded = {0}", SystemInfo.graphicsMultiThreaded);
        Debug.LogFormat("SystemInfo.graphicsShaderLevel = {0}", SystemInfo.graphicsShaderLevel);
        Debug.LogFormat("SystemInfo.graphicsUVStartsAtTop = {0}", SystemInfo.graphicsUVStartsAtTop);
        Debug.LogFormat("SystemInfo.maxCubemapSize = {0}", SystemInfo.maxCubemapSize);
        Debug.LogFormat("SystemInfo.maxTextureSize = {0}", SystemInfo.maxTextureSize);
        Debug.LogFormat("SystemInfo.npotSupport = {0}", SystemInfo.npotSupport);
        Debug.LogFormat("SystemInfo.operatingSystem = {0}", SystemInfo.operatingSystem);
        Debug.LogFormat("SystemInfo.operatingSystemFamily = {0}", SystemInfo.operatingSystemFamily);
        Debug.LogFormat("SystemInfo.processorCount = {0}", SystemInfo.processorCount);
        Debug.LogFormat("SystemInfo.processorFrequency = {0}", SystemInfo.processorFrequency);
        Debug.LogFormat("SystemInfo.processorType = {0}", SystemInfo.processorType);
        Debug.LogFormat("SystemInfo.supportedRenderTargetCount = {0}", SystemInfo.supportedRenderTargetCount);
        Debug.LogFormat("SystemInfo.supports2DArrayTextures = {0}", SystemInfo.supports2DArrayTextures);
        Debug.LogFormat("SystemInfo.supports32bitsIndexBuffer = {0}", SystemInfo.supports32bitsIndexBuffer);
        Debug.LogFormat("SystemInfo.supports3DRenderTextures = {0}", SystemInfo.supports3DRenderTextures);
        Debug.LogFormat("SystemInfo.supports3DTextures = {0}", SystemInfo.supports3DTextures);
        Debug.LogFormat("SystemInfo.supportsAccelerometer = {0}", SystemInfo.supportsAccelerometer);
        Debug.LogFormat("SystemInfo.supportsAsyncCompute = {0}", SystemInfo.supportsAsyncCompute);
        Debug.LogFormat("SystemInfo.supportsAsyncGPUReadback = {0}", SystemInfo.supportsAsyncGPUReadback);
        Debug.LogFormat("SystemInfo.supportsAudio = {0}", SystemInfo.supportsAudio);
        Debug.LogFormat("SystemInfo.supportsComputeShaders = {0}", SystemInfo.supportsComputeShaders);
        Debug.LogFormat("SystemInfo.supportsCubemapArrayTextures = {0}", SystemInfo.supportsCubemapArrayTextures);
        Debug.LogFormat("SystemInfo.supportsGPUFence = {0}", SystemInfo.supportsGPUFence);
        Debug.LogFormat("SystemInfo.supportsGyroscope = {0}", SystemInfo.supportsGyroscope);
        Debug.LogFormat("SystemInfo.supportsHardwareQuadTopology = {0}", SystemInfo.supportsHardwareQuadTopology);
        Debug.LogFormat("SystemInfo.supportsImageEffects = {0}", SystemInfo.supportsImageEffects);
        Debug.LogFormat("SystemInfo.supportsInstancing = {0}", SystemInfo.supportsInstancing);
        Debug.LogFormat("SystemInfo.supportsLocationService = {0}", SystemInfo.supportsLocationService);
        Debug.LogFormat("SystemInfo.supportsMipStreaming = {0}", SystemInfo.supportsMipStreaming);
        Debug.LogFormat("SystemInfo.supportsMotionVectors = {0}", SystemInfo.supportsMotionVectors);
        Debug.LogFormat("SystemInfo.supportsMultisampleAutoResolve = {0}", SystemInfo.supportsMultisampleAutoResolve);
        Debug.LogFormat("SystemInfo.supportsMultisampledTextures = {0}", SystemInfo.supportsMultisampledTextures);
        Debug.LogFormat("SystemInfo.supportsRawShadowDepthSampling = {0}", SystemInfo.supportsRawShadowDepthSampling);
        Debug.LogFormat("SystemInfo.supportsRenderToCubemap = {0}", SystemInfo.supportsRenderToCubemap);
        Debug.LogFormat("SystemInfo.supportsShadows = {0}", SystemInfo.supportsShadows);
        Debug.LogFormat("SystemInfo.supportsSparseTextures = {0}", SystemInfo.supportsSparseTextures);
        Debug.LogFormat("SystemInfo.supportsTextureWrapMirrorOnce = {0}", SystemInfo.supportsTextureWrapMirrorOnce);
        Debug.LogFormat("SystemInfo.supportsVibration = {0}", SystemInfo.supportsVibration);
        Debug.LogFormat("SystemInfo.systemMemorySize = {0}", SystemInfo.systemMemorySize);
        Debug.LogFormat("SystemInfo.unsupportedIdentifier = {0}", SystemInfo.unsupportedIdentifier);
        Debug.LogFormat("SystemInfo.usesReversedZBuffer = {0}", SystemInfo.usesReversedZBuffer);
    }
}
