using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        private int GCD(int p, int q)
        {
            if (q == 0) return p;
            int r = p % q;
            return GCD(q, r);
        }

        private int euclideanAlgorithm(int b, int m)
        {
            int q, a1, a2, a3, b1, b2, b3, t1, t2, t3;
            a1 = 1;  a2 = 0; a3 = m;
            b1 = 0;  b2 = 1; b3 = b;
            while (true)
            {
                if (b3 == 0)
                {
                    return -1;
                }
                else if(b3 == 1)
                {
                    return b2;
                }
                q = a3 / b3;
                t1 = a1 - q * b1;   t2 = a2 - q * b2;  t3 = a3 - q * b3;

                a1 = b1;  a2 = b2;  a3 = b3;

                b1 = t1;  b2 = t2; b3 = t3;
            }
        }
        private int mod(int x1, int x2)
        {
            if (x1 < 0)
                return ((x1 % x2) + x2) % x2;
            return x1 % x2;
        }

        public int Encrypt(int p, int q, int M, int e)
        {
            int n = q * p;
            int q_n = (q - 1) * (p - 1);
            if(e > q_n || GCD(e, q_n) != 1)
            {
                return -1;
            }
            double c = Math.Pow(M, e) % n;
            return (int) c;
            //throw new NotImplementedException();
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            //int d = euclideanAlgorithm(e, q_n);

            throw new NotImplementedException();
        }
    }
}
