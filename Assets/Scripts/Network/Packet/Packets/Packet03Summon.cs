namespace Network.Packet.Packets {
    
    public sealed class Packet04Summon : Packet {

        private string uuid, username;
        private float posX, posY;
        
        public Packet04Summon(string uuid, float posX, float posY) : base(PacketTypeOutgoing.SUMMON_PLAYER) {
            this.uuid = uuid;
            this.posX = posX;
            this.posY = posY;
            ConstructPacket();
        }

        protected override void ConstructPacket() {
            string dataMessage = uuid + ":" + posX + "/" + posY;
            data = System.Text.Encoding.UTF8.GetBytes(dataMessage.Length + ":" + ConvertId() + ":" + dataMessage);
        }
    }
    
}