namespace Network.Packet.Packets {
    
    public class Packet01Disconnect : Packet {

        private string uuid;
        
        public Packet01Disconnect(string uuid) : base(PacketTypeOutgoing.DISCONNECT) {
            this.uuid = uuid;
            ConstructPacket();
        }

        protected sealed override void ConstructPacket() {
            string message = uuid.Length + ":" + ConvertId() + ":" + uuid;
            data = System.Text.Encoding.UTF8.GetBytes(message);
        }
    }
    
}