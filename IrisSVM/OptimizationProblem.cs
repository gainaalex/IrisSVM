using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisSVM
{
    public interface IOptimizationProblem
    {
        void ComputeFitness(Chromosome c);

        Chromosome MakeChromosome();
    }

    public class IrisClasificationProblem : IOptimizationProblem
    {
        private List<double[]> X; // datele
        private List<int> Y;   // etichetele (lista de elem 1/-1)
        private double C;     // parametrul SVM

        public IrisClasificationProblem(List<double[]> X, List<int> Y, double C)
        {
            this.X = X;
            this.Y = Y;
            this.C = C;
        }

        public Chromosome MakeChromosome()
        {
            int N = X.Count;
            double[] mins = Enumerable.Repeat(0.0, N).ToArray();
            double[] maxs = Enumerable.Repeat(C, N).ToArray();
            return new Chromosome(N, mins, maxs);
        }

        public void ComputeFitness(Chromosome c)
        {
            // Ajustare α înainte de calcul fitness
            AdjustAlphas(c, Y, C);

            // Formula duală SVM
            double sumAlpha = c.Genes.Sum();
            double sumKernel = 0;
            int N = X.Count;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    sumKernel += 0.5 * c.Genes[i] * c.Genes[j] * Y[i] * Y[j] * LinearKernel(X[i], X[j]);
                }
            }
            c.Fitness = sumAlpha - sumKernel; // maximizăm
        }

        public static void AdjustAlphas(Chromosome c, List<int> Y, double C)
        {
            int N = c.NoGenes;

            // 1. Trunchiere [0, C]
            for (int i = 0; i < N; i++)
            {
                if (c.Genes[i] < 0) c.Genes[i] = 0;
                if (c.Genes[i] > C) c.Genes[i] = C;
            }

            // 2. Ajustare suma de αi*yi = 0
            double sum = 0;
            for (int i = 0; i < N; i++)
                sum += c.Genes[i] * Y[i];

            for (int i = 0; i < N; i++)
                c.Genes[i] -= sum / N * Y[i];
        }

        //wi zice cat de importanta e caracteristica i din x in deciderea categoriei din care face parte subiectul
        public static double[] ComputeW(List<double[]> X, List<int> Y, Chromosome c)
        {
            int dim = X[0].Length;
            double[] w = new double[dim];
            for (int i = 0; i < X.Count; i++)
                for (int j = 0; j < dim; j++)
                    w[j] += c.Genes[i] * Y[i] * X[i][j]; // un subient versicolor va trage in jos iar un subiect setosa va trage in sus indicele
            return w;
        }

        //un fel de offset
        public static double ComputeB(List<double[]> X, List<int> Y, Chromosome c)
        {
            int N = X.Count;
            for (int i = 0; i < N; i++)
            {
                if (c.Genes[i] > 1e-5)
                {
                    double sum = 0;
                    for (int j = 0; j < N; j++)
                        sum += c.Genes[j] * Y[j] * LinearKernel(X[j], X[i]);
                    return Y[i] - sum;
                }
            }
            return 0.0;
        }

        //exemplu de determinare a clasei unui nou subiect bazat pe modelul antrenat:
        //double f = 0;
        //for (int j = 0; j<x.Length; j++)
        //  f += w[j]* x[j];
        //f += b;
        //int predictedLabel = f >= 0 ? +1 : -1;


        private static double LinearKernel(double[] xi, double[] xj)
        {
            double sum = 0;
            for (int k = 0; k < xi.Length; k++)
                sum += xi[k] * xj[k];
            return sum;
        }
    }


}
