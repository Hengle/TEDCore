using UnityEngine;

namespace TEDCore.Debugger.CheatMenu
{
    public abstract class CheatElement : MonoBehaviour
    {
        protected CheatMenuOptions m_cheatMenuOptions;
        protected CheatMenuData m_unitTestingData;

        public abstract void SetData(CheatMenuOptions cheatMenuOptions, CheatMenuData unitTestingData);
    }
}
