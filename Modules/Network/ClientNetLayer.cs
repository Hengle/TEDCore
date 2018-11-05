using LiteNetLib;
using LiteNetLib.Utils;

namespace TEDCore.Network
{
    public class ClientNetLayer : BaseNetLayer, INetEventListener
    {
        private NetPeer m_server;

        public void Init(string connectKey, string address, int port, int updateTime, bool simulateLatency, int simulationMinLatency, int simulationMaxLatency)
        {
            Destroy();

            m_netManager = new NetManager(this, connectKey);
            m_netManager.NatPunchEnabled = true;
            m_netManager.UpdateTime = updateTime;
            m_netManager.SimulateLatency = simulateLatency;
            m_netManager.SimulationMinLatency = simulationMinLatency;
            m_netManager.SimulationMaxLatency = simulationMaxLatency;
            m_netManager.Start();
            m_netManager.Connect(address, port);

            m_dataWriter = new NetDataWriter();
        }


        public void SendToServer(NetDataWriter netDataWriter, SendOptions sendOption)
        {
            if (null != m_server)
            {
                m_server.Send(netDataWriter, sendOption);
            }
        }


        #region INetEventListener implementation
        public void OnPeerConnected(NetPeer peer)
        {
            m_server = peer;
            TEDDebug.Log("[CLIENT] We connected to " + peer.EndPoint);
        }


        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            TEDDebug.Log("[CLIENT] We disconnected because " + disconnectInfo.Reason);
        }


        public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
        {
            TEDDebug.Log("[CLIENT] We received error " + socketErrorCode);
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
            if (messageType == UnconnectedMessageType.DiscoveryResponse && m_netManager.PeersCount == 0)
            {
                TEDDebug.Log("[CLIENT] Received discovery response. Connecting to: " + remoteEndPoint);
                m_netManager.Connect(remoteEndPoint);
            }
        }


        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }
        #endregion
    }
}