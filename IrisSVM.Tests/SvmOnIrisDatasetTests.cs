using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IrisSVM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IrisProject.Tests
{
    [TestClass]
    public class SvmOnIrisDatasetTests
    {
        private static readonly CultureInfo Ci = CultureInfo.InvariantCulture;
    
        private const string IrisCsv = @"
4.8,3.4,1.6,0.2,Iris-setosa
4.8,3.0,1.4,0.1,Iris-setosa
4.3,3.0,1.1,0.1,Iris-setosa
5.8,4.0,1.2,0.2,Iris-setosa
5.7,4.4,1.5,0.4,Iris-setosa
5.4,3.9,1.3,0.4,Iris-setosa
5.1,3.5,1.4,0.3,Iris-setosa
5.7,3.8,1.7,0.3,Iris-setosa
5.1,3.8,1.5,0.3,Iris-setosa
5.4,3.4,1.7,0.2,Iris-setosa
5.1,3.7,1.5,0.4,Iris-setosa
4.6,3.6,1.0,0.2,Iris-setosa
5.1,3.3,1.7,0.5,Iris-setosa
4.8,3.4,1.9,0.2,Iris-setosa
5.0,3.0,1.6,0.2,Iris-setosa
5.0,3.4,1.6,0.4,Iris-setosa
5.2,3.5,1.5,0.2,Iris-setosa
5.2,3.4,1.4,0.2,Iris-setosa
4.7,3.2,1.6,0.2,Iris-setosa
4.8,3.1,1.6,0.2,Iris-setosa
5.4,3.4,1.5,0.4,Iris-setosa
5.2,4.1,1.5,0.1,Iris-setosa
5.5,4.2,1.4,0.2,Iris-setosa
4.9,3.1,1.5,0.1,Iris-setosa
5.0,3.2,1.2,0.2,Iris-setosa
5.5,3.5,1.3,0.2,Iris-setosa
4.9,3.1,1.5,0.1,Iris-setosa
4.4,3.0,1.3,0.2,Iris-setosa
5.1,3.4,1.5,0.2,Iris-setosa
5.0,3.5,1.3,0.3,Iris-setosa
4.5,2.3,1.3,0.3,Iris-setosa
4.4,3.2,1.3,0.2,Iris-setosa
5.0,3.5,1.6,0.6,Iris-setosa
5.1,3.8,1.9,0.4,Iris-setosa
4.8,3.0,1.4,0.3,Iris-setosa
5.1,3.8,1.6,0.2,Iris-setosa
4.6,3.2,1.4,0.2,Iris-setosa
5.3,3.7,1.5,0.2,Iris-setosa
5.0,3.3,1.4,0.2,Iris-setosa
7.0,3.2,4.7,1.4,Iris-versicolor
6.4,3.2,4.5,1.5,Iris-versicolor
6.9,3.1,4.9,1.5,Iris-versicolor
5.5,2.3,4.0,1.3,Iris-versicolor
6.5,2.8,4.6,1.5,Iris-versicolor
5.7,2.8,4.5,1.3,Iris-versicolor
6.3,3.3,4.7,1.6,Iris-versicolor
4.9,2.4,3.3,1.0,Iris-versicolor
6.6,2.9,4.6,1.3,Iris-versicolor
5.2,2.7,3.9,1.4,Iris-versicolor
5.0,2.0,3.5,1.0,Iris-versicolor
5.9,3.0,4.2,1.5,Iris-versicolor
6.0,2.2,4.0,1.0,Iris-versicolor
6.1,2.9,4.7,1.4,Iris-versicolor
5.6,2.9,3.6,1.3,Iris-versicolor
6.7,3.1,4.4,1.4,Iris-versicolor
5.6,3.0,4.5,1.5,Iris-versicolor
5.8,2.7,4.1,1.0,Iris-versicolor
6.2,2.2,4.5,1.5,Iris-versicolor
5.6,2.5,3.9,1.1,Iris-versicolor
5.9,3.2,4.8,1.8,Iris-versicolor
6.1,2.8,4.0,1.3,Iris-versicolor
6.3,2.5,4.9,1.5,Iris-versicolor
6.1,2.8,4.7,1.2,Iris-versicolor
6.4,2.9,4.3,1.3,Iris-versicolor
6.6,3.0,4.4,1.4,Iris-versicolor
6.8,2.8,4.8,1.4,Iris-versicolor
6.7,3.0,5.0,1.7,Iris-versicolor
6.0,2.9,4.5,1.5,Iris-versicolor
5.7,2.6,3.5,1.0,Iris-versicolor
5.5,2.4,3.8,1.1,Iris-versicolor
5.5,2.4,3.7,1.0,Iris-versicolor
5.8,2.7,3.9,1.2,Iris-versicolor
6.0,2.7,5.1,1.6,Iris-versicolor
5.4,3.0,4.5,1.5,Iris-versicolor
";

        private static void ParseSetosaVersicolor(out List<double[]> X, out List<int> Y)
        {
            X = new List<double[]>();
            Y = new List<int>();

            foreach (var raw in IrisCsv.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var line = raw.Trim();
                if (line.Length == 0) continue;

                var parts = line.Split(',');
                if (parts.Length != 5) continue;

                var label = parts[4].Trim();
                if (label == "Iris-setosa")
                {
                    X.Add(new[]
                    {
                        double.Parse(parts[0], Ci),
                        double.Parse(parts[1], Ci),
                        double.Parse(parts[2], Ci),
                        double.Parse(parts[3], Ci),
                    });
                    Y.Add(+1);
                }
                else if (label == "Iris-versicolor")
                {
                    X.Add(new[]
                    {
                        double.Parse(parts[0], Ci),
                        double.Parse(parts[1], Ci),
                        double.Parse(parts[2], Ci),
                        double.Parse(parts[3], Ci),
                    });
                    Y.Add(-1);
                }
            }

            Assert.IsTrue(X.Count > 0, "nu exista date de antrenare");
            Assert.AreEqual(X.Count, Y.Count);
        }

        private static double Decision(double[] w, double b, double[] x)
        {
            double f = b;
            for (int j = 0; j < x.Length; j++)
                f += w[j] * x[j];
            return f;
        }

        private static int PredictLabel(double[] w, double b, double[] x)
            => Decision(w, b, x) >= 0 ? +1 : -1;

        [TestMethod]
        public void TrainOnIrisSubset_ShouldAchieveHighTrainingAccuracy() // verifica ca alg evo poate invata pe subsetul Setosa/vertosa cu acc > 95%
        {
            ParseSetosaVersicolor(out var X, out var Y);

            double C = 1.0;
            var p = new IrisClasificationProblem(X, Y, C);
            var ea = new EvolutionaryAlgorithm();

            var best = ea.Solve(p,
                populationSize: 120,
                maxGenerations: 300,
                crossoverRate: 0.7,
                mutationRate: 0.1);

            var w = IrisClasificationProblem.ComputeW(X, Y, best);
            var b = IrisClasificationProblem.ComputeB(X, Y, best);

            // verificare w/b finite și w non-zero
            Assert.IsFalse(w.Any(v => double.IsNaN(v) || double.IsInfinity(v)), "w conține NaN/Infinity.");
            Assert.IsFalse(double.IsNaN(b) || double.IsInfinity(b), "b este NaN/Infinity.");
            var norm = Math.Sqrt(w.Sum(v => v * v));
            Assert.IsTrue(norm > 1e-6, $"||w|| prea mic: {norm}");

            int correct = 0;
            for (int i = 0; i < X.Count; i++)
                if (PredictLabel(w, b, X[i]) == Y[i]) correct++;

            double acc = (double)correct / X.Count;

            // separabilitatea intre tipurile setosa si versicolor 
            Assert.IsTrue(acc >= 0.95, $"Acuratete prea mica: {acc:P1} (corecte {correct}/{X.Count})");
        }

        [TestMethod]

        public void SmallDataset_ShouldProduceFiniteWandB() // verifica daca pe un subset mic, w si b sunt finite
        {
            var x = new List<double[]>
            {
                new double[]{5.1, 3.5, 1.4, 0.2},
                new double[]{6.0, 2.9, 4.5, 1.5}
            };

            var y = new List<int> { +1, -1 };

            var p = new IrisClasificationProblem(x, y, 1.0);
            var ea = new EvolutionaryAlgorithm();

            var best = ea.Solve(p, 10, 20, 0.7, 0.1);
            var w = IrisClasificationProblem.ComputeW(x, y, best);
            var b = IrisClasificationProblem.ComputeB(x, y, best);

            Assert.IsFalse(w.Any(double.IsNaN), "w nu tre sa aiba NaN");
            Assert.IsFalse(double.IsInfinity(b), "b sa nu fie infinit");

        }

        [TestMethod]
        public void TrainedModel_ShouldClassifyNewExamples_Correctly() // verifica ca pe un subset mic de date, vect. w si bias b sunt finite si valide
        {
            ParseSetosaVersicolor(out var X, out var Y);

            double C = 1.0;
            var p = new IrisClasificationProblem(X, Y, C);
            var ea = new EvolutionaryAlgorithm();

            var best = ea.Solve(p,
                populationSize: 120,
                maxGenerations: 300,
                crossoverRate: 0.7,
                mutationRate: 0.1);

            var w = IrisClasificationProblem.ComputeW(X, Y, best);
            var b = IrisClasificationProblem.ComputeB(X, Y, best);
            //1 setosa
            //-1 versicolor
            var newFlowers = new List<(double[] x, int y)>
            {
                (new double[]{5.1,3.5,1.4,0.2}, +1),
                (new double[]{4.8,3.4,1.6,0.2}, +1),
                (new double[]{6.0,2.9,4.5,1.5}, -1),
                (new double[]{6.4,3.2,4.5,1.5}, -1),
            };

            foreach (var (x, yTrue) in newFlowers)
            {
                int pred = PredictLabel(w, b, x);
                Assert.AreEqual(yTrue, pred, $"Predictie gresita pt [{string.Join(", ", x)}]");
            }
        }
    }
}
