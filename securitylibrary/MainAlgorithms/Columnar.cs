using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>

    {
        //List pf list to handle all possible permutes
        List<List<int>> All_possible_permutes = new List<List<int>>();
        // To get permutation
        public void get_permutation(int[] nums, int start, int end)
        {
            if (start == end)
            {
                All_possible_permutes.Add(new List<int>(nums));
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    Swap(ref nums[start], ref nums[i]);
                    get_permutation(nums, start + 1, end);
                    Swap(ref nums[start], ref nums[i]);
                }
            }
        }
        // Swap function 
        public static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        // making matrix for encryption and decryption
        public char[,] Make_Matrix(string Text, List<int> key, int get_coumns, int get_rows)
        {
            int columns = key.Count;
            int rows = (int)Math.Ceiling((double)Text.Length / columns);

            char[,] matrix = new char[rows, columns];
            int Counter = 0;
            for (int i = 0; i < matrix.GetLength(get_rows); i++)
            {
                for (int j = 0; j < matrix.GetLength(get_coumns); j++)
                {

                    if (Counter == Text.Length)
                    { break; }
                    if (get_coumns == 1)
                    { matrix[i, j] = Text[Counter]; }
                    if (get_rows == 1)
                    { matrix[j, i] = Text[Counter]; }

                    Counter++;

                }

            }
            return matrix;

        }
        //Make text to encryption
        public string Conc_text_To_enc(char[,] Matrix, int get_coumns, int get_rows, List<int> key)
        {
            Dictionary<int, string> MatrixElemnts = new Dictionary<int, string>();
            string CT = "";
            for (int i = 0; i < Matrix.GetLength(get_coumns); i++)
            {
                string emptyCompare = "";
                for (int j = 0; j < Matrix.GetLength(get_rows); j++)
                {
                    emptyCompare += Matrix[j, i];
                    MatrixElemnts[key[i]] = emptyCompare;
                }
            }

            for (int i = 1; i <= MatrixElemnts.Count; i++)
            {
                CT += MatrixElemnts[i];
            }

            return CT.ToUpper();
        }
        //To store index of the key
        public List<int> store_index(List<int> key, int columns)
        {
            List<int> key_storage = new List<int>();
            int count = 1;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (key[j] == count)
                    {
                        key_storage.Add(j);
                        count++;
                        //break;
                    }
                }
            }
            return key_storage;
        }
        public List<int> Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            //list of keys
            List<int> key = new List<int>();
            int columns = 1;
            //brute force on the columns
            while (columns < cipherText.Length)
            {
                int[] no_of_permutes = new int[columns];
                for (int i = 0; i < columns; i++)
                {
                    no_of_permutes[i] = i + 1;
                }
                get_permutation(no_of_permutes, 0, no_of_permutes.Length - 1);

                foreach (var a in All_possible_permutes)
                {
                    key = a;
                    string new_cipher = Encrypt(plainText, key);
                    //An enumeration value that represents a case-insensitive string comparison using the invariant culture
                    if (new_cipher.Equals(cipherText, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return key;
                    }
                }
                All_possible_permutes.Clear();

                columns++;
            }


            return new List<int> { -1 };
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            string plainText = "";
            int columns = key.Count;
            int rows = (int)Math.Round((decimal)cipherText.Length / columns);
            //To store the index of the key
            List<int> key_storage;
            key_storage = store_index(key, columns);
            //Matrix of decryption
            char[,] dec_matrix;
            dec_matrix = Make_Matrix(cipherText.ToLower(), key, 0, 1);
            //make original mayrix
            char[,] originalMessageMatrix = new char[rows, columns];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    originalMessageMatrix[j, key_storage[i]] = dec_matrix[j, i];
                }
            }
            //Restore the plain text
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    plainText += originalMessageMatrix[i, j];
                }
            }

            return plainText.ToUpper();
        }

        public string Encrypt(string plainText, List<int> key)
        {
            //make matrix
            char[,] enc_matrix;
            string cipher = "";
            enc_matrix = Make_Matrix(plainText.ToUpper(), key, 1, 0);
            //make text
            cipher = Conc_text_To_enc(enc_matrix, 1, 0, key);
            return cipher;
        }
    }
}
