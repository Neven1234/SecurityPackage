using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.MD5
{
    public class MD5
    {
        public uint A = 0x67452301;
        public uint B = 0xEFCDAB89;
        public uint C = 0x98BADCFE;
        public uint D = 0x10325476;
        static int[] s = new int[64] {
        7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
        5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
        4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
        6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        };
        public uint[] Table()
        {
            uint[] t = new uint[64];

            for (int i = 0; i < s.Length; i++)
                t[i] = (uint)(long)((Math.Pow(2, 32)) * Math.Abs(Math.Sin(i + 1)));
            return t;
        }

        public uint rotateLeft(uint x, int n)
        {
            return (x << n) | (x >> (32 - n));
        }
        public string GetHash(string text)
        {
            uint[] table = new uint[64];
            for (int i = 0; i < 64; i++)
                table[i] = (uint)(long)((Math.Pow(2, 32)) * Math.Abs(Math.Sin(i + 1)));
            byte[] message = Encoding.ASCII.GetBytes(text);

            int messLenBytes = message.Length;

            int y = (int)((messLenBytes + 8) / 64);//trible shift ==/64

            int numblocks = y + 1; //(messLenBytes + 8) >> 6

            int totalLen = numblocks * 64;//<<6 each block 64 byte  is 64 *2 =512bit

            byte[] paddingBytes = new byte[totalLen - messLenBytes];

            paddingBytes[0] = (byte)0x80;

            long messageLenBits = (long)messLenBytes * 8;//<<3

            for (int i = 0; i < 8; i++)
            {
                paddingBytes[paddingBytes.Length - 8 + i] = (byte)messageLenBits;
                messageLenBits = messageLenBits / 256;//<<8
            }
            /*paddingBytes[messLenBytes] = (byte)0x80;
            for (int i = messLenBytes + 1; i < paddingBytes.Length - 8; i++)
            {
                paddingBytes[i] = 0x00;
            }*/
            uint a = A;
            uint b = B;
            uint c = C;
            uint d = D;
            int[] buffer = new int[16];
            for (int i = 0; i < numblocks; i++)
            {
                int index = i * 64;//<<6//512bit
                for (int j = 0; j < 64; j++, index++)
                {
                    if ((index < messLenBytes))
                    {
                        buffer[j / 4] = (int)message[index] << 24 | ((int)((uint)buffer[j / 4] / 256));
                    }
                    else
                    {
                        buffer[j / 4] = (int)paddingBytes[index - messLenBytes] << 24 | ((int)((uint)buffer[j / 4] / 256));
                    }

                    //buffer[j/4] = ((int)((index < messLenBytes) ? message[index] : paddingBytes[index - messLenBytes]) << 24) | ((int)TripleShift(buffer[TripleShift(j, 2)], 8));//buffer[TripleShift(j,2)] >>> 8
                }
                uint originalA = a;
                uint originalB = b;
                uint originalC = c;
                uint originalD = d;
                for (int j = 0; j < 64; j++)
                {
                    int div16 = j / 16;
                    int f = 0;
                    int g = j;
                    switch (div16)
                    {
                        case 0:
                            f = (int)((b & c) | (~b & d));
                            break;
                        case 1:
                            f = (int)((b & d) | (c & ~d));
                            g = (j * 5 + 1) % 16;
                            break;
                        case 2:
                            f = (int)(b ^ c ^ d);
                            g = (j * 3 + 5) % 16;
                            break;
                        case 3:
                            f = (int)(c ^ (b | ~d));
                            g = (j * 7) % 16;
                            break;

                    }
                    uint temp = (uint)(b + rotateLeft((uint)(a + f + buffer[g] + table[j]), s[j]));
                    a = d;
                    d = c;
                    c = b;
                    b = temp;
                }
                a += originalA;
                b += originalB;
                c += originalC;
                d += originalD;
            }
            byte[] md5 = new byte[16];
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                int n = 0;
                if (i == 0)
                { n = (int)a; }
                else if (i == 1) { n = (int)b; }
                else if (i == 2) { n = (int)c; }
                else if (i == 3) { n = (int)d; }
                for (int j = 0; j < 4; j++)
                {
                    md5[count++] = (byte)n;
                    n = (int)((uint)n / 256);
                }
            }
              return BitConverter.ToString(md5).Replace("-", "");
        }
    }
}
