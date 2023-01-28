namespace Event.Events.Network.Movement {
    
    public class MovementSyncEvent : EventSystem<MovementSyncEvent> {

        public string uuid;
        public float posX, posY;
        public float time;
        
        public MovementSyncEvent(string uuid, float posX, float posY, float time) {
            this.uuid = uuid;
            this.posX = posX;
            this.posY = posY;
            this.time = time;
        }
        
    }
    
}