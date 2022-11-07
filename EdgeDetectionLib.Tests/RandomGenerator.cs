using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Tests
{
    public static class RandomGenerator
    {
        public static double[] GenereateDoubleNumbers(int count, double lowerBound, double upperBound)
        {
            double[] numbers = new double[count];

            for (int i = 0; i < count; i++)
            {
                numbers[i] = GetPseudoDoubleWithinRange(-lowerBound, upperBound);
            }
            return numbers;
        }

        public static int GetPseudoIntegerWithinRange(int lowerBound, int upperBound)
        {
            var random = new Random();
            random.Next(lowerBound, upperBound);
            return random.Next(lowerBound, upperBound);
        }

        public static double GetPseudoDoubleWithinRange(double lowerBound, double upperBound)
        {
            var random = new Random();
            var rDouble = random.NextDouble();
            var rRangeDouble = rDouble * (upperBound - lowerBound) + lowerBound;
            return rRangeDouble;
        }
    }
}
