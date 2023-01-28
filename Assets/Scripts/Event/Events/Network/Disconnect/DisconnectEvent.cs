namespace Event.Events.Network.Disconnect {
    
    public class DisconnectEvent : EventSystem<DisconnectEvent> {

        public string uuid;
        
        public DisconnectEvent(string uuid) {
            this.uuid = uuid;
        }
        
    }
    
}