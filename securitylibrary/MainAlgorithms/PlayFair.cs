using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {

            var uniquekey = new HashSet<char>(key);
            char[,] matrix = new char[5, 5];
            string restofalpa = "";
            bool IsI = false;
            bool IsJ = false;
            Tuple<int, int> firstChar;
            Tuple<int, int> secondChar;

            foreach (char c in uniquekey)
            {
                if (c == 'i')
                {
                    IsI = true;
                }
                if (c == 'j')
                {
                    IsJ = true;
                }

                restofalpa += c;
            }
            if (IsI && IsJ)
            {
                restofalpa.Remove('j');
            }
            Console.WriteLine(restofalpa);
            string alpha = "abcdefghijklmnopqrstuvwxyz";

            foreach (char c in alpha)
            {

                if (!restofalpa.Contains(c))
                {
                    if (IsI && c == 'j')
                    {
                        continue;
                    }
                    if (IsJ && c == 'i')
                    {
                        continue;
                    }
                    if (!IsI && !IsJ && c == 'j')
                    {
                        continue;
                    }
                    restofalpa += c;
                }
            }

            Console.WriteLine(restofalpa);
            int CharNum = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = restofalpa[CharNum];
                    CharNum++;
                }
            }

            string res = "";
            string plain = "";
            for (var i = 0; i < cipherText.Length; i += 2)
            {
                res = cipherText.Substring(i, Math.Min(2, cipherText.Length - i)).ToLower();
                firstChar = pos(res[0], matrix);
                secondChar = pos(res[1], matrix);
                //same row
                if (firstChar.Item1 == secondChar.Item1)
                {
                    if (secondChar.Item2 == 0)
                    {
                        plain += matrix[firstChar.Item1, firstChar.Item2 - 1];
                        plain += matrix[secondChar.Item1, 4];
                    }
                    else if (firstChar.Item2 == 0)
                    {
                        plain += matrix[firstChar.Item1, 4];
                        plain += matrix[secondChar.Item1, secondChar.Item2 - 1];
                    }
                    else
                    {
                        plain += matrix[firstChar.Item1, firstChar.Item2 - 1];
                        plain += matrix[secondChar.Item1, secondChar.Item2 - 1];
                    }
                }
                // same column
                else if (firstChar.Item2 == secondChar.Item2)
                {
                    if (secondChar.Item1 == 0)
                    {
                        plain += matrix[firstChar.Item1 - 1, firstChar.Item2];
                        plain += matrix[4, secondChar.Item2];
                    }
                    else if (firstChar.Item1 == 0)
                    {
                        plain += matrix[4, firstChar.Item2];
                        plain += matrix[secondChar.Item1 - 1, secondChar.Item2];
                    }
                    else
                    {
                        plain += matrix[firstChar.Item1 - 1, firstChar.Item2];
                        plain += matrix[secondChar.Item1 - 1, secondChar.Item2];
                    }
                }
                /// triangl
                else
                {
                    plain += matrix[firstChar.Item1, secondChar.Item2];
                    plain += matrix[secondChar.Item1, firstChar.Item2];
                }
            }
            string test = "";
            for (var i = 0; i < plain.Length; i += 2)
            {
                test = plain.Substring(i, Math.Min(2, plain.Length - i));
                if (test.Length == 1)
                {
                    i = plain.Length;
                }
                else if (test[1] == 'x')
                {
                    if (i + 2 == plain.Length)
                    {
                        break;
                    }
                    if (test[0] == plain[i + 2])
                    {

                        plain = plain.Remove(i + 1, 1);
                        i--;

                    }
                }
            }

            if (plain[plain.Length - 1] == 'x')
            {
                plain = plain.Remove(plain.Length - 1, 1);
            }
            return plain;
        }

        public string Encrypt(string plainText, string key)
        {

            var uniquekey = new HashSet<char>(key);

            char[,] matrix = new char[5, 5];
            string restofalpa = "";
            bool IsI = false;
            bool IsJ = false;
            Tuple<int, int> firstChar;
            Tuple<int, int> secondChar;

            foreach (char c in uniquekey)
            {
                if (c == 'i')
                {
                    IsI = true;
                }
                if (c == 'j')
                {
                    IsJ = true;
                }

                restofalpa += c;
            }
            if (IsI && IsJ)
            {
                restofalpa.Remove('j');
            }
            Console.WriteLine(restofalpa);
            string alpha = "abcdefghijklmnopqrstuvwxyz";

            foreach (char c in alpha)
            {

                if (!restofalpa.Contains(c))
                {
                    if (IsI && c == 'j')
                    {
                        continue;
                    }
                    if (IsJ && c == 'i')
                    {
                        continue;
                    }
                    if (!IsI && !IsJ && c == 'j')
                    {
                        continue;
                    }
                    restofalpa += c;
                }
            }

            Console.WriteLine(restofalpa);
            int CharNum = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = restofalpa[CharNum];
                    CharNum++;
                }
            }

            string res = "";
            string plain = "";



            string flago = "";
            int counter = 0;
            Console.WriteLine("length 2bl " + plainText.Length);
            while (true)
            {
                string test = "";
                string newplan = "";
                bool flag = false;
                for (var i = 0; i < plainText.Length; i += 2)
                {
                    test = plainText.Substring(i, Math.Min(2, plainText.Length - i));
                    if (test.Length == 1)
                    {
                        i = plainText.Length;
                    }
                    else if (test[0] == test[1])
                    {
                        flag = false;
                        counter++;
                        plainText = plainText.Insert(i + 1, "x");
                    }
                }
                if (flag == false)
                {
                    break;
                }
            }
            if (plainText.Length % 2 != 0)
            {
                Console.WriteLine("true");
                plainText += 'x';
            }
            Console.WriteLine("5arag:");
            Console.WriteLine("length ba3d " + plainText.Length + " counter :" + counter);
            if (plainText.Length % 2 != 0)
            {
                Console.WriteLine("true");
                // newplan += 'x';
            }


            for (var i = 0; i < plainText.Length; i += 2)
            {
                res = plainText.Substring(i, Math.Min(2, plainText.Length - i));
                firstChar = pos(res[0], matrix);
                secondChar = pos(res[1], matrix);
                //same row
                if (firstChar.Item1 == secondChar.Item1)
                {
                    if (secondChar.Item2 == 4)
                    {
                        plain += matrix[firstChar.Item1, firstChar.Item2 + 1];
                        plain += matrix[secondChar.Item1, 0];
                    }
                    else if (firstChar.Item2 == 4)
                    {
                        plain += matrix[firstChar.Item1, 0];
                        plain += matrix[secondChar.Item1, secondChar.Item2 + 1];
                    }
                    else
                    {
                        plain += matrix[firstChar.Item1, firstChar.Item2 + 1];
                        plain += matrix[secondChar.Item1, secondChar.Item2 + 1];
                    }
                }
                // same column
                else if (firstChar.Item2 == secondChar.Item2)
                {
                    if (secondChar.Item1 == 4)
                    {
                        plain += matrix[firstChar.Item1 + 1, firstChar.Item2];
                        plain += matrix[0, secondChar.Item2];
                    }
                    else if (firstChar.Item1 == 4)
                    {
                        plain += matrix[0, firstChar.Item2];
                        plain += matrix[secondChar.Item1 + 1, secondChar.Item2];
                    }
                    else
                    {
                        plain += matrix[firstChar.Item1 + 1, firstChar.Item2];
                        plain += matrix[secondChar.Item1 + 1, secondChar.Item2];
                    }
                }
                /// triangl
                else
                {
                    plain += matrix[firstChar.Item1, secondChar.Item2];
                    plain += matrix[secondChar.Item1, firstChar.Item2];
                }
            }

            return plain;
        }
        public static Tuple<int, int> pos(char c, char[,] matrix)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (matrix[i, j].Equals(c))
                        return Tuple.Create(i, j);
                }
            }
            return Tuple.Create(-1, -1);
        }
    }
}

