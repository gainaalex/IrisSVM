using System;
using IrisSVM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IrisSVM.Tests
{   
    [TestClass]
    public class ChromosomeAndMutationTest
    {
        [TestMethod]

        public void Chromosome_CopyConstructor_ShouldCreateIdenticalCopy() // verifica ca constructorul de copiere creeaza un cromozom identic
        {
            var original = new Chromosome(3, new double[] { 0, 0, 0 }, new double[] { 1, 1, 1 });

            original.Genes[0] = 0.5;
            original.Genes[1] = 0.7;
            original.Genes[2] = 0.9;

            var copy = new Chromosome(original);

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(original.Genes[i], copy.Genes[i], 1e-9, $"Genele nu sunt indentice la {i}");
                Assert.AreEqual(original.MinValues[i], copy.MinValues[i], 1e-9);
                Assert.AreEqual(original.MaxValues[i], copy.MaxValues[i], 1e-9);
            }
        }

        [TestMethod]

        public void Chromosome_GenesShouldBeWithinBounds() // verifica ca genes cromozomului sunt mereu in int [min, max]
        {
            var min = new double[] { 0, 1, 2 };
            var max = new double[] { 5, 6, 7 };
            var c = new Chromosome(3, min, max);

            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(c.Genes[i] >= min[i] && c.Genes[i] <= max[i], $"Gene {i} nu este in int [{min[i]}, {max[i]}]");
            }
        }

        /* [TestMethod]

         public void Mutation_Reset_ShouldModifyGenes_WhenProbHigh() // verifica ca mutatia modifica genes cand prob e mare
         {
             var c = new Chromosome(3, new double[] { 0, 0, 0 }, new double[] { 1, 1, 1 });
             double[] originalGenes = (double[])c.Genes.Clone();

             Mutation.Reset(c, 1.0);

             bool ok = false;

             for (int i = 0; i < 3; i++)
             {
                 if (c.Genes[i] != originalGenes[i])
                     ok = true;
             }

             Assert.IsTrue(ok, "Cel putin o gena ar trebui sa se modifice la prob 100%");
         }*/

        [TestMethod]

        public void Mutation_Reset_ShouldNotThrow()// Mutation.Reset poate fi apelata fara sa dea erori
        {
            var c = new Chromosome(3, new double[] { 0, 0, 0 }, new double[] { 1, 1, 1 });

            Mutation.Reset(c, 1.0);
            Mutation.Reset(c, 0.0);
        }
    }
}
