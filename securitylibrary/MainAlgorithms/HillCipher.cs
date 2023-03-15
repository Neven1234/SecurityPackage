using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {

        bool isListEqual(List<int> l1, List<int> l2)
        {
            for (int i = 0; i < l1.Count; i++)
            {
                if (l1[i] != l2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            List<int> testCipher = Encrypt(plainText, new List<int> { l, k, i, j });
                            if (isListEqual(testCipher, cipherText))
                            {
                                return new List<int> { l, k, i, j };
                            }
                        }
                    }
                }
            }
            throw new InvalidAnlysisException();
        }


        private int mod(int x1, int x2)
        {
            if (x1 < 0)
                return ((x1 % x2) + x2) % x2;
            return x1 % x2;
        }

        int[,] createIndices()
        {
            int[,] mapIndices = new int[3, 3];

            int realIndex = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mapIndices[i, j] = realIndex;
                    realIndex++;
                }
            }
            return mapIndices;
        }

        int calculateDet(int[,] mapIndices, List<int> key)
        {
            int det = 0;
            for (int i = 0; i < 3; i++)
                det += (key[mapIndices[0, i]] *
                                (
                                key[mapIndices[1, (i + 1) % 3]] * key[mapIndices[2, (i + 2) % 3]] -
                                key[mapIndices[1, (i + 2) % 3]] * key[mapIndices[2, (i + 1) % 3]]
                                )
                            );
            det = mod(det, 26);
            if (GCD(det, 26) != 1 || det == 0)
            {
                throw new NotImplementedException();
            }
            return det;
        }

        int calculateMultiplicativeInverse(int det)
        {
            double c = 0;
            double x = 1;
            int b;
            while (true)
            {
                if (Math.Ceiling(x / (26 - det)) == Math.Floor(x / (26 - det)))//integer
                {
                    c = (x / (26 - det));
                    break;
                }
                /*b = Convert.ToInt32(26 - c);
                if (b >= 26)
                {
                    throw new NotImplementedException();
                }*/
                x += 26;
            }
            b = Convert.ToInt32(26 - c);
            return b;
        }

        public static int GCD(int p, int q)
        {
            if (q == 0)
            {
                return p;
            }

            int r = p % q;

            return GCD(q, r);
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            for (int i = 0; i < key.Count; i++)
            {
                if (key[i] < 0 || key[i] >= 26)
                {
                    throw new NotImplementedException();
                }
            }

            if (key.Count == 4)
            {
                List<int> inverseKey1 = new List<int>();
                int A = key[0] * key[3] - key[1] * key[2];//det
                if (Math.Abs(GCD(A, 26)) != 1 || A == 0)
                {
                    throw new NotImplementedException();
                }
                inverseKey1.Add((1 / A) * key[3]);
                inverseKey1.Add((1 / A) * key[1] * -1);
                inverseKey1.Add((1 / A) * key[2] * -1);
                inverseKey1.Add((1 / A) * key[0]);
                List<int> plain1 = Encrypt(cipherText, inverseKey1);
                return plain1;
            }

            int[,] mapIndices = createIndices();
            int det = calculateDet(mapIndices, key);
            int b = calculateMultiplicativeInverse(det);

            List<int> inverseKey = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int temp = key[mapIndices[(i + 1) % 3, (j + 1) % 3]] * key[mapIndices[(i + 2) % 3, (j + 2) % 3]] -
                               key[mapIndices[(i + 1) % 3, (j + 2) % 3]] * key[mapIndices[(i + 2) % 3, (j + 1) % 3]];
                    if ((i == 1 || j == 1) && i != j)
                    {
                        temp = temp * -1;
                    }
                    temp = temp * b * Convert.ToInt32(Math.Pow(-1, i + j));
                    temp = mod(temp, 26);
                    inverseKey.Add(temp);
                }
            }

            List<int> transposedKey = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    transposedKey.Add(inverseKey[mapIndices[j, i]]);
                }
            }

            List<int> plain = Encrypt(cipherText, transposedKey);
            return plain;
        }

        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> cipher = new List<int>();
            int res = 0;
            for (int i = 0; i < plainText.Count; i += Convert.ToInt32(Math.Sqrt(key.Count)))
            {
                for (int j = 0; j < key.Count; j += Convert.ToInt32(Math.Sqrt(key.Count)))
                {
                    for (int k = 0; k < Math.Sqrt(key.Count); k++)
                    {
                        res += key[j + k] * plainText[i + k];
                    }
                    res = mod(res, 26);
                    cipher.Add(res);
                    res = 0;
                }
            }
            return cipher;
        }

        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            int[,] mapIndices = createIndices();
            int det = calculateDet(mapIndices, plainText);
            int b = calculateMultiplicativeInverse(det);

            List<int> inversePlane = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int temp = plainText[mapIndices[(i + 1) % 3, (j + 1) % 3]] * plainText[mapIndices[(i + 2) % 3, (j + 2) % 3]] -
                               plainText[mapIndices[(i + 1) % 3, (j + 2) % 3]] * plainText[mapIndices[(i + 2) % 3, (j + 1) % 3]];
                    if ((i == 1 || j == 1) && i != j)
                    {
                        temp = temp * -1;
                    }
                    temp = temp * b * Convert.ToInt32(Math.Pow(-1, i + j));
                    temp = mod(temp, 26);
                    inversePlane.Add(temp);
                }
            }

            List<int> transposedPlane = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    transposedPlane.Add(inversePlane[mapIndices[j, i]]);
                }
            }

            List<int> outkey = new List<int>();
            int res = 0;
            for (int i = 0; i < cipherText.Count / 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        res += (transposedPlane[mapIndices[i, j]] * cipherText[mapIndices[j, k]]);
                    }
                    outkey.Add(mod(res, 26));
                    res = 0;
                }
            }

            List<int> transposedOutkey = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    transposedOutkey.Add(outkey[mapIndices[j, i]]);
                }
            }
            return transposedOutkey;
        }
    }
}
