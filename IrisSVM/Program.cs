using IrisSVM;

public class Program
{
    public static void Main()
    {
        IrisData irisDatabase=new IrisData();
        irisDatabase.LoadIrisData("iris.data");
        List<double[]> X = irisDatabase.X;
        List<int> Y = irisDatabase.y;

        /*Console.WriteLine("Numar exemple din fisier: " + X.Count);
        Console.WriteLine("Primul vector: " + string.Join(", ", X[0]));
        Console.WriteLine("Tipul: " + Y[0]);*/

        int N = X.Count;
        double C = 1.0;

        var svmProblem = new IrisClasificationProblem(X, Y, C);

        var ea = new EvolutionaryAlgorithm();
        Chromosome best = ea.Solve(svmProblem,
                                    populationSize: 50,
                                    maxGenerations: 100,
                                    crossoverRate: 0.7,
                                    mutationRate: 0.1);

        double[] w = IrisClasificationProblem.ComputeW(X, Y, best);
        double b = IrisClasificationProblem.ComputeB(X, Y, best);

        Console.WriteLine("Vector w: " + string.Join(", ", w));
        Console.WriteLine("Bias b: " + b);


        using (var writer = new StreamWriter("svm_solution.txt"))
        {
            writer.WriteLine(string.Join(",", w));
            writer.WriteLine(b);
            writer.WriteLine(string.Join(",", best.Genes));
        }

        double[][] newFlowers = new double[][]
        {
                new double[]{5.1,3.5,1.4,0.2}, // Setosa
                new double[]{6.0,2.9,4.5,1.5}  // Versicolor
        };

        foreach (var xNew in newFlowers)
        {
            double f = 0;
            for (int j = 0; j < xNew.Length; j++)
                f += w[j] * xNew[j];
            f += b;

            string predicted = f >= 0 ? "Iris-setosa" : "Iris-versicolor";
            Console.WriteLine($"Floare: [{string.Join(", ", xNew)}] => Tip ul anticipat:: {predicted}");
        }
        Console.ReadKey();

    }
}