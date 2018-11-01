using TEDCore.UnitTesting;
using UnityEngine;

public class UnitTesting_EnumMask : BaseUnitTesting
{
    public enum TestEnum
    {
        Low,
        Medium,
        High
    }

    [EnumMask] [SerializeField] private TestEnum m_testEnum;
}
