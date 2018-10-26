using TEDCore.UnitTesting;
using TEDCore.Audio;

public class UnitTesting_SFX : BaseUnitTesting
{
    [TestInputField]
    public void OnPlayResourceSFX(string value)
    {
        SFXManager.Instance.Play(value);
    }

    [TestInputField]
    public void SetVolume(string value)
    {
        SFXManager.Instance.SetVolume(float.Parse(value));
    }
}
