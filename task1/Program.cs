using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    internal class Program
    {
        static void ReadData(out int size, out double[,] matrixTensor, out double[] vector)
        {
            try
            {
                using (StreamReader sr = new StreamReader("data.txt"))
                {
                    size = Int32.Parse(sr.ReadLine());
                    matrixTensor = new double[size, size];

                    for (int i = 0; i < size; i++)
                    {
                        string[] line = sr.ReadLine().Split();
                        for (int j = 0; j < size; j++)
                        {
                            matrixTensor[i, j] = Int32.Parse(line[j]);
                        }
                    }

                    string[] vectorString = sr.ReadLine().Split(' ');
                    vector = new double[size];

                    for (int i = 0; i < size; i++)
                    {
                        vector[i] = Int32.Parse(vectorString[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                size = 0;
                matrixTensor = null;
                vector = null;
            }
        }
        static bool IsSymmetric(double[,] matrix, double size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (matrix[i, j] != matrix[j, i]) return false;
                }
            }
            return true;
        }
        static double CalculateLength(int size, double[,] matrixTensor, double[] vector)
        {
            double[] tempVector = new double[size]; 
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    tempVector[i] += vector[j] * matrixTensor[i, j];
                }
            }

            double result = 0;
            for (int i = 0; i < size; i++)
            {
                result += vector[i] * tempVector[i];
            }
            return Math.Sqrt(result);
        }
        static void Main(string[] args)
        {
            int size;
            double[,] matrixTensor;
            double[] vector;
            ReadData(out size, out matrixTensor, out vector);

            if (IsSymmetric(matrixTensor, size))
            {
                Console.WriteLine(CalculateLength(size, matrixTensor, vector));
            }
            else
            {
                Console.WriteLine("Матрица не симметрична");
            }
        }
    }
}
