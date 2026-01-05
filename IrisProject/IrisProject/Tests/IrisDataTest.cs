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
    public class IrisDataTests : IDisposable
    {
        private readonly string _testFilePath;

        public IrisDataTests()
        {
            // Creaza un nume de fisier temporar pentru teste
            _testFilePath = Path.Combine(Path.GetTempPath(), "iris_test_data.csv");
        }

        [Fact]
        public void LoadIrisData_ShouldLoadValidData_AndFilterCorrectly()
        {
            // 1. Arrange: Pregatim date de test
            var csvContent =
                "5.1,3.5,1.4,0.2,Iris-setosa\n" +      // +1
                "7.0,3.2,4.7,1.4,Iris-versicolor\n" +  // -1
                "6.3,3.3,6.0,2.5,Iris-virginica\n" +   // ignora
                "\n" +                                  // ignora
                "4.9,3.0,1.4,0.2,Iris-setosa";         // +1

            File.WriteAllText(_testFilePath, csvContent);
            var irisData = new IrisData();

            // 2. Act: Executa metoda
            irisData.LoadIrisData(_testFilePath);

            // 3. Assert: Verifica rezultatele
            Assert.Equal(3, irisData.X.Count); // 2 setosa + 1 versicolor
            Assert.Equal(3, irisData.y.Count);

            // Verifica maparea claselor
            Assert.Equal(1, irisData.y[0]);  // Setosa -> +1
            Assert.Equal(-1, irisData.y[1]); // Versicolor -> -1
            Assert.Equal(1, irisData.y[2]);  // Setosa -> +1

            Assert.Equal(5.1, irisData.X[0][0]);
            Assert.Equal(3.5, irisData.X[0][1]);
        }

        [Fact]
        public void LoadIrisData_FileNotFound_ThrowsFileNotFoundException()
        {
            // Arrange
            var irisData = new IrisData();
            string nonExistentPath = "cale_falsa.csv";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => irisData.LoadIrisData(nonExistentPath));
        }

        [Fact]
        public void LoadIrisData_EmptyFile_ReturnsEmptyLists()
        {
            // Arrange
            File.WriteAllText(_testFilePath, "");
            var irisData = new IrisData();

            // Act
            irisData.LoadIrisData(_testFilePath);

            // Assert
            Assert.Empty(irisData.X);
            Assert.Empty(irisData.y);
        }

        // Elimina fisierul temporar dupa teste
        public void Dispose()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
}