﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        string[,] SBOX = new string[16, 16] { { "63", "7C", "77", "7B", "F2", "6B", "6F", "C5", "30", "01", "67", "2B", "FE", "D7", "AB", "76" },
                                                  { "CA", "82", "C9", "7D", "FA", "59", "47", "F0", "AD", "D4", "A2", "AF", "9C", "A4", "72", "C0" },
                                                  { "B7", "FD", "93", "26", "36", "3F", "F7", "CC", "34", "A5", "E5", "F1", "71", "D8", "31", "15" },
                                                  { "04", "C7", "23", "C3", "18", "96", "05", "9A", "07", "12", "80", "E2", "EB", "27", "B2", "75" },
                                                  { "09", "83", "2C", "1A", "1B", "6E", "5A", "A0", "52", "3B", "D6", "B3", "29", "E3", "2F", "84" },
                                                  { "53", "D1", "00", "ED", "20", "FC", "B1", "5B", "6A", "CB", "BE", "39", "4A", "4C", "58", "CF" },
                                                  { "D0", "EF", "AA", "FB", "43", "4D", "33", "85", "45", "F9", "02", "7F", "50", "3C", "9F", "A8" },
                                                  { "51", "A3", "40", "8F", "92", "9D", "38", "F5", "BC", "B6", "DA", "21", "10", "FF", "F3", "D2" },
                                                  { "CD", "0C", "13", "EC", "5F", "97", "44", "17", "C4", "A7", "7E", "3D", "64", "5D", "19", "73" },
                                                  { "60", "81", "4F", "DC", "22", "2A", "90", "88", "46", "EE", "B8", "14", "DE", "5E", "0B", "DB" },
                                                  { "E0", "32", "3A", "0A", "49", "06", "24", "5C", "C2", "D3", "AC", "62", "91", "95", "E4", "79" },
                                                  { "E7", "C8", "37", "6D", "8D", "D5", "4E", "A9", "6C", "56", "F4", "EA", "65", "7A", "AE", "08" },
                                                  { "BA", "78", "25", "2E", "1C", "A6", "B4", "C6", "E8", "DD", "74", "1F", "4B", "BD", "8B", "8A" },
                                                  { "70", "3E", "B5", "66", "48", "03", "F6", "0E", "61", "35", "57", "B9", "86", "C1", "1D", "9E" },
                                                  { "E1", "F8", "98", "11", "69", "D9", "8E", "94", "9B", "1E", "87", "E9", "CE", "55", "28", "DF" },
                                                  { "8C", "A1", "89", "0D", "BF", "E6", "42", "68", "41", "99", "2D", "0F", "B0", "54", "BB", "16" } };


        string[,] mixColumnMatrix = new string[4, 4] { {"02", "03", "01", "01"},
                                                       {"01", "02", "03", "01"},
                                                       {"01", "01", "02", "03"},
                                                       {"03", "01", "01", "02"}};

        //i'm just testing something by this matix, i'm gonna delete it later
        string[,] test = new string[4, 4] { {"87", "f2", "4d", "97"},
                                            {"6e", "02", "03", "01"},
                                            {"46", "01", "02", "03"},
                                            {"a6", "01", "01", "02"}};

        private string[,] mixColumns(string[,] matrix)
        {
            string[,] mixedMatrix = new string[4, 4];
            int res = 0;
            int xor;
            string hexa;
            for (int i = 0; i < 4; i++) //column in plane
            {
                for (int j = 0; j < 4; j++) //row in mixer
                {
                    for (int k = 0; k < 4; k++) // index of element in column plane and row mixed
                    {
                        hexa = matrix[k, i];
                        if (Convert.ToInt32(mixColumnMatrix[j, k], 16) >= 2)
                        {
                            int shifted = Convert.ToInt32(matrix[k, i], 16) << 1; //270
                            hexa = shifted.ToString("X");//10e
                            if (hexa[0] == '1' && hexa.Length >= 3)
                            {
                                hexa = hexa.Remove(0, 1);
                                xor = Convert.ToInt32(hexa, 16) ^ Convert.ToInt32("1b", 16); //21(xor)
                                hexa = xor.ToString("X");//15
                            }
                            if (Convert.ToInt32(mixColumnMatrix[j, k], 16) == 3)
                            {
                                xor = Convert.ToInt32(hexa, 16) ^ Convert.ToInt32(matrix[k, i], 16);
                                hexa = xor.ToString("X");
                            }
                        }
                        if (res == 0)
                        {
                            res = Convert.ToInt32(hexa, 16);
                        }
                        else
                        {
                            res = res ^ Convert.ToInt32(hexa, 16);
                        }

                    }
                    mixedMatrix[i, j] = res.ToString("X");
                    res = 0;

                }
            }
            return mixedMatrix;
        }


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
                plainText_Matrix = ShiftRow(plainText_Matrix, "enc");
                //maxx col
                //round key
                plainText_Matrix = XOR(plainText_Matrix, key_Matrix);

                i++;
            }
            plainText_Matrix = SubBytes(plainText_Matrix);
            plainText_Matrix = ShiftRow(plainText_Matrix, "enc");
            //round key
            plainText_Matrix = XOR(plainText_Matrix, key_Matrix);
            return plainText_Matrix.ToString();

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
        public string[,] SubBytes(string[,] str)
        {
            string[,] Matrix = new string[4, 4];
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)

                {
                    int row = Convert.ToInt32(str[i, j][0].ToString(), 16);
                    int col = Convert.ToInt32(str[i, j][1].ToString(), 16);
                    Matrix[i,j]= SBOX[row, col];
                }
            }
            return Matrix;

        }
        public string[,] ShiftRow(string[,] str,string flag)
        {
            string[,] Matrix = new string[4, 4];
            int[] arr = { 0, Matrix.GetLength(0)};
            int counter=0;
            if (flag == "enc")
            {
                 counter = arr[0];

            }
            else if(flag=="dec")
            {
                 counter = arr[1];
            }
            for(int i=0;i<Matrix.GetLength(0);i++)
            {
                for (int j = 0; j < Matrix.GetLength(1);j++)
                {
                    if(flag=="enc")
                    {
                        Matrix[i, j] = str[i, (j + counter) % 4];

                    }
                    if (flag=="dec")
                    {
                        Matrix[i, j] = str[i, (j + counter) % 4];

                    }
                }
                if (flag == "enc")
                {
                    counter++;

                }
                if (flag == "dec")
                {
                    counter--;
                }
            }
                      return Matrix;

        }
    }
}

