using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShannonEntropy.Models
{
    public class EntropyCalculator
    {
        public EntropyCalculator()
        {

        }

        public static double Calculate(params double[] EventsProbability) => Calculate(EventsProbability.ToArray());
        public static double Calculate(IEnumerable<double> EventsProbability)
        {
            double totalEntropy = 0;
            foreach (var probability in EventsProbability)
            {
                totalEntropy += (probability * Math.Log(1 / probability, 2));
            }

            return totalEntropy;
        }
    }
}
