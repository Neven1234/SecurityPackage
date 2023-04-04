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
        public string[,] XOR(string[,] d1, string[,] d2)
        {
            string[,] Matrix = new string[4, 4];
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {

                    int dec1 = Convert.ToInt32(d1[i, j], 16);
                    int dec2 = Convert.ToInt32(d2[i, j], 16);
                    string tmp = Convert.ToString(dec1^dec2, 16);

                    Matrix[i, j] = tmp;




                }
            }

            return Matrix;

        }

    }
}
