using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketSniffer
{
    public class Crypt
    {
        private int m_ConstKey1;
        private int m_ConstKey2;
        private int m_DynamicKey;

        public Crypt()
        {
            this.m_ConstKey1 = 0x241AE7;
            this.m_ConstKey2 = 0x15DCB2;
            this.m_DynamicKey = 0x4C478BD;
        }

        public Crypt(int constKey1, int constKey2, int dynamicKey)
        {
            this.m_ConstKey1 = constKey1;
            this.m_ConstKey2 = constKey2;
            this.m_DynamicKey = dynamicKey;
        }

        public void Decrypt(ref byte[] buffer)
        {
            int sOffset = 0x0C;
            for (int i = sOffset; i + 4 <= buffer.Length; i += 4)
            {
                int dynamicKey = this.m_DynamicKey;
                for (int j = i; j < i + 4; j++)
                {
                    byte pSrc = buffer[j];
                    buffer[j] = (byte)(buffer[j] ^ (dynamicKey >> 8));
                    dynamicKey = (pSrc + dynamicKey) * this.m_ConstKey1 + this.m_ConstKey2;
                }
            }
        }

        public void Encrypt(ref byte[] buffer)
        {
            int sOffset = 0x0C;
            for (var i = sOffset; i + 4 <= buffer.Length; i += 4)
            {
                int dynamicKey = this.m_DynamicKey;
                for (int j = i; j < i + 4; j++)
                {
                    buffer[j] = (byte)(buffer[j] ^ (dynamicKey >> 8));
                    dynamicKey = (buffer[j] + dynamicKey) * this.m_ConstKey1 + this.m_ConstKey2;
                }
            }
        }
    }
}
