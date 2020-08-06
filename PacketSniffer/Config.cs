namespace PacketSniffer
{
    class Config
    {
        public string ServerIp { get; set; }
        public ushort LoginAgentPort { get; set; }
        public ushort ZoneAgentPort { get; set; }
        public bool ShowUnique { get; set; }
    }
}
