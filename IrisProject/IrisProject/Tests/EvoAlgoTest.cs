using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using IrisSVM;

namespace IrisSVM.tests
{
    public class EvoAlgoTests
    {
        [Fact]
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
            Assert.Equal(50.2, best.Fitness);
        }

        [Fact]
        public void Crossover_Arithmetic_ShouldRespectFormula()
        {
            // Arrange
            double[] mins = { 0 }; double[] maxs = { 100 };
            var mother = new Chromosome(1, mins, maxs);
            mother.Genes[0] = 10;
            var father = new Chromosome(1, mins, maxs);
            father.Genes[0] = 20;

            double rate = 1.0; // Probabilitate 100% pentru crossover

            // Act
            var kid = Crossover.Arithmetic(mother, father, rate);

            // Assert 
            Assert.Equal(10, kid.Genes[0]);
        }

    }
}

