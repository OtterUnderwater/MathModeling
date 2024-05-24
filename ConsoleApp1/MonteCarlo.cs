using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class MonteCarlo
    {
        private readonly Random random = new Random();
        private readonly int _countGrains = 1000000; //количество итераций цикла
        public void GetMonteCarloPi()
        {
            double side = 2;
            double S0 = Math.Pow(side, 2);
            double countHits = 0; //количество попаданий
            for (int i = 0; i < _countGrains; i++)
            {
                double x = random.NextDouble() * side;
                double y = random.NextDouble() * side;
                if ((Math.Pow((x - 1), 2) + Math.Pow((y - 1), 2)) <= 1)
                {
                    countHits++;
                }
            }
            double S = S0 * countHits / _countGrains;
            Console.WriteLine($"Результат pi = {S}");
            Console.WriteLine($"Точное pi = {Math.PI}");
        }
        public void GetMonteCarloS()
        {
            //ширина и высота базового прямоугольника
            double width = 8.5;  //ширина прямоугольника (a)
            double height = 5; //высота прямоугольника (b)
            double countHits = 0; //количество попаданий (k)
            for (int i = 0; i < _countGrains; i++)
            {
                double x = random.NextDouble() * width;
                double y = random.NextDouble() * height;
                if ((x / 3 <= y) && (y <= x * (10 - x) / 5))
                {
                    countHits++;
                }
            }
            double S = width * height * countHits / _countGrains;
            Console.WriteLine($"Результат S = {S}");
        }
        public void GetMonteCarloS1()
        {
            double width = 20; //ширина
            double height = 1; //высота
			double countHits = 0; //количество попаданий
            for (int i = 0; i < _countGrains; i++)
            {
                double x = random.NextDouble() * width; 
				double y = random.NextDouble() * height;
				if ((0 <= y && y <= Math.Sin(x)) &&
					((-4 <= x && x <= -3) || (0 <= x && x <= 3) || (6 <= x && x <= 10) || (13 <= x && x <= 15)))
                {
                    countHits++;
                }
            }
            double S = width * height * countHits / _countGrains;
            Console.WriteLine($"Результат S = {S}");
        }
		public void GetMonteCarloS2()
		{
			double width = 7;
			double height = 8;
			double countHits = 0;
			for (int i = 0; i < _countGrains; i++)
			{
				double x = random.NextDouble() * width;
				double y = random.NextDouble() * height;
				if ((0 <= x && x <= 7) && ((x/2) <= y && y <= (x * (8 - x) / 2)))
				{
					countHits++;
				}
			}
			double S = width * height * countHits / _countGrains;
			Console.WriteLine($"Результат S = {S}");
		}
		public void GetMonteCarloS3()
		{
			double width = 12;
			double height = 6;
			double countHits = 0;
			for (int i = 0; i < _countGrains; i++)
			{
				double x = random.NextDouble() * width;
				double y = random.NextDouble() * height;
				if ((0 <= x && x <= 12) && (Math.Pow((x - 6), 2) / 6 <= y && y <= 6))
				{
					countHits++;
				}
			}
			double S = width * height * countHits / _countGrains;
			Console.WriteLine($"Результат S = {S}");
		}
		public void GetMonteCarloS4()
		{
			double width = 10;
			double height = 4;
			double countHits = 0;
			for (int i = 0; i < _countGrains; i++)
			{
				double x = random.NextDouble() * width;
				double y = random.NextDouble() * height;
				if ((0 <= x && x <= 10) && (x / 5 <= y && y <= x * (12 - x) / 9))
				{
					countHits++;
				}
			}
			double S = width * height * countHits / _countGrains;
			Console.WriteLine($"Результат S = {S}");
		}
		public void GetMonteCarloS5()
		{
			double width = 7;
			double height = 4;
			double countHits = 0;
			for (int i = 0; i < _countGrains; i++)
			{
				double x = random.NextDouble() * width;
				double y = random.NextDouble() * height;
				if ((1 <= x && x <= 8) && ((8 - x) / 8 <= y && y <= x * (8 - x) / 4))
				{
					countHits++;
				}
			}
			double S = width * height * countHits / _countGrains;
			Console.WriteLine($"Результат S = {S}");
		}
		public void GetMonteCarloS6()
		{
			double width = 2;
			double height = 1;
			double countHits = 0;
			for (int i = 0; i < _countGrains; i++)
			{
				double x = random.NextDouble() * width;
				double y = random.NextDouble() * height;
				if ((1 <= x && x <= 3) && (Math.Pow((x - 2), 2) / 2 <= y && y <= Math.Sin(x)))
				{
					countHits++;
				}
			}
			double S = width * height * countHits / _countGrains;
			Console.WriteLine($"Результат S = {S}");
		}
    }
}
