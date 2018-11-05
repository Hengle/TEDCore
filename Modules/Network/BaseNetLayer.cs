using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

namespace TEDCore.Network
{
    public class BaseNetLayer : MonoBehaviour, IDestroy
    {
        protected NetManager m_netManager;
        protected NetDataWriter m_dataWriter;

        public virtual void Update()
        {
            if (null != m_netManager)
            {
                m_netManager.PollEvents();
            }
        }


        #region IDestroyable implementation
        public virtual void Destroy()
        {
            if (null != m_netManager)
            {
                m_netManager.Stop();
                m_netManager = null;
            }

            if (null != m_dataWriter)
            {
                m_dataWriter.Reset();
            }
        }
        #endregion
    }
}