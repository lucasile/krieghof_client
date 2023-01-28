using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Event.Events.Network.Disconnect;
using Event.Events.Network.Movement;
using Event.Events.Network.Player;
using Network.Packet.Packets;
using Player;
using TMPro;
using UnityEngine;
using Utils;
using NetworkPlayer = Player.NetworkPlayer;

namespace Network.Player {
    
    public class PlayerManager : MonoBehaviour {

        //SINGLETON CLASS
        
        private static PlayerManager instance;
        private static bool canDisconnect = true;
        
        private MainThreadDispatcher mainThreadDispatcher;
        
        private UDPClient udpClient;
        public NetworkPlayer networkPlayer;
        
        [SerializeField] private GameObject newPlayerPrefab;
        
        public Dictionary<string, ConnectedPlayer> playersConnected;

        private Vector3 _lastPostition;
        
        private readonly float packetsPerSecondLimit = 5f;
        private readonly float awakePeriod = 3f;
        
        private float packetInterval;
        private float nextPacketTime;

        private void Awake() {

            //CONTROL SINGLETON CONCURRENCY
            
            if (instance != null) {
                Destroy(this);
            }
            
            instance = this;
            
            playersConnected = new Dictionary<string, ConnectedPlayer>();
            
            packetInterval = 1 / packetsPerSecondLimit;
            nextPacketTime = 0f;
            
            SummonEvent.RegisterListener(LoadPlayer);
            DisconnectEvent.RegisterListener(Disconnect);
            
        }

        private void OnDestroy() {
            SummonEvent.UnregisterListener(LoadPlayer);
            DisconnectEvent.UnregisterListener(Disconnect);
        }
        
        [RuntimeInitializeOnLoadMethod]
        public static void RunOnStart() {
            Application.wantsToQuit += WantsToQuit;
        }

        private static bool WantsToQuit() {
            instance.StartCoroutine(SendQuitPacket());
            return canDisconnect;
        }

        private static IEnumerator SendQuitPacket() {
            Packet01Disconnect packet = new Packet01Disconnect(instance.networkPlayer.networkClient.uuid);
            instance.udpClient.sendData(packet);
            yield return new WaitForSeconds(0.5f);
            instance.udpClient.socket.Close();
            canDisconnect = true;
            Application.Quit();
        }

        private IEnumerator AwakeLoop() {
            yield return new WaitForSeconds(awakePeriod);
            Packet04Awake packet = new Packet04Awake(networkPlayer.networkClient.uuid);
            udpClient.sendData(packet);
            if (!canDisconnect) {
                StartCoroutine(AwakeLoop());
            }
        }

        public void InitManager() {
            udpClient = UDPClient.GetInstance();
            canDisconnect = false;
            StartCoroutine(AwakeLoop());
        }
        
        //MOVEMENT SYNC PACKET
        public bool SendSync(Vector3 position) {

            if (_lastPostition == position)
                return false;
            
            // LIMIT TO X PACKETS PER SECOND
            if (Time.time <= nextPacketTime)
                return false;

            nextPacketTime = Time.time + packetInterval;
            
            Packet02Movement packet = new Packet02Movement(networkPlayer.networkClient.uuid, position.x, position.y, Time.time);
            udpClient.sendData(packet);

            _lastPostition = position;

            return true;

        }

        private void LoadPlayer(SummonEvent e) {
            
            if (playersConnected.ContainsKey(e.uuid))
                return;
            
            MainThreadDispatcher.RunOnMainThread(() => {

                GameObject playerGameObject = Instantiate(newPlayerPrefab, new Vector3(e.posX, e.posY, 0), transform.rotation);

                NetworkClient networkClient = new NetworkClient(e.uuid, e.username);
                ConnectedPlayer connectedPlayer = new ConnectedPlayer(playerGameObject.GetComponent<OtherPlayer>(), networkClient, networkClient.uuid, networkClient.username);

                TextMeshPro text = playerGameObject.GetComponentInChildren<TextMeshPro>();
                text.text = networkClient.username;
                
                playersConnected.Add(networkClient.uuid, connectedPlayer);
                
            });

        }

        private void Disconnect(DisconnectEvent e) {

            print(e.uuid);
            
            string uuid = e.uuid;
            
            if (!playersConnected.ContainsKey(uuid))
                return;

            Destroy(playersConnected[uuid].otherPlayer.gameObject);
            
            playersConnected.Remove(uuid);

        }

    }
    
}