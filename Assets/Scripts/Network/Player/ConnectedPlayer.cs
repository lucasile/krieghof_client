using Player;
using UnityEngine;

namespace Network.Player {

    public struct ConnectedPlayer {

        public OtherPlayer otherPlayer;
        public NetworkClient networkClient;
        public string uuid;
        public string username;

        public ConnectedPlayer(OtherPlayer otherPlayer, NetworkClient networkClient, string uuid, string username) {
            this.otherPlayer = otherPlayer;
            this.networkClient = networkClient;
            this.uuid = uuid;
            this.username = username;
        }
    }
    
}