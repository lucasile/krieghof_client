using System.Collections;
using System.Collections.Generic;
using Network;
using Network.Packet.Packets;
using Network.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NetworkPlayer = Player.NetworkPlayer;

namespace UI {

    public class Login : MonoBehaviour {

        private UDPClient udpClient;

        [SerializeField] private GameObject playerObject;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI usernameInput;
        
        private void Start() {
            button.onClick.AddListener(LoginButtonPress);
        }

        private void LoginButtonPress() {

            if (usernameInput == null)
                return;
            
            if (usernameInput.text == null)
                return;

            if (usernameInput.text == " " || usernameInput.text == "")
                return;
            
            AttemptLogin(usernameInput.text.Replace(" ", ""));

        }
        
        private void AttemptLogin(string username) {

            GameObject player = Instantiate(playerObject);
            
            // SET USERNAME TO NETWORK PLAYER
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
            networkPlayer.username = username;
            
            // INITIALIZE PLAYER MANAGER UDP CLIENT
            PlayerManager playerManager = FindObjectOfType<PlayerManager>();
            playerManager.networkPlayer = networkPlayer;
            playerManager.InitManager();

            Vector3 position = player.transform.position;
            
            Packet00LoginAttempt packet = new Packet00LoginAttempt(username, position.x, position.y);
            udpClient = UDPClient.GetInstance();
            udpClient.sendData(packet);
            
        }

    }

}
