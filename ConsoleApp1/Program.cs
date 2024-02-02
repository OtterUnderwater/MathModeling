using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ВходнойКонтроль
{
    class Control
    {
        private static bool Closed(int[,] Array)
        {
            int SumRow = 0;
            int SumColumn = 0;
            for (int i = 0; i < Array.GetLength(1); i++)
            {
                SumRow += Array[0, i];  //сумма в строке
            }
            for (int i = 0; i < Array.GetLength(0); i++)
            {
                SumColumn += Array[i, 0]; //сумма в столбце
            }
            if (SumRow == SumColumn)
            {
                Console.WriteLine("Задача закрытая!");
                return true;
            } 
            else
            {
                Console.WriteLine("Задача открытая!");
                return false;
            }
        }

        private static int max(int[,] Array)
        {
            int max = Array[1, 1];
            for (int i = 1; i < Array.GetLength(0); i++)
            {
                for (int j = 1; j < Array.GetLength(1); j++)
                {
                    if (Array[i, j] > max)
                    {
                        max = Array[i, j];
                    }
                }
            }
            return max;
        }

        static void Main()
        {
            int N, M;
            Console.WriteLine("Введите количество склада:");
            N = Convert.ToInt32(Console.ReadLine()) + 1;
            Console.WriteLine("Введите количество потреблений:");
            M = Convert.ToInt32(Console.ReadLine()) + 1;

            int[,] Tariff = new int[N, M];
            int[,] Plan = new int[N, M];
            int[,] Check = new int[N, M];
            int SumCheck = 0; // 0 - не заполнен тариф, 1 - заполнен тариф
            int L = 0; //Целевая функция
            int V = (M - 1) + (N-1) - 1; //Вырожденность
            int countV = 0;

            //Ввод
            Console.WriteLine("Введите склад:");  //Строки
            for (int i = 1; i < Tariff.GetLength(1); i++)
            {
                Tariff[0, i] = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine("Введите потребления:"); //Столбцы
            for (int i = 1; i < Tariff.GetLength(0); i++)
            {
                Tariff[i, 0] = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine("Введите тарифы:");
            for (int i = 1; i < Tariff.GetLength(0); i++)
            {
                for (int j = 1; j < Tariff.GetLength(1); j++)
                {
                    Tariff[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }

            //Подсчет тарифного плана
            if (Closed(Tariff))
            {
                Console.WriteLine("Тарифы:");
                for (int i = 0; i < Tariff.GetLength(0); i++)
                {
                    for (int j = 0; j < Tariff.GetLength(1); j++)
                    {
                        Console.Write(Tariff[i, j] + " \t");
                    }
                    Console.WriteLine();
                }
                while (SumCheck < (N-1)*(M-1))
                {
                    //Поиск минимального элемента
                    int minEl = max(Tariff);
                    for (int i = 1; i < Tariff.GetLength(0); i++)
                    {
                        for (int j = 1; j < Tariff.GetLength(1); j++)
                        {
                            if (Tariff[i, j] < minEl && Check[i, j] == 0)
                            {
                                minEl = Tariff[i, j];
                            }
                        }
                    }
                    //Заполняем ячейку тарифом
                    for (int i = 1; i < Tariff.GetLength(0); i++)
                    {
                        for (int j = 1; j < Tariff.GetLength(1); j++)
                        {
                            if (Tariff[i, j] == minEl && Check[i, j] == 0)
                            {
                                //Данный элемент минимальный и тариф не заполнен
                                if (Tariff[0, j] >= Tariff[i, 0])
                                {
                                    Plan[i, j] = Tariff[i, 0];
                                    Tariff[0, j] -= Tariff[i, 0];
                                    Tariff[i, 0] -= Tariff[i, 0];
                                }
                                else if (Tariff[0, j] < Tariff[i, 0])
                                {
                                    Plan[i, j] = Tariff[0, j];
                                    Tariff[i, 0] -= Tariff[0, j];
                                    Tariff[0, j] -= Tariff[0, j];
                                }
                                Check[i, j] = 1;
                            }
                        }
                    }
                    SumCheck = 0;
                    //Проверка на количество заполненных элементов
                    for (int i = 1; i < Check.GetLength(0); i++)
                    {
                        for (int j = 1; j < Check.GetLength(1); j++)
                        {
                            SumCheck += Check[i, j];
                        }
                    }
                }
                //Ответ
                Console.WriteLine("Тарифый план:");
                for (int i = 1; i < Plan.GetLength(0); i++)
                {
                    for (int j = 1; j < Plan.GetLength(1); j++)
                    {
                        Console.Write(Plan[i, j] + " \t");
                        L = L + (Plan[i, j] * Tariff[i, j]);
                        if (Plan[i, j] != 0)
                        {
                            countV++;
                        }
                    }
                    Console.WriteLine();
                }
                if (V == countV){
                    Console.WriteLine("Невырожденная задача!");
                }
                else {
                    Console.WriteLine("Вырожденная задача!");
                }
                Console.WriteLine($"L(X) = {L}");
            } 
        }
    }
}