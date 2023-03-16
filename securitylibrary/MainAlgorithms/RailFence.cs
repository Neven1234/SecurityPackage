using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public char[,] GetMatrix(string Text, int key, int byCol, int byRow)
        {
            int num_row = key;
            decimal len = Text.Length;
            decimal num_col = Math.Ceiling(len / key);
            char[,] matrix = new char[num_row, Convert.ToInt32(num_col)];
            int t = 0;
            for (int i = 0; i < matrix.GetLength(byCol); i++)
            {
                for (int j = 0; j < matrix.GetLength(byRow); j++)
                {

                    if (t == Text.Length)
                        break;
                    if (byCol == 1)
                        matrix[j, i] = Text[t];
                    if (byRow == 1)
                        matrix[i, j] = Text[t];
                    t++;

                }

            }
            return matrix;
        }
        public string get_text(char[,] Matrix, int byCol, int byRow)
        {
            string text = "";
            char c = '\0';
            for (int i = 0; i < Matrix.GetLength(byCol); i++)
            {
                for (int j = 0; j < Matrix.GetLength(byRow); j++)
                {
                    if (byCol == 1)
                        if (Matrix[j, i] != c)
                        {
                            text += String.Join("", Matrix[j, i]);
                        }
                    if (byRow == 1)
                        if (Matrix[i, j] != c)
                        {
                            text += String.Join("", Matrix[i, j]);
                        }
                }

            }
            return text;
        }
        public int Analyse(string plainText, string cipherText)
        {
            for (int j = 1; j < Math.Max(plainText.Length, cipherText.Length); j++)
            {
                bool check = cipherText.ToUpper() == Encrypt(plainText, j).ToUpper();
                if (check)
                {
                    return j;
                }
            }
            return 1000;
        }

        public string Decrypt(string cipherText, int key)
        {

            char[,] dec_matrix;
            dec_matrix = GetMatrix(cipherText.ToLower(), key, 0, 1);
            string plain = "";
            plain = get_text(dec_matrix, 1, 0);
            return plain;
        }

        public string Encrypt(string plainText, int key)
        {

            char[,] enc_matrix;
            string cipher = "";
            enc_matrix = GetMatrix(plainText.ToUpper(), key, 1, 0);
            cipher = get_text(enc_matrix, 0, 1);
            return cipher;
        }
    }
}
