using UnityEngine;

namespace Network.Packet.Packets {
    
    public sealed class Packet00LoginAttempt : Packet {

        private string username;
        private float posX, posY;
        
        public Packet00LoginAttempt(string username, float posX, float posY) : base(PacketTypeOutgoing.LOGIN_ATTEMPT) {
            this.username = username;
            this.posX = posX;
            this.posY = posY;
            ConstructPacket();
        }

        protected override void ConstructPacket() {
            string dataMessage = username + "/" + posX + "/" + posY;
            string message = dataMessage.Length + ":" + ConvertId() + ":" + dataMessage;
            data = System.Text.Encoding.UTF8.GetBytes(message);
        }
        
    }
    
}