using System;
using System.Collections.Generic;
using System.Text;

namespace ShannonEntropy.Models
{
    public class EntropyCalculator
    {
        public EntropyCalculator()
        {

        }

        public static double Calculate(params double[] EventsProbability)
        {
            double totalEntropy = 0;
            int logbase = EventsProbability.Length;
            foreach (var probability in EventsProbability)
            {
                totalEntropy += (probability * Math.Log(1 / probability, logbase));
            }

            return totalEntropy;
        }
    }
}
