namespace Network.Packet {
    
    public enum PacketTypeIncoming {
        
        INVALID = -1,
        LOGIN_SUCCESS = 00,
        LOGIN_FAIL = 01,
        DISCONNECT = 02,
        MOVEMENT = 03,
        SUMMON_PLAYER = 04
        
    }
    
    public enum PacketTypeOutgoing {
        
        INVALID = -1,
        LOGIN_ATTEMPT = 00,
        DISCONNECT = 01,
        MOVEMENT = 02,
        SUMMON_PLAYER = 03,
        AWAKE = 04
        
    }
    
}