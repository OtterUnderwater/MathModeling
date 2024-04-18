using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class TravellingSalesmanProblem
    {
        const int M = Int32.MaxValue;
        int[,] matrix;

        public TravellingSalesmanProblem(int[,] matrix)
        {
            this.matrix = matrix;
            PrintTable();
        }

        public void PrintTable()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == M)
                    {
                        Console.Write("     M");
                    }
                    else
                    {
                        Console.Write("{0,6}", matrix[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }

        public void LittsAlgorithm()
        {

        }

    }
}
