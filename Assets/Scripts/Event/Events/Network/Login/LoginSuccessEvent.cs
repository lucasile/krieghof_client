using System.Collections;
using System.Collections.Generic;
using Event;
using UnityEngine;

namespace Event.Events.Network.Login {

    public class LoginSuccessEvent : EventSystem<LoginSuccessEvent> {

        public string uuid;
        
        public LoginSuccessEvent(string uuid) {
            this.uuid = uuid;
        }
        
    }

}
