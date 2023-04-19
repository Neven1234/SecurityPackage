using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {
     
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
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            Random rnd = new Random();
            long k1 = 1;
            long k2=1;
            //int xa_= rnd.Next(1, xa);
            //int xb_ = rnd.Next(1, xb);

            int ya = ModularPower(alpha,xa, q);
            int yb= ModularPower(alpha , xb, q);
            k1 = ModularPower(yb , xa,q) ;
            k2= ModularPower(ya , xb, q);
            return new List<int>() { (int)k1, (int)k2 };


        }
    }
}
