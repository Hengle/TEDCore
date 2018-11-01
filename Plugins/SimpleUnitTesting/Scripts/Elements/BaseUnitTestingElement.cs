using UnityEngine;

namespace TEDCore.UnitTesting
{
    public abstract class BaseUnitTestingElement : MonoBehaviour
    {
        protected BaseUnitTesting m_baseUnitTesting;
        protected UnitTestingData m_unitTestingData;

        public abstract void SetData(BaseUnitTesting baseUnitTesting, UnitTestingData unitTestingData);
    }
}
