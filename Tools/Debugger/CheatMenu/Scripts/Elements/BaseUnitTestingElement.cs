using UnityEngine;

namespace TEDCore.UnitTesting
{
    public abstract class BaseUnitTestingElement : MonoBehaviour
    {
        protected CheatMenuOptions m_cheatMenuOptions;
        protected UnitTestingData m_unitTestingData;

        public abstract void SetData(CheatMenuOptions cheatMenuOptions, UnitTestingData unitTestingData);
    }
}
