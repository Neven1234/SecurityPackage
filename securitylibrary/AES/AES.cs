using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
              // throw new NotImplementedException();
            string[,] plainText_Matrix = Convert_To_Matrix(plainText);
            string[,] key_Matrix = Convert_To_Matrix(key);
            plainText_Matrix = XOR(plainText_Matrix, key_Matrix);
            int i = 0;
            while(i<9)
            {
                plainText_Matrix = SubBytes(plainText_Matrix);
                i++;
            }
            return plainText;

        }
        public string[,] Convert_To_Matrix(string str)
        {
            string[,] Matrix = new string[4, 4];
            int c1= 2;//skip 0x
            string two_char = "";
            for (int i=0;i<Matrix.GetLength(0);i++)
            {
                for(int j=0;j<Matrix.GetLength(1);j++)
                {
                    char[] chars = { str[c1], str[c1+1] };
                     two_char = new string(chars);
                    Matrix[j, i] = two_char;
                    if (c1 >= str.Length)
                        break;
                    c1 += 2;

                }
            }
            return Matrix;

        }

  
        public string[,] XOR(string[,] M, string[,] K)
        {
            string[,] Matrix = new string[4, 4];
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    string s_bin_1 = Convert.ToString(Convert.ToInt64(M[i, j], 16), 2).PadLeft(8, '0');
                    string s_bin_2 = Convert.ToString(Convert.ToInt64(K[i, j], 16), 2).PadLeft(8, '0');                   
                  
                 
                    long tmp = Convert.ToInt64(s_bin_1, 2) ^ Convert.ToInt64(s_bin_2, 2);

                    Matrix[i, j] = Convert.ToString(tmp, 16).PadLeft(2, '0');
                }
            }

            return Matrix;

        }
        public string[,] SubBytes(string[,] plain)
        {
            string[,] Matrix = new string[4, 4];

            return Matrix;

        }
    }
}
