namespace Network.Packet {
    
    public class Packet {

        public PacketTypeOutgoing packetTypeOutgoing;

        public int id;
        public byte[] data;

        public Packet(PacketTypeOutgoing packetTypeOutgoing) {
            this.packetTypeOutgoing = packetTypeOutgoing;
            id = (int) packetTypeOutgoing;
        }

        protected virtual void ConstructPacket() {}

        protected string ConvertId() {
            if (id < 10)
                return "0" + id;
            return id.ToString();
        }
        
    }
    
}