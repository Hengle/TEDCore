using TEDCore.UnitTesting;
using UnityEngine;

public class UnitTesting_EnumFlag : BaseUnitTesting
{
    public enum TestEnum
    {
        Low = 1,
        Medium = 2,
        High = 4
    }

    [EnumFlag] [SerializeField] private TestEnum m_testEnum;

    [TestButton]
    public void ShowEnum()
    {
        TEDCore.TEDDebug.Log(m_testEnum.ToString());
        TEDCore.TEDDebug.Log((int)m_testEnum);
    }
}
