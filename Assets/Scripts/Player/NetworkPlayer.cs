using System.Collections;
using Event.Events.Network.Login;
using Network;
using Network.Packet.Packets;
using UnityEngine;
using Utils;

namespace Player {
    
    public class NetworkPlayer : MonoBehaviour {

        public string username;
        
        public NetworkClient networkClient;
        
        private UDPClient udpClient;
        
        private void Awake() {
            LoginSuccessEvent.RegisterListener(LoginEvent);
            udpClient = UDPClient.GetInstance();
        }

        private void OnDestroy() {
            LoginSuccessEvent.UnregisterListener(LoginEvent);
        }

        private void LoginEvent(LoginSuccessEvent e) {
            
            networkClient = new NetworkClient(e.uuid, username);
            print("Successfully logged in as " + networkClient.username + " with uuid: " + networkClient.uuid);
            
            MainThreadDispatcher.RunOnMainThread(() => {
                
                //DELETE LOGIN UI FIELD
                Destroy(GameObject.Find("Login"));
                
                Vector3 position = transform.position;

                Packet04Summon packet = new Packet04Summon(networkClient.uuid, position.x, position.y);
                udpClient.sendData(packet);
                
            });

        }

    }
    
}