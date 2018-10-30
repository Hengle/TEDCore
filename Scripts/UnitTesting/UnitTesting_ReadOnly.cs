using TEDCore.UnitTesting;
using UnityEngine;

public class UnitTesting_ReadOnly : BaseUnitTesting
{
    [ReadOnly] [SerializeField] private int m_int;
    [ReadOnly] [SerializeField] private GameObject m_gameObject;
}
