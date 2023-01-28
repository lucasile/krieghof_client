using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network {

    [Serializable]
    public class NetworkClient {

        public string uuid;
        public string username;
        
        public NetworkClient(string uuid, string username) {
            this.uuid = uuid;
            this.username = username;
        }

    }

}
