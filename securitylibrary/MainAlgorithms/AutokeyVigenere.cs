using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //dictionary map from alphabet to numbers
            int count = 0;
            Dictionary<String, int> lettersToNumbers = new Dictionary<String, int>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                lettersToNumbers.Add(c.ToString(), count);
                count++;
            }

            //dictionary map from number to alphabets
            count = 0;
            Dictionary<int, String> numbersToLetters = new Dictionary<int, String>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                numbersToLetters.Add(count, c.ToString());
                count++;
            }
            // analysis process 
            List<String> key = new List<String>();
            int key_temp;
            cipherText = cipherText.ToLower();

            for (int i = 0; i < cipherText.Length; i++)
            {
                key_temp = (lettersToNumbers[cipherText[i].ToString()] - lettersToNumbers[plainText[i].ToString()] + 26) % 26;
                key.Add(numbersToLetters[key_temp].ToString());
            }

            //geting key from repeated key
            int stop_idx = -1;
            int j = 0;

            for (int i = 1; i < key.Count(); i++)
            {
                if (key[i] == plainText[j].ToString())
                {
                    if (stop_idx == -1)
                    {
                        stop_idx = i;
                    }
                    j++;
                }
                else
                {
                    stop_idx = -1;
                    j = 0;
                }
                if (j == stop_idx)
                {
                    break;
                }
            }

            List<String> single_key = new List<String>();
            for (int i = 0; i < stop_idx; i++)
            {
                single_key.Add(key[i]);
            }

            string K = string.Join("", single_key).ToLower();
            return K;

            throw new NotImplementedException();

        }

        public string Decrypt(string cipherText, string key)
        {
            //dictionary map from alphabet to numbers
            int count = 0;
            Dictionary<String, int> lettersToNumbers = new Dictionary<String, int>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                lettersToNumbers.Add(c.ToString(), count);
                count++;
            }

            //dictionary map from number to alphabets
            count = 0;
            Dictionary<int, String> numbersToLetters = new Dictionary<int, String>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                numbersToLetters.Add(count, c.ToString());
                count++;
            }

            List<String> plainText = new List<String>();

            // encrypt process
            int text_temp;
            int j = 0; //index of key
            int s_key = key.Length;
            cipherText = cipherText.ToLower();


            for (int i = 0; i < cipherText.Length; i++)
            {
                text_temp = (lettersToNumbers[cipherText[i].ToString()] - lettersToNumbers[key[j].ToString()] + 26) % 26;
                plainText.Add(numbersToLetters[text_temp].ToString());
                key = key + numbersToLetters[text_temp].ToString();
                j++;
            }


            string text = string.Join("", plainText).ToUpper();
            return text;


            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {

            //dictionary map from alphabet to numbers
            int count = 0;
            Dictionary<String, int> lettersToNumbers = new Dictionary<String, int>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                lettersToNumbers.Add(c.ToString(), count);
                count++;
            }

            //dictionary map from number to alphabets
            count = 0;
            Dictionary<int, String> numbersToLetters = new Dictionary<int, String>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                numbersToLetters.Add(count, c.ToString());
                count++;
            }

            List<String> cipherText = new List<String>();

            // encrypt process 
            key = key + plainText;
            int cipher_temp;
            int j = 0; //index of key
            int s_key = key.Length;

            for (int i = 0; i < plainText.Length; i++)
            {
                cipher_temp = (lettersToNumbers[plainText[i].ToString()] + lettersToNumbers[key[i].ToString()]) % 26;
                cipherText.Add(numbersToLetters[cipher_temp]);
                j++;
            }
            string cipher = string.Join("", cipherText).ToUpper();
            return cipher;
        }
    }
}
