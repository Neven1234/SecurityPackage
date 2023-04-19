using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int rule(int q,int a,int b)
        {
            int res = a - (q * b);
            return res;
        }
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            //throw new NotImplementedException();
            int r1 = baseN;
            int r2 = number;
            int t1 = 0;
            int t2 = 1;
            while(true)
            {
                if(r2 ==0||r2==1) { break; }
                
                int q = r1 / r2;
                int r = rule(q, r1, r2);
                int t= rule(q,t1, t2);
                r1 = r2;
                r2 = r;
                t1 = t2;
                t2 = t;

              
            }
           
          if(r2==1)
          {
                if (t2 < 0)//-ve // <-1
                {
                    return t2 + baseN;
                }
                else
                    return t2;
          }
              
            return -1;
        }
    }
}
