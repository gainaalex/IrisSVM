using System;
using System.IO;
using IrisSVM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IrisSVM.tests
{
    [TestClass]
    public class IrisDataTests
    {
        private string _testFilePath;

        // Rulează înainte de FIECARE test
        [TestInitialize]
        public void Setup()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), "iris_test_data.csv");
        }

        [TestMethod]
        public void LoadIrisData_ShouldLoadValidData_AndFilterCorrectly() // verifica ca fisierul CSV e corect si clasele sunt mapate +1/-1
        {
            // 1. Arrange
            var csvContent =
                "5.1,3.5,1.4,0.2,Iris-setosa\n" +      // +1
                "7.0,3.2,4.7,1.4,Iris-versicolor\n" +  // -1
                "6.3,3.3,6.0,2.5,Iris-virginica\n" +   // ignora
                "\n" +                                  // ignora
                "4.9,3.0,1.4,0.2,Iris-setosa";         // +1

            File.WriteAllText(_testFilePath, csvContent);
            var irisData = new IrisData();

            // 2. Act
            irisData.LoadIrisData(_testFilePath);

            // 3. Assert
            Assert.AreEqual(3, irisData.X.Count);
            Assert.AreEqual(3, irisData.y.Count);

            Assert.AreEqual(1, irisData.y[0]);   // Setosa -> +1
            Assert.AreEqual(-1, irisData.y[1]);  // Versicolor -> -1
            Assert.AreEqual(1, irisData.y[2]);   // Setosa -> +1

            Assert.AreEqual(5.1, irisData.X[0][0], 1e-9);
            Assert.AreEqual(3.5, irisData.X[0][1], 1e-9);
        }

        [TestMethod]
        public void LoadIrisData_FileNotFound_ThrowsFileNotFoundException() // verifica ca metoda arunca exceptie daca fisierul nu exista
        {
            // Arrange
            var irisData = new IrisData();
            string nonExistentPath = "cale_falsa.csv";

            // Act & Assert
            Assert.ThrowsException<FileNotFoundException>(
                () => irisData.LoadIrisData(nonExistentPath)
            );
        }

        [TestMethod]
        public void LoadIrisData_EmptyFile_ReturnsEmptyLists() // verifica ca un fisier gol produce liste x si y goale
        {
            // Arrange
            File.WriteAllText(_testFilePath, "");
            var irisData = new IrisData();

            // Act
            irisData.LoadIrisData(_testFilePath);

            // Assert
            Assert.AreEqual(0, irisData.X.Count);
            Assert.AreEqual(0, irisData.y.Count);
        }

        [TestMethod]

        public void LoadIrisData_ShouldIgnoreInvalidLines() // ignora liniile invalide
        {
            var csvContent =
                "5.1,3.5,1.4,0.2,Iris-setosa\n" +
                "invalid,line,here\n" +  // linie invalida
                "6.0,2.9,4.5,1.5,Iris-versicolor";
            File.WriteAllText(_testFilePath, csvContent);

            var iris = new IrisData();

            iris.LoadIrisData(_testFilePath);

            Assert.AreEqual(2, iris.X.Count, "Trebuie ignorate linii invalide");
            Assert.AreEqual(2, iris.y.Count);
        }

        // Rulează după FIECARE test
        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
}
