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
                t1 = a1 - q * b1;     t2 = a2 - q * b2;    t3 = a3 - q * b3;

                a1 = b1;    a2 = b2;    a3 = b3;

                b1 = t1;    b2 = t2;    b3 = t3;
            }
        }

        private int pow_mod(int M, int c, int  e, int n)
        {
            if(e == 1)
            {
                return c % n;
            }
            e--;
            c = (c * M) % n;
            
            return pow_mod(M, c, e, n);
        }

        public int Encrypt(int p, int q, int M, int e)
        {
            int n = q * p;
            int q_n = (q - 1) * (p - 1);
            if(e > q_n || GCD(e, q_n) != 1)
            {
                return -1;
            }
            int re = M % n;
            re = pow_mod(M, re, e, n);
            return re;
            //throw new NotImplementedException();
        }

        int calculateMultiplicativeInverse(int det, int mod)
        {
            double c = 0;
            double x = 1;
            int b;
            while (true)
            {
                if (Math.Ceiling(x / (mod - det)) == Math.Floor(x / (mod - det)))//integer
                {
                    c = (x / (mod - det));
                    break;
                }
                x += mod;
            }
            b = Convert.ToInt32(mod - c);
            return b;
        }


        public int Decrypt(int p, int q, int C, int e)
        {
            int n = q * p;
            int q_n = (q - 1) * (p - 1);
            int d = calculateMultiplicativeInverse(e, q_n);
            if (e > q_n || GCD(e, q_n) != 1)
            {
                return -1;
            }
            int re = C % n;
            re = pow_mod(C, re, d, n);
            return re;
            //throw new NotImplementedException();
        }
    }
}
