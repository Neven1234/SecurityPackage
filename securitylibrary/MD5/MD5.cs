using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.MD5
{
    public class MD5
    {
        //<< left shift
        //>> right shift
        public uint a = 0x67452301;
        public uint b = 0xEFCDAB89;
        public uint c = 0x98BADCFE;
        public uint d = 0x10325476;
        
        static int[] s = new int[64] {
        7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
        5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
        4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
        6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        };

        uint[] table = new uint[64];

        public uint Rotate_Left(uint x, int n)
        {
            return (x << n) | (x >> (32 - n));
        }
        public int[] Cal_Block(int i, byte[] message,int messLenBytes, byte[] paddingBytes)
        {
            //each block 32 bit or 256 byte

            int[] Block_Message = new int[16];
            int index = i * 64;//<<6//512bit
            for (int j = 0; j < 64; j++)
            {
                if ((index < messLenBytes))
                {
                    int invbyte = (int)message[index] << 24;

                    Block_Message[j / 4] = invbyte | ((int)((uint)Block_Message[j / 4]/256));//>>8 == /256
                }
                else
                {
                    int invbyte = (int)paddingBytes[index - messLenBytes] << 24;
                    Block_Message[j / 4] = invbyte | ((int)((uint)Block_Message[j / 4] /256));
                }
                index++;
            }
            /*int[] block = new int[16];
            byte[] blockBytes = new byte[64];
            for (int j = 0; j < 16; j++)
            {
                block[j] = BitConverter.ToUInt32(blockBytes, j * 4);
            }*/
            return Block_Message;

        }
        public byte[] padding(byte[] paddingBytes, long messageLenBits)
        {
            
            for (int i = 0; i < 8; i++)
            {
                paddingBytes[paddingBytes.Length - 8 + i] = (byte)messageLenBits;

                messageLenBits = messageLenBits / 256;//>>8
            }
            return paddingBytes;
        }
       public int  block_number(int messageLenBytes)
        {

            int y = (int)((messageLenBytes + 8) / 64);//trible shift ==/64

            int numblocks = y + 1; //(messageLenBytes + 8) >> 6
            return numblocks;
        }
        public string GetHash(string text)
        {
            

            // cal padding divide them to blocks each block 512 bit or 64 byte that is the same 
            ///////////////////////////////////start
            
            byte[] message = Encoding.ASCII.GetBytes(text);


            int messageLenBytes = message.Length;

            int totalLen = block_number(messageLenBytes) * 64;//<<6 each block 64 byte  is 64 *2 =512bit

            byte[] paddingBytes = new byte[totalLen - messageLenBytes];

            paddingBytes[0] = (byte)0x80;

            long messageLenBits = messageLenBytes * 8;//<<3

           paddingBytes=padding(paddingBytes, messageLenBits);

            /////////////////////////////////end
           
          
           
            int[] Block_Part ;
            for (int i = 0; i < block_number(messageLenBytes); i++)
            {
                Block_Part = Cal_Block(i, message, messageLenBytes, paddingBytes);
                //cal table t[i]
                ///////////////////////////////////start
                for (int j = 0; j < 64; j++)
                {
                    table[j] = (uint)((Math.Pow(2, 32)) * Math.Abs(Math.Sin(j + 1)));
                }

                /////////////////////////////////////end
                uint originalA = a;
                uint originalB = b;
                uint originalC = c;
                uint originalD = d;
                for (int j = 0; j < 64; j++)
                {

                    int div_16 = j / 16;

                    int f = 0;

                    int num_block = j;

                    if (div_16 == 0)
                    {
                        f = (int)((b & c) | (~b & d));
                    }
                    else if (div_16 == 1)
                    {
                        f = (int)((b & d) | (c & ~d));
                        num_block = (j * 5 + 1) % 16;
                    }
                    else if (div_16 == 2)

                    {
                        f = (int)(b ^ c ^ d);
                        num_block = (j * 3 + 5) % 16;
                    }
                    else if (div_16 == 3)
                    {
                        f = (int)(c ^ (b | ~d));
                        num_block = (j * 7) % 16;
                    }
                   
                    uint temp = (uint)(b + Rotate_Left((uint)(a + f + Block_Part[num_block] + table[j]), s[j]));
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
            byte[] MD5 = new byte[16];
            int count = 0;
            int k = 0;
            int m = 0;
            
            while (k<4)
            {
                int n = 0;
                if      (k == 0) { n = (int)a; }
                else if (k == 1) { n = (int)b; }
                else if (k == 2) { n = (int)c; }
                else if (k == 3) { n = (int)d; }
                for (int j = 0; j < 4; j++)
                {
                    MD5[count] = (byte)n;
                    count++;
                    n = (int)((uint)n/256);
                }
                k++;
            }
              return BitConverter.ToString(MD5).Replace("-", "");
        }
    }
}
