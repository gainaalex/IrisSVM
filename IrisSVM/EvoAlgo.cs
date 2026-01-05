using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisSVM
{
    

    /// <summary>
    /// Clasa care reprezinta operatia de selectie
    /// </summary>
    public class Selection
    {
        private static Random _rand = new Random();

        public static Chromosome Tournament(Chromosome[] population)
        {
            var c1 = _rand.Next(population.Length);
            var c2 = _rand.Next(population.Length);
            return population[c1].Fitness < population[c2].Fitness ? population[c2] : population[c1];
        }

        public static Chromosome GetBest(Chromosome[] population)
        {
            Chromosome best = population[0];
            foreach (var c in population)
            {
                if (c.Fitness > best.Fitness)
                    best = c;
            }
            return best;
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta operatia de incrucisare
    /// </summary>
    public class Crossover
    {
        private static Random _rand = new Random();

        public static Chromosome Arithmetic(Chromosome mother, Chromosome father, double rate)
        {
            var rnd_num = _rand.NextDouble();
            var kid = new Chromosome(mother);
            if (rnd_num <= rate)
            {
                for (int i = 0; i < mother.NoGenes; i++)
                {
                    kid.Genes[i] = rate * mother.Genes[i] + (1 - rate) * father.Genes[i];
                }
                return kid;
            }
            else
            {
                return mother.Fitness > father.Fitness ? mother : father;
            }
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta operatia de mutatie
    /// </summary>
    public class Mutation
    {
        private static Random _rand = new Random();

        public static void Reset(Chromosome child, double rate)
        {
            var rnd_num = _rand.NextDouble();
            if (rnd_num <= rate)
            {
                child = new Chromosome(child);
            }
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care implementeaza algoritmul evolutiv pentru optimizare
    /// </summary>
    public class EvolutionaryAlgorithm
    {
        /// <summary>
        /// Metoda de optimizare care gaseste solutia problemei
        /// </summary>
        public Chromosome Solve(IOptimizationProblem p, int populationSize, int maxGenerations, double crossoverRate, double mutationRate)
        {
            Chromosome[] population = new Chromosome[populationSize];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = p.MakeChromosome();
                p.ComputeFitness(population[i]);
            }

            for (int gen = 0; gen < maxGenerations; gen++)
            {
                Chromosome[] newPopulation = new Chromosome[populationSize];
                newPopulation[0] = Selection.GetBest(population); // elitism

                for (int i = 1; i < populationSize; i++)
                {
                    // selectare 2 parinti: Selection.Tournament
                    var parent1 = Selection.Tournament(population);
                    var parent2 = Selection.Tournament(population);
                    // generarea unui copil prin aplicare crossover: Crossover.Arithmetic
                    var kid = Crossover.Arithmetic(parent2, parent1, crossoverRate);
                    // aplicare mutatie asupra copilului: Mutation.Reset
                    Mutation.Reset(kid, mutationRate);
                    // calculare fitness pentru copil: ComputeFitness din problema p
                    p.ComputeFitness(kid);
                    // introducere copil in newPopulation
                    newPopulation[i] = kid;

                }
                for (int i = 0; i < populationSize; i++)
                    population[i] = newPopulation[i];
            }
            return Selection.GetBest(population);
        }
    }
    
}