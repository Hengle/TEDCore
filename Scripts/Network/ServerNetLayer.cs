using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TEDCore.Network
{
    public class ServerNetLayer : BaseNetLayer, INetEventListener
    {
        protected int m_maxConnections;
        private List<NetPeer> m_connectedClients = new List<NetPeer>();

        private float m_syncTimer = 0f;
        private float m_syncTime;

        private Action m_syncToClientMethods;

        public void Init(int syncTimePerSecond, int maxConnections, string connectKey, int port, int updateTime)
        {
            Destroy();

            m_syncTime = 1.0f / syncTimePerSecond;

            m_maxConnections = maxConnections;

            m_netManager = new NetManager(this, maxConnections, connectKey);
            m_netManager.Start(port);
            m_netManager.NatPunchEnabled = true;
            m_netManager.DiscoveryEnabled = true;
            m_netManager.UpdateTime = updateTime;

            m_dataWriter = new NetDataWriter();

            InitServer();
        }


        public virtual void InitServer()
        {
            
        }


        public void AddSyncToClientMethod(Action method)
        {
            m_syncToClientMethods += method;
        }


        public void RemoveSyncToClientMethod(Action method)
        {
            m_syncToClientMethods -= method;
        }


        public void SendToClients(NetDataWriter netDataWriter, SendOptions sendOption)
        {
            for(int i = 0; i < m_connectedClients.Count; i++)
            {
                m_connectedClients[i].Send(netDataWriter, sendOption);
            }
        }


        public override void Update()
        {
            base.Update();

            if (null == m_syncToClientMethods)
            {
                return;
            }

            m_syncTimer += Time.deltaTime;
            if (m_syncTimer >= m_syncTime)
            {
                m_syncTimer = 0;
                m_syncToClientMethods();
            }
        }


        public override void Destroy()
        {
            base.Destroy();

            if (null != m_connectedClients)
            {
                m_connectedClients.Clear();
            }
        }


        #region INetEventListener implementation
        public virtual void OnPeerConnected(NetPeer peer)
        {
            TEDDebug.Log("[SERVER] We have new peer connected: " + peer.EndPoint);

            if (!m_connectedClients.Contains(peer))
            {
                m_connectedClients.Add(peer);
            }
            else
            {
                TEDDebug.LogError("[SERVER] We get repeated peer connected: " + peer.EndPoint);
            }
        }


        public virtual void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            TEDDebug.Log("[SERVER] peer disconnected " + peer.EndPoint + ", info: " + disconnectInfo.Reason);

            for(int i = 0; i < m_connectedClients.Count; i++)
            {
                if(m_connectedClients[i] == peer)
                {
                    m_connectedClients.RemoveAt(i);
                    break;
                }
            }
        }


        public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
        {
            TEDDebug.LogError("[SERVER] error " + socketErrorCode);
        }


        public virtual void OnNetworkReceive(NetPeer peer, NetDataReader reader)
        {
            if (null == reader)
            {
                return;
            }
        }


        public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
        {
            if (messageType == UnconnectedMessageType.DiscoveryRequest)
            {
                TEDDebug.Log("[Server] recieved discovery response. connecting to:" + remoteEndPoint.ToString());
                m_netManager.SendDiscoveryResponse(new byte[] { 1 }, remoteEndPoint);
            }
        }


        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }
        #endregion
    }
}