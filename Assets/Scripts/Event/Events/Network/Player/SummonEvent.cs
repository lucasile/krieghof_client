namespace Event.Events.Network.Player {
    
    public class SummonEvent : EventSystem<SummonEvent> {

        public string uuid, username;
        public float posX, posY;
        
        public SummonEvent(string uuid, string username, float posX, float posY) {
            this.uuid = uuid;
            this.username = username;
            this.posX = posX;
            this.posY = posY;
        }
        
    }
    
}