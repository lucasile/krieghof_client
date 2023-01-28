using System;
using System.Collections.Generic;
using Event.Events.Network.Disconnect;
using Event.Events.Network.Login;
using Event.Events.Network.Movement;
using Event.Events.Network.Player;
using Network.Packet.Packets;
using UnityEngine;

namespace Network.Packet {
    
    public class PacketHandler {

        private UDPClient udpClient;
        
        private readonly Dictionary<int, PacketTypeIncoming> packetTypeDict;

        public PacketHandler(UDPClient udpClient) {
            this.udpClient = udpClient;
            packetTypeDict = new Dictionary<int, PacketTypeIncoming>();
            InitDictionary();
        }

        public void ProcessData(byte[] data) {

            string message = System.Text.Encoding.UTF8.GetString(data);
            
            string[] messageSplit = message.Split(':');

            int length;
            
            try {
                length = Convert.ToInt32(messageSplit[0]);
            } catch (Exception ignored) {
                return;
            }

            if (length < 0) {
                return;
            }

            int identifier = Convert.ToInt32(messageSplit[1]);

            PacketTypeIncoming packetType = packetTypeDict[identifier];

            string content = messageSplit[2].Substring(0, length);

            Packet packet = CreatePacket(packetType, content);

            if (packet.packetTypeOutgoing == PacketTypeOutgoing.INVALID) {
                return;
            }

            udpClient.sendData(packet);

        }

        private Packet CreatePacket(PacketTypeIncoming packetType, string data) {

            switch (packetType) {

                case PacketTypeIncoming.LOGIN_SUCCESS: {
                    LoginSuccessEvent loginEvent = new LoginSuccessEvent(data.Split('/')[0]);
                    loginEvent.FireEvent();
                    return new PacketInvalid();
                }

                case PacketTypeIncoming.SUMMON_PLAYER: {
                    string[] dataSplit = data.Split('/');
                    string uuid = dataSplit[0];
                    string username = dataSplit[1];
                    float posX = float.Parse(dataSplit[2]);
                    float posY = float.Parse(dataSplit[3]);
                    SummonEvent summonEvent = new SummonEvent(uuid, username, posX, posY);
                    summonEvent.FireEvent();
                    return new PacketInvalid();
                }

                case PacketTypeIncoming.MOVEMENT: {
                    string[] dataSplit = data.Split('/');
                    string uuid = dataSplit[0];
                    float posX = float.Parse(dataSplit[1]);
                    float posY = float.Parse(dataSplit[2]);
                    float time = float.Parse(dataSplit[3]);
                    MovementSyncEvent moveEvent = new MovementSyncEvent(uuid, posX, posY, time);
                    moveEvent.FireEvent();
                    return new PacketInvalid();
                }

                case PacketTypeIncoming.DISCONNECT: {
                    string uuid = data;
                    Debug.Log(uuid);
                    DisconnectEvent disconnectEvent = new DisconnectEvent(uuid);
                    disconnectEvent.FireEvent();
                    return new PacketInvalid();
                }

            }
            
            return new PacketInvalid();
            
        }

        private void InitDictionary() {
            foreach (PacketTypeIncoming packetType in Enum.GetValues(typeof(PacketTypeIncoming))) {
                packetTypeDict.Add((int) packetType, packetType);
            }
        }
        
    }
    
}