using System;
using System.Collections.Generic;
using System.Linq;
using IrisSVM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IrisProject.Tests
{
    [TestClass]
    public class SvmQualityTests
    {
        private static double Decision(double[] w, double b, double[] x)
        {
            double f = b;
            for (int j = 0; j < x.Length; j++)
                f += w[j] * x[j];
            return f;
        }

        [TestMethod]
        public void AdjustAlphas_ShouldKeepBounds_AndEnforceEqualityConstraint()
        { 
            // set date noi 
            var y = new List<int> { +1, -1, +1, -1 };
            double C = 1.0;

            // Chromosome cu gene random dar rele (inclusiv negative/ >C) ca să testam corectitudinea
            var mins = new double[] { 0, 0, 0, 0 };
            var maxs = new double[] { C, C, C, C };
            var c = new Chromosome(4, mins, maxs);
            c.Genes[0] = -5;
            c.Genes[1] = 10;
            c.Genes[2] = 0.3;
            c.Genes[3] = 0.9;

            IrisClasificationProblem.AdjustAlphas(c, y, C);

            // verificare 1: genele in intervalul [0, C]
            foreach (var a in c.Genes)
            {
                Assert.IsTrue(a >= -1e-9, $"Alpha < 0: {a}");
                Assert.IsTrue(a <= C + 1e-9, $"Alpha > C: {a}");
            }

            // verificare 2: sum(alpha_i * y_i) egal aprox 0
            double sum = 0;
            for (int i = 0; i < c.Genes.Length; i++)
                sum += c.Genes[i] * y[i];

            Assert.AreEqual(0.0, sum, 1e-6, "Constrangerea sum(alpha_i * y_i)=0 nu este satisfacuta");
        }

        [TestMethod]
        public void Solve_ShouldProduceFiniteNonTrivialW_AndFiniteB()
        {
            // set mic, separabil aproape liniar (Setosa(1) si Versicolor(-1))
            var X = new List<double[]>
            {
                new double[]{5.1,3.5,1.4,0.2},
                new double[]{4.9,3.0,1.4,0.2},
                new double[]{6.0,2.9,4.5,1.5},
                new double[]{6.7,3.1,4.7,1.5},
            };
            var Y = new List<int> { +1, +1, -1, -1 };
            double C = 1.0;

            var p = new IrisClasificationProblem(X, Y, C);
            var ea = new EvolutionaryAlgorithm();

            
            var best = ea.Solve(p, populationSize: 60, maxGenerations: 120, crossoverRate: 0.7, mutationRate: 0.1);
            var w = IrisClasificationProblem.ComputeW(X, Y, best);
            var b = IrisClasificationProblem.ComputeB(X, Y, best);

            // verificare: w și b finite
            Assert.IsFalse(w.Any(v => double.IsNaN(v) || double.IsInfinity(v)), "w contine NaN/Inf");
            Assert.IsFalse(double.IsNaN(b) || double.IsInfinity(b), "b este NaN/Infinity");

            // verificare: w nu e vector aproape zero
            double norm = Math.Sqrt(w.Sum(v => v * v));
            Assert.IsTrue(norm > 1e-6, $"||w|| prea mic: {norm}");
        }

        [TestMethod]
        public void TrainedModel_ShouldClassifyTrainingPoints_WithGoodAccuracy()
        {
            var X = new List<double[]>
            {
                new double[]{5.1,3.5,1.4,0.2},
                new double[]{4.9,3.0,1.4,0.2},
                new double[]{4.7,3.2,1.3,0.2},

                new double[]{6.0,2.9,4.5,1.5},
                new double[]{6.7,3.1,4.7,1.5},
                new double[]{5.6,2.8,4.9,2.0},
            };
            var Y = new List<int> { +1, +1, +1, -1, -1, -1 };
            double C = 1.0;

            var p = new IrisClasificationProblem(X, Y, C);
            var ea = new EvolutionaryAlgorithm();

            // Act
            var best = ea.Solve(p, populationSize: 80, maxGenerations: 200, crossoverRate: 0.7, mutationRate: 0.1);
            var w = IrisClasificationProblem.ComputeW(X, Y, best);
            var b = IrisClasificationProblem.ComputeB(X, Y, best);

            int correct = 0;
            for (int i = 0; i < X.Count; i++)
            {
                double f = Decision(w, b, X[i]);
                int pred = f >= 0 ? +1 : -1;
                if (pred == Y[i]) correct++;
            }

            double acc = (double)correct / X.Count;

            // verificare: prag rezonabil
            Assert.IsTrue(acc >= 0.83, $"Acuratete slaba: {acc:P0}");
        }
    }
}
