namespace Network.Packet.Packets {
    
    public sealed class Packet04Awake : Packet {

        private string uuid;
        
        public Packet04Awake(string uuid) : base(PacketTypeOutgoing.AWAKE) {
            this.uuid = uuid;
            ConstructPacket();
        }

        protected override void ConstructPacket() {
            data = System.Text.Encoding.UTF8.GetBytes(uuid.Length + ":" + ConvertId() + ":" + uuid);
        }
    }
    
}