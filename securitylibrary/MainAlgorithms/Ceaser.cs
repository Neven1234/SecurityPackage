using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        String secured = "";
        String decrypted = "";

        public string Encrypt(string plainText, int key)
        {

            foreach (char c in plainText)
            {
                int temp = numberChar(c);
                int CTIndex = temp + key;
                if (CTIndex >= 26)
                {
                    CTIndex = CTIndex % 26;
                }
                secured += NumberToChar(CTIndex);

            }
            return secured;
        }
        public int numberChar(char c)
        {
            //char c = 'b'; you may use lower case character.
            int index = char.ToUpper(c) - 65;//index == 1
            return index;
        }

        public char NumberToChar(int number)
        {
            char c = (char)(number + 97);
            return c;
        }
        public string Decrypt(string cipherText, int key)
        {

            foreach (char c in cipherText)
            {
                int temp = numberChar(c);
                int CTIndex = temp - key;
                if (CTIndex < 0)
                {
                    CTIndex = CTIndex + 26;
                }
                decrypted += NumberToChar(CTIndex);

            }
            return decrypted;
        }

        public int Analyse(string plainText, string cipherText)
        {
            int key = char.ToUpper(cipherText[0]) - char.ToUpper(plainText[0]);
            if (key < 0)
            {
                key += 26;
            }
            return key;
        }

    }
}
