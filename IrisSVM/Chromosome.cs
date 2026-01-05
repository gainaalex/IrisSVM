using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisSVM
{
    public class Chromosome
    {
        public int NoGenes { get; set; } // nr gene individ

        public double[] Genes { get; set; } // val gene

        public double[] MinValues { get; set; } // val min posibile ale gene

        public double[] MaxValues { get; set; } // val max posibile ale gene

        public double Fitness { get; set; } // val fct de adaptare individ

        private static Random _rand = new Random();

        public Chromosome(int noGenes, double[] minValues, double[] maxValues)
        {
            NoGenes = noGenes;
            Genes = new double[noGenes];
            MinValues = new double[noGenes];
            MaxValues = new double[noGenes];

            for (int i = 0; i < noGenes; i++)
            {
                MinValues[i] = minValues[i];
                MaxValues[i] = maxValues[i];

                Genes[i] = minValues[i] + _rand.NextDouble() * (maxValues[i] - minValues[i]); // init gene random
            }
        }

        public Chromosome(Chromosome c) // constructor de copiere
        {
            NoGenes = c.NoGenes;
            Fitness = c.Fitness;

            Genes = new double[c.NoGenes];
            MinValues = new double[c.NoGenes];
            MaxValues = new double[c.NoGenes];

            for (int i = 0; i < c.Genes.Length; i++)
            {
                Genes[i] = c.Genes[i];
                MinValues[i] = c.MinValues[i];
                MaxValues[i] = c.MaxValues[i];
            }
        }
    }
}
