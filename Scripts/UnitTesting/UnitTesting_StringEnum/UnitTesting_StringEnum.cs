using TEDCore.UnitTesting;
using UnityEngine;

public class UnitTesting_StringEnum : BaseUnitTesting
{
    [SerializeField] private TestStringEnum m_test;

    [TestButton]
    public void ShowEnum()
    {
        TEDCore.TEDDebug.Log(m_test.GetEnum().ToString());
    }
}
