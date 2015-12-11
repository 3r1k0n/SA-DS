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
            var S = "attagcgagcg$";
            S = S.ToUpper();

            // 1. Scan S once to classify all the characters as L - or S - type into t;
            List<bool> t = CreateLSList(S);
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
    }
}
