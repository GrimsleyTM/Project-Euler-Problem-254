using System;

namespace Euler254
{
    /// <summary>
    /// Represents a potentially extremely-large number with digit-based restrictions.
    /// The digits never increase from least-significant digit to most-significant digit.
    /// There may be any number of 9s; however, every other digit may only appear 
    /// the number of times corresponding to its own value; i.e., there may only
    /// be 5 5s and 3 3s.
    /// </summary>
    public readonly struct RDDNumber : IEquatable<RDDNumber>
    {
        public readonly ulong[] DigitCount;
        public ulong SumOfDigits {
            get
            {
                ulong sum = 0;
                for(ulong i = 1; i<(ulong)DigitCount.Length; i++)
                {
                    sum = checked( sum + DigitCount[i] * i );
                }
                return sum;
            }
        }

        /// <param name="digitCount">An array containing the count of the digit corresponding
        /// to the array's index</param>
        public RDDNumber(ulong[] digitCount)
        {
            DigitCount = digitCount ?? new ulong[10];
            DigitCount[0] = 1;
        }

        /// <summary>
        /// Returns the next largest number within the rules of the set. Assumes this RDDNumber
        /// is not 0.
        /// </summary>
        /// <returns></returns>
        public RDDNumber Next()
        {
            ulong[] nextDigits = (ulong[]) DigitCount.Clone();

            var largestDigit = LargestDigit(nextDigits, 9);

            if(largestDigit == 0)
            {
                // The current number is 0; so the next is 1
                nextDigits[1] = 1;
                return new RDDNumber(nextDigits);
            }
            
            nextDigits[largestDigit] -= 1;

            if(largestDigit != 9)
            {
                // In this case, the largest digit can simply be increased by one
                nextDigits[largestDigit+1] += 1;
                return new RDDNumber(nextDigits);
            }

            // "carry-the-one" in this weird number system
            var secondLargestDigit = LargestDigit(nextDigits, (int)largestDigit - 1);

            nextDigits[secondLargestDigit] -= 1;

            var workingDigit = secondLargestDigit+1;

            if(workingDigit == 1)
            {
                // There can only be a single 1
                nextDigits[1] += 1;
                nextDigits[2] += 1;
                workingDigit++;
            }
            else
            {
                nextDigits[workingDigit] += 2;
            }

            if(workingDigit == 9)
            {
                // The next number is all 9s
                return new RDDNumber(nextDigits);
            }

            while(nextDigits[9] > 0)
            {
                if(nextDigits[workingDigit] == workingDigit)
                    workingDigit++;

                if(workingDigit == 9)
                    break;

                nextDigits[workingDigit] += 1;
                nextDigits[9] -= 1;
            }

            return new RDDNumber(nextDigits);
        }

        private ulong LargestDigit(ulong[] digits, int maxDigit)
        {
            for(int i=maxDigit; i>=0; i--)
            {
                if(digits[i] > 0)
                    return (ulong)i;
            }
            return 0;
        }

        public bool Equals(RDDNumber other)
        {
            for(int i=1; i<DigitCount.Length; i++)
            {
                if(other.DigitCount[i] != this.DigitCount[i])
                    return false;
            }
            return true;
        }

        public override bool Equals(object other)
        {
            if(other is RDDNumber)
            {
                return Equals((RDDNumber)other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)(DigitCount[1] + DigitCount[2] + DigitCount[3] + DigitCount[4]
                + DigitCount[5] + DigitCount[6] + DigitCount[7]+ DigitCount[8]
                + DigitCount[9]);
        }

        public override string ToString()
        {
            string outputString = "";
            for(int i=1; i<9; i++)
            {
                outputString += new String(i.ToString().ToCharArray()[0], (int)DigitCount[i]);
            }
            if(DigitCount[9] <= 6 || (ulong)outputString.Length + DigitCount[9] < 40)
            {
                return outputString + new String('9', (int)DigitCount[9]);
            }
            return outputString + $"99...{DigitCount[9] - 2}x";
        }
    }
}