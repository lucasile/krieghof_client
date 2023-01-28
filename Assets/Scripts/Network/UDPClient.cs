using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Network.Packet;
using UnityEngine;

namespace Network {

    public class UDPClient : MonoBehaviour {

        //SINGLETON CLASS

        private static UDPClient instance;

        private PacketHandler packetHandler;
        
        private Thread listenThread;

        private static int localPort;
        
        private string ip;
        private int port;

        private readonly int MIN_PACKET_SIZE = 2;
        
        private IPEndPoint remoteEndPoint;
        public UdpClient socket;
        
        private string _lastPacket;

        private void Awake() {
            
            //CONTROL SINGLETON CONCURRENCY
            
            if (instance != null) {
                Destroy(this);
            }
            
            instance = this;
            packetHandler = new PacketHandler(this);
            ip = NetworkData.IP;
            port = NetworkData.SERVER_PORT;
            
        }

        private void Start() {
            Init();
        }
        
        private void Init() {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new UdpClient();
            socket.Connect(remoteEndPoint);
            socket.BeginReceive(ReceiveCallback, null);
        }

        public void sendData(Packet.Packet packet) {
            byte[] data = packet.data;
            socket.BeginSend(data, data.Length, SendCallback, null);
        }

        private void SendCallback(IAsyncResult result) {
            Socket client = (Socket) result.AsyncState;
            client.EndSend(result);
        }

        private void ReceiveCallback(IAsyncResult result) {

            try {

                byte[] data = socket.EndReceive(result, ref remoteEndPoint);

                socket.BeginReceive(ReceiveCallback, null);

                if (data.Length < MIN_PACKET_SIZE) {
                    return;
                }
                
                packetHandler.ProcessData(data);

            } catch (Exception e) { 
                print(e);  
            }
            
        }

        public static UDPClient GetInstance() {
            return instance;
        }

    }

}
