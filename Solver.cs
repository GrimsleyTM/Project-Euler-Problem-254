using System;
using System.Collections.Generic;

namespace Euler254
{
    public class Solver
    {
        const ulong largestKeyInLookup = 62;
        public Dictionary<ulong, RDDNumber> gLookup = new Dictionary<ulong, RDDNumber>();
        
        public Solver()
        {
            PopulateGLookupArray();
        }

        /// <summary>
        /// Calculates the solution for Euler Project 254: The sum of sg(i) for i=[1,n].
        /// </summary>
        /// <param name="n">Upper limit of the series</param>
        /// <returns></returns>
        public ulong Euler254(ulong n)
        {
            ulong sum = 0;
            for(ulong x=1; x<=n; x++)
            {
                if(gLookup.ContainsKey(x))
                {
                    sum = checked(sum + gLookup[x].SumOfDigits);
                }
                else
                {
                    var temp = i_To_n((int)x).SumOfDigits;
                    sum = checked(sum + temp);
                    Console.WriteLine($"i:{x}, sg:{temp}");
                }
            }
            return sum;
        }

        /// <summary>
        /// Calculates the solution for the HackerRank variation of Euler Project 254:
        /// Modulo m of the sum of sg(i) for i=[1,n].
        /// </summary>
        /// <param name="n">Upper limit of the series</param>
        /// <param name="m">The divisor for the modulo</param>
        /// <returns></returns>
        public ulong Euler254(ulong n, ulong m)
        {
            ulong sum = 0;
            for(ulong x=1; x<=n; x++)
            {
                if(gLookup.ContainsKey(x))
                {
                    sum = checked(sum + gLookup[x].SumOfDigits);
                }
                else
                {
                    sum = checked(sum + i_To_n((int)x).SumOfDigits);
                }
                sum %= m;
            }
            return sum;
        }

        private ulong f(RDDNumber n)
        {
            ulong[] factorialLookup = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880 };
            ulong sum = 0;
            for(int i=1; i<9; i++)
            {
                sum += factorialLookup[i] * (ulong)n.DigitCount[i];
            }
            sum = checked(sum + factorialLookup[9] * (ulong)n.DigitCount[9]);
            return sum;
        }
        
        private ulong SumOfDigits(ulong number)
        {
            ulong sum = 0;
            while(number >= 1)
            {
                sum += number%10;
                number /= 10;
            }
            return sum;
        }

        private ulong sf(RDDNumber n)
        {
            ulong fOfn = f(n);
            return SumOfDigits(fOfn);
        }

        private ulong i_To_fOfn(int i)
        {
            if(i > 163)
            {
                throw new InvalidOperationException("i cannot be greater than 163. i=" + i.ToString());
            }

            ulong result = 0;

            for(int y=0; y<i/9; y++)
            {
                result += 9 * (ulong)System.Math.Pow(10, y);
            }

            result += (ulong)i%9 * (ulong)System.Math.Pow(10, i/9);
            return result;
        }

        private RDDNumber fOfn_To_n(ulong fOfn)
        {
            ulong[] divisors = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880 };

            var digitCount = new ulong[divisors.Length];

            for(int i=divisors.Length-1; i>0; i--) // i = [9,1]
            {
                digitCount[i] = fOfn / divisors[i];
                fOfn -= digitCount[i] * divisors[i];
                if(i==9)
                {
                    Console.WriteLine($"/9! Result: {digitCount[i]}");
                    Console.WriteLine($"/9! Remainder: {fOfn}");
                }
            }

            return new RDDNumber(digitCount);
        }

        private RDDNumber i_To_n(int i)
        {
            return fOfn_To_n(i_To_fOfn(i));
        }
        
        private void PopulateGLookupArray()
        {
            var nextGLookupInput = new RDDNumber(new ulong[] {0,1,0,0,0,0,0,0,0,0});
            while(true)
            {
                ulong i = sf(nextGLookupInput);
                if(!gLookup.ContainsKey(i))
                {
                    gLookup.Add(i, nextGLookupInput);
                    if(i == largestKeyInLookup)
                        break;
                }
                nextGLookupInput = nextGLookupInput.Next();
            }
        }
    }
}

