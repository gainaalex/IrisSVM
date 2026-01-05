using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisSVM
{
    public class IrisData
    {
        public List<double[]> X { get; private set; } = new List<double[]>();
        public List<int> y { get; private set; } = new List<int>();

        public void LoadIrisData(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Fisierul iris.data nu a fost gasit", path);

            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var tokens = line.Split(',');

                if (tokens.Length != 5)
                    continue;

                double f1 = double.Parse(tokens[0], CultureInfo.InvariantCulture);
                double f2 = double.Parse(tokens[1], CultureInfo.InvariantCulture);
                double f3 = double.Parse(tokens[2], CultureInfo.InvariantCulture);
                double f4 = double.Parse(tokens[3], CultureInfo.InvariantCulture);
                string label = tokens[4].Trim();

                int cls;
                if (label == "Iris-setosa")
                    cls = +1;
                else if (label == "Iris-versicolor")
                    cls = -1;
                else
                    continue; // rezolvand o problema duala (incadram floarea in una din cele 2 clase : setosa/versicolor ) vom ignora clasa virginica

                X.Add(new double[] { f1, f2, f3, f4 });
                y.Add(cls);
            }
        }
    }
}
