using System;
using System.Collections.Generic;
using System.Linq;

namespace SA_DS
{
    class Program
    {
        static int d = 2;
        static void Main(string[] args)
        {
            
            var S = "mmiissiissiippii$".ToUpper();

            // transform string -> List<int>
            List<int> Sint = S.Select(x => Convert.ToInt32(x)).ToList();

            List<int> SA = SADS(Sint);
            Console.WriteLine("");
            
        }
        private static List<int> SADS(List<int> S)
        {
            // 1. Scan S once to classify all the characters as L - or S - type into t;
            List<bool> t = CreateLSList(S);

            // 2. Scan t once to find all the d-critical substrings in S into P1
            List<int> P1 = CreateP1(t, d);

            // 3. Bucket sort all the d-critical substrings using P1
            // 4. Name each d-critical substring in S by its bucket index to get a new shortened string S1;
            List<int> S1 = CreateS1(S, P1, t);

            // if number of buckets == number of elements
            if (S1.Count != (S1.Max() + 1))
            {
                // fire a recursive call
                List<int> SA1 = SADS(S1);
            }
            else
            {
                // directly compute SA1 from S1
                List<int> SA1 = S1;
            }

            // induce SA from SA1
            List<int> SA = new List<int>();

            return SA;
        }
        private static List<bool> CreateLSList(List<int> S)
        {
            // L/S list where S is true and L is false
            List<bool> t = new List<bool>();

            for (int i = S.Count - 1; i >= 0; i--)
            {
                if (S[i] < S[i + 1] || i == S.Count - 1) //insert S for last charachter
                {
                    t.Insert(0, true); //insert S for S[i] < S[i + 1]
                }
                else if (S[i] > S[i + 1])
                {
                    t.Insert(0, false); //insert L for S[i] > S[i + 1]
                }
                else
                {
                    t.Insert(0, t[0]); //repeat value if S[i] == S[i + 1]
                }
            }

            return t;
        }
        private static List<int> CreateP1(List<bool> t, int d)
        {
            // creates P1 array of pointers to d-critical chars
            // RULES:
            // - S[i] is d-critical only if it is a LMS or if  S[i-d] is d-critical and no char in between is critical
            // - S[i] is LMS if S[i] is S type (true) and S[i-1] is L type (false)
            // - first char is not critical, last char is critical
            // - if S[i] is critical, S[i-1] and S[i+1] are not
            // - d>=2, in this implementation d=2

            List<int> P1 = new List<int>();

            for (int i = t.Count-1; i > 0; i--)
            {
                if (t[i] == true||(P1[0]==i+d && t[i-1]!=true))
                // if S[i] is critical or the last added d-critical char was S[i+d] (we compare indices)
                {
                    P1.Insert(0, i);
                }
            }
            return P1;
        }
        private static List<int> CreateS1(List<int> S,List<int> P1, List<bool> t)
        {
            S.Add(36); S.Add(36); S.Add(36);
            t.Add(false); t.Add(false); t.Add(false);

            List<List<int>> valueLists = new List<List<int>>();

            // for each index in P1 we're creating an int list element_in_p1|list_of_elements_in_subarray|placeholder_for_bucket_number
            foreach(int p1element in P1)
            {
                valueLists.Add(new List<int>() {p1element});
                for (int i = 0; i < d + 2; i++)
                {
                    valueLists.Last().Add(2*S[p1element+i]+Convert.ToInt32(t[p1element + i]));
                }
                valueLists.Last().Add(0);
            }

            // this works only for d=2
            valueLists = valueLists.OrderBy(l => l[1]).ThenBy(l => l[2]).ThenBy(l => l[3]).ThenBy(l => l[4]).ToList(); 

            for (int i = 0; i < P1.Count; i++)
            {
                P1[i] = valueLists[i][0];
            }

            int bucketNumber = 0;
            valueLists[0][d + 2+1] = bucketNumber++;

            for(int i = 1; i < valueLists.Count; i++)
            {
                if(substringsEqual(d, valueLists[i - 1], valueLists[i]))
                {
                    bucketNumber--;
                }

                valueLists[i][d + 2+1] = bucketNumber;
                bucketNumber++;
            }

            valueLists = valueLists.OrderBy(e => e[0]).ToList();

            return valueLists.Select(l => l[d + 2 + 1]).ToList();
        }

        // why are we skipping the first element
        private static bool substringsEqual(int d,List<int> list1, List<int> list2)
            // returns true if the substrings of S (d+2 elements) are equal
        {
            for(int i = 0; i < d + 2; i++)
            {
                if(list1[1+i] != list2[1 + i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}