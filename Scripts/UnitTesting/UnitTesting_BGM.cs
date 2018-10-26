using TEDCore.UnitTesting;
using TEDCore.Audio;

public class UnitTesting_BGM : BaseUnitTesting
{
    [TestInputField]
    public void OnPlayResourceBGM(string value)
    {
        BGMManager.Instance.Play(value);
    }

    [TestInputField]
    public void OnPlayAssetBundleBGM(string value)
    {
        BGMManager.Instance.Play("main", value);
    }

    [TestInputField]
    public void SetVolume(string value)
    {
        BGMManager.Instance.SetVolume(float.Parse(value));
    }

    [TestInputField]
    public void SetVolumeFading(string value)
    {
        BGMManager.Instance.SetVolume(float.Parse(value), 2.0f);
    }

    [TestButton]
    public void StopBGM()
    {
        BGMManager.Instance.Stop();
    }
}
