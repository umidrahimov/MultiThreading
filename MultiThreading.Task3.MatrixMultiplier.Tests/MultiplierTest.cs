using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        [DataRow(1, false)]
        [DataRow(5, false)]
        [DataRow(10, false)]
        [DataRow(15, true)]
        [DataRow(20, true)]
        public void ParallelEfficiencyTest(int size, bool isFaster)
        {
            var matrix1 = new Matrix(size, size, true);
            var matrix2 = new Matrix(size, size, true);

            MatricesMultiplier matrixMultiplier = new MatricesMultiplier();
            MatricesMultiplierParallel matricesMultiplierParallel = new MatricesMultiplierParallel();

            var sequentialTime = MeasureExecutionTime(() => matrixMultiplier.Multiply(matrix1, matrix2));
            var parallelTime = MeasureExecutionTime(() => matricesMultiplierParallel.Multiply(matrix1, matrix2));

            Console.WriteLine($"Size: {size}, Sequential: {sequentialTime.TotalMilliseconds} ms, Parallel: {parallelTime.TotalMilliseconds} ms");

            Assert.AreEqual(isFaster, parallelTime < sequentialTime, "Expected parallel performance to match the provided isFaster value.");
        }

        [TestMethod]
        public void FindOptimalSizeForParallelProcessing()
        {
            int maxSize = 50; // A maximum size to avoid excessively long tests
            int step = 1; // A step size for increasing the matrix dimensions

            MatricesMultiplier matrixMultiplier = new MatricesMultiplier();
            MatricesMultiplierParallel matricesMultiplierParallel = new MatricesMultiplierParallel();

            for (int size = 1; size <= maxSize; size += step)
            {
                var matrix1 = new Matrix(size, size, true);
                var matrix2 = new Matrix(size, size, true);

                var sequentialTime = MeasureExecutionTime(() => matrixMultiplier.Multiply(matrix1, matrix2));
                var parallelTime = MeasureExecutionTime(() => matricesMultiplierParallel.Multiply(matrix1, matrix2));

                Console.WriteLine($"Size: {size}, Sequential: {sequentialTime.TotalMilliseconds} ms, Parallel: {parallelTime.TotalMilliseconds} ms");

                if (parallelTime < sequentialTime)
                {
                    Console.WriteLine($"Parallel processing becomes faster at matrix size {size}");
                    break;
                }
            }
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        TimeSpan MeasureExecutionTime(Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }


        #endregion
    }
}
