using System;
using IrisSVM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IrisSVM.tests
{
    [TestClass]
    public class EvoAlgoTests
    {
        [TestMethod]
        public void GetBest_ShouldReturnChromosomeWithHighestFitness()
        {
            // Arrange
            var mins = new double[] { 0 };
            var maxs = new double[] { 1 };
            var pop = new[]
            {
                new Chromosome(1, mins, maxs) { Fitness = 10.5 },
                new Chromosome(1, mins, maxs) { Fitness = 50.2 }, // Cel mai bun
                new Chromosome(1, mins, maxs) { Fitness = 25.0 }
            };

            // Act
            var best = Selection.GetBest(pop);

            // Assert
            Assert.AreEqual(50.2, best.Fitness, 1e-9);
        }

        [TestMethod]
        public void Crossover_Arithmetic_ShouldRespectFormula()
        {
            // Arrange
            double[] mins = { 0 };
            double[] maxs = { 100 };

            var mother = new Chromosome(1, mins, maxs);
            mother.Genes[0] = 10;

            var father = new Chromosome(1, mins, maxs);
            father.Genes[0] = 20;

            double rate = 1.0; // Probabilitate 100% pentru crossover

            // Act
            var kid = Crossover.Arithmetic(mother, father, rate);

            // Assert
            Assert.AreEqual(10.0, kid.Genes[0], 1e-9);
        }
    }
}
