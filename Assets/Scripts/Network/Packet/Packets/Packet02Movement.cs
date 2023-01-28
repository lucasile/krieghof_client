namespace Network.Packet.Packets {
    
    public sealed class Packet02Movement : Packet {

        private string uuid;
        private float posX, posY;
        private float time;
        
        public Packet02Movement(string uuid, float posX, float posY, float time) : base(PacketTypeOutgoing.MOVEMENT) {
            this.uuid = uuid;
            this.posX = posX;
            this.posY = posY;
            this.time = time;
            ConstructPacket();
        }

        protected override void ConstructPacket() {
            string message = uuid + ":" + posX + "/" + posY + "/" + time;
            data = System.Text.Encoding.UTF8.GetBytes(message.Length + ":" + ConvertId() + ":" + message);
        }
    }
    
}