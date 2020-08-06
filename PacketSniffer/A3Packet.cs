using PacketDotNet;
using SharpPcap;
using System;
using System.Linq;

namespace PacketSniffer
{
    class A3Packet
    {
        public RawCapture p;
        public byte[] Data;
        public bool IsIncoming = false;

        public string Time { get; private set; }
        public string ServerIp { get; private set; }
        public int Port { get; private set; }
        public int Length { get; private set; }
        public string Type { get; private set; }
        public string Proto { get; private set; }

        public A3Packet(RawCapture p, Config config, Crypt crypt)
        {
            this.p = p;
            Packet packet = Packet.ParsePacket(p.LinkLayerType, p.Data);
            var tcpPacket = (TcpPacket)packet.PayloadPacket.PayloadPacket;
            var ipPacket = (IPPacket)packet.PayloadPacket;
            Length = tcpPacket.PayloadData.Length;
            Time = p.Timeval.Date.ToString("dd-MMM-yy HH:mm:ss tt");

            if (tcpPacket.SourcePort == config.LoginAgentPort || tcpPacket.SourcePort == config.ZoneAgentPort)
                IsIncoming = true;

            Port = IsIncoming ? tcpPacket.SourcePort : tcpPacket.DestinationPort;

            Type = IsIncoming ? "S2C_UNKNOWN" : "C2S_UNKNWON"; 

            ServerIp = IsIncoming ? ipPacket.SourceAddress.ToString() : ipPacket.DestinationAddress.ToString();

            if (Port == config.ZoneAgentPort && tcpPacket.PayloadData[8] == 0x03 && tcpPacket.PayloadData[9] == 0xFF)
            {
                byte[] temp = new byte[tcpPacket.PayloadData.Length];
                Array.Copy(tcpPacket.PayloadData, temp, tcpPacket.PayloadData.Length);
                crypt.Decrypt(ref temp);
                Data = temp;
                if (Data.Length > 11)
                {
                    var protocol = BitConverter.ToUInt16(temp.Skip(10).Take(2).ToArray(), 0);
                    var hexProtocol = $"0x{protocol:X}";
                    Proto = hexProtocol;
                    Type = Protocol.GetPacketProtocolName(tcpPacket.PayloadData, IsIncoming);
                }
            }
            else
            {
                Data = tcpPacket.PayloadData;
            }
        }

    }
}
