using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA_DS
{
    class Program
    {
        static void Main(string[] args)
        {
            var S = "mmiissiissiippii$";
            S = S.ToUpper();

            // 1. Scan S once to classify all the characters as L - or S - type into t;
            List<bool> t = CreateLSList(S);
            // 2. Scan t once to find all the d-critical substringsin S into P1
            List<int> P1 = CreateP1(t);
        }

        private static List<bool> CreateLSList(string S)
        {
            // L/S list where S is true and L is false
            List<bool> t = new List<bool>();

            for (int i = S.Length - 1; i >= 0; i--)
            {
                if (S[i] == '$')
                {
                    t.Insert(0, true); //insert S for $
                }
                else
                {
                    if (S[i] > S[i + 1])
                    {
                        t.Insert(0, false); //insert L for S[i] > S[i + 1]
                    }
                    else if (S[i] < S[i + 1])
                    {
                        t.Insert(0, true); //insert S for S[i] < S[i + 1]
                    }
                    else
                    {
                        t.Insert(0, t[0]); //copy value if S[i] == S[i + 1]
                    }
                }
            }

            return t;
        }
        private static List<int> CreateP1(List<bool> t)
        {
            // creates P1 array of pointers to d-critical chars
            // RULES:
            // - S[i] is d-critical only if it is a LMS or if  S[i-d] is d-critical and no char in between is critical
            // - S[i] is LMS if S[i] is S type (true) and S[i-1] is L type (false)
            // - first char is not critical, last char is critical
            // - if S[i] is critical, S[i-1] and S[i+1] are not
            // - d>=2, in this implementation d=2

            List<int> P1 = new List<int>();
            int d = 2;

            for (int i = 1; i < t.Count; i++)
            {
                if ((t[i] == true && t[i - 1] == false) || P1.DefaultIfEmpty(-d).Last()==i-d)
                    // if S[i] is critical or the last added d-critical char was S[i-d] (we compare indices)
                {
                    P1.Add(i);
                }
            }
            return P1;
        }
    }
}
