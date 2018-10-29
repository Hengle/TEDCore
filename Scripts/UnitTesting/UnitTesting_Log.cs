using TEDCore;
using TEDCore.UnitTesting;
using TEDCore.Log;

public class UnitTesting_Log : BaseUnitTesting
{
    [TestButton]
    public void StartRecording()
    {
        LogManager.Instance.StartRecording();
    }

    [TestButton]
    public void Log()
    {
        TEDDebug.Log("Log");
    }

    [TestButton]
    public void LogWarning()
    {
        TEDDebug.LogWarning("LogWarning");
    }

    [TestButton]
    public void LogError()
    {
        TEDDebug.LogError("LogError");
    }

    [TestButton]
    public void LogException()
    {
        TEDDebug.LogException(new System.Exception("LogException"));
    }
}
