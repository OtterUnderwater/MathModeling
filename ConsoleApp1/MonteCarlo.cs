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
        private Random random = new Random();
        private int _countGrains = 1000000; //количество итераций цикла

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

        public void GetMonteCarloS2()
        {
            //ширина и высота базового прямоугольника
            double width = 8.5;  //ширина прямоугольника (a) ХЗ ХЗ ХЗ 
            double height = 5; //высота прямоугольника (b)
            double countHits = 0; //количество попаданий (k)
            for (int i = 0; i < _countGrains; i++)
            {
                double x = random.NextDouble() * 1;
                double y = random.NextDouble() * 2;

                if ((y <= Math.Sin(x) && ((x is -3 or 0) || (x is 3 or 6) || (x is 9 or 12)))
                {
                    countHits++;
                }
            }
            double S = width * height * countHits / _countGrains;
            Console.WriteLine($"Результат S = {S}");
        }
    }
}
