using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        public static int ModularPower(int baseVal, int expVal, int modVal)
        {
            int initialVal = 1;
            int i = 0;
            int flag = 0;
            baseVal = baseVal % modVal;

            if (baseVal == 0)
                return 0; // In case x is divisible by p;

            if (expVal % 2 == 0)//even
            {
                expVal = expVal / 2;
                flag = 1;
            }
            while (i < expVal)
            {

                initialVal = (initialVal * baseVal) % modVal;

                i++;
            }
            if (flag == 1)
            {
                initialVal = (initialVal * initialVal) % modVal;
            }


            return initialVal;
        }

        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {   // q = mod
            int K_ = ModularPower(y, k, q);
            int c1 = ModularPower(alpha, k, q);
            int c2 = (K_ * m) % q;

            List<long> C = new List<long>();
            C.Add((long)c1);
            C.Add((long)c2);
            return C;
            //throw new NotImplementedException();
        }

        int calculateMultiplicativeInverse(int baseVal, int mod)
        {
            double c = 0;
            double x = 1;
            int b;
            while (true)
            {
                if (Math.Ceiling(x / (mod - baseVal)) == Math.Floor(x / (mod - baseVal)))//integer
                {
                    c = (x / (mod - baseVal));
                    break;
                }
                x += mod;
            }
            b = Convert.ToInt32(mod - c);
            return b;
        }
        public int Decrypt(int c1, int c2, int x, int q)
        {
            int k = ModularPower(c1, x, q);
            int k_1 = calculateMultiplicativeInverse(k, q);
            int m = (c2 * k_1) % q;
            return m;
            //throw new NotImplementedException();

        }
    }
}
