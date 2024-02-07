using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ВходнойКонтроль
{
    class Control
    {
        static bool Closed(int[,] Array)
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
        static int max(int[,] Array)
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
        static void Metod1()
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
            int V = (M - 1) + (N - 1) - 1; //Вырожденность
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
                while (SumCheck < (N - 1) * (M - 1))
                {
                    //Заполняем ячейки тарифом
                    for (int j = 1; j < Tariff.GetLength(1); j++)
                    {
                        for (int i = 1; i < Tariff.GetLength(0); i++)
                        {
                            if (Check[i, j] == 0)
                            {
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
                if (V == countV)
                {
                    Console.WriteLine("Невырожденная задача!");
                }
                else
                {
                    Console.WriteLine("Вырожденная задача!");
                }
                Console.WriteLine($"L(X) = {L}");
            }
        }
        static void Metod2()
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
        static void Metod3()
        {
            int N, M;
            Console.WriteLine("Введите количество склада:");
            N = Convert.ToInt32(Console.ReadLine()) + 1;
            Console.WriteLine("Введите количество потреблений:");
            M = Convert.ToInt32(Console.ReadLine()) + 1;

            int[,] Tariff = new int[N, M];
            int[,] Plan = new int[N, M];
            int[,] Check = new int[N, M]; //0 - не заполнен, 1 - предпочтение +, 2 - предпочтение ++, 3 - заполнен тариф
            int SumCheck = 0; // Проверка количества заполненных ячеек
            int L = 0; //Целевая функция
            int V = (M - 1) + (N - 1) - 1; //Вырожденность
            int countV = 0; //Проверка на вырожденность
            int MinEl = 0;
            int Preference = 2;
            int CountPref = 0;

            //Строки
            Tariff[0, 1] = 13;
            Tariff[0, 2] = 5;
            Tariff[0, 3] = 13;
            Tariff[0, 4] = 12;
            Tariff[0, 5] = 13;
            //Столбцы
            Tariff[1, 0] = 14;
            Tariff[2, 0] = 14;
            Tariff[3, 0] = 14;
            Tariff[4, 0] = 14;
            //Тарифы
            Tariff[1, 1] = 16;
            Tariff[1, 2] = 26;
            Tariff[1, 3] = 12;
            Tariff[1, 4] = 24;
            Tariff[1, 5] = 3;

            Tariff[2, 1] = 5;
            Tariff[2, 2] = 2;
            Tariff[2, 3] = 19;
            Tariff[2, 4] = 27;
            Tariff[2, 5] = 2;

            Tariff[3, 1] = 29;
            Tariff[3, 2] = 23;
            Tariff[3, 3] = 25;
            Tariff[3, 4] = 16;
            Tariff[3, 5] = 8;

            Tariff[4, 1] = 2;
            Tariff[4, 2] = 25;
            Tariff[4, 3] = 14;
            Tariff[4, 4] = 15;
            Tariff[4, 5] = 21;
            //Ввод
            //Console.WriteLine("Введите склад:");  //Строки
            //for (int i = 1; i < Tariff.GetLength(1); i++)
            //{
            //    Tariff[0, i] = Convert.ToInt32(Console.ReadLine());
            //}
            //Console.WriteLine("Введите потребления:"); //Столбцы
            //for (int i = 1; i < Tariff.GetLength(0); i++)
            //{
            //    Tariff[i, 0] = Convert.ToInt32(Console.ReadLine());
            //}
            //Console.WriteLine("Введите тарифы:");
            //for (int i = 1; i < Tariff.GetLength(0); i++)
            //{
            //    for (int j = 1; j < Tariff.GetLength(1); j++)
            //    {
            //        Tariff[i, j] = Convert.ToInt32(Console.ReadLine());
            //    }
            //}

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
                //Двойное предпочтение
                //Расставляем плюсы в предпочтении по строкам
                for (int i = 1; i < Tariff.GetLength(0); i++)
                {
                    MinEl = Tariff[i, 1];
                    for (int j = 1; j < Tariff.GetLength(1); j++)
                    {
                        if (Tariff[i, j] < MinEl)
                        {
                            MinEl = Tariff[i, j];
                        }
                    }
                    //На случай, если в строке два минимальных элемента
                    for (int j = 1; j < Tariff.GetLength(1); j++)
                    {
                        if (Tariff[i, j] <= MinEl)
                        {
                            Check[i, j]++;
                        }
                    }
                }
                //Расставляем плюсы в предпочтении по столбцам
                for (int j = 1; j < Tariff.GetLength(1); j++)
                {
                    MinEl = Tariff[1, j];
                    for (int i = 1; i < Tariff.GetLength(0); i++)
                    {
                        if (Tariff[i, j] < MinEl)
                        {
                            MinEl = Tariff[i, j];
                        }
                    }
                    //На случай, если в столбце два минимальных элемента
                    for (int i = 1; i < Tariff.GetLength(0); i++)
                    {
                        if (Tariff[i, j] <= MinEl)
                        {
                            Check[i, j]++;
                        }
                    }
                }
                while (SumCheck < (N - 1) * (M - 1))
                {
                    //Заполняем ячейку тарифом (Начиная с максимального предпочтения, пока оно не закончится)
                    while (Preference != 0)
                    {
                        for (int i = 1; i < Tariff.GetLength(0); i++)
                        {
                        for (int j = 1; j < Tariff.GetLength(1); j++)
                        {
                            if (Check[i, j] == Preference) //preference = 2, потом = 1
                            {
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
                                Check[i, j] = 3;
                            }
                        }
                        }
                        //Проверка сколько осталось предпочтений
                        CountPref = 0;
                        for (int i = 1; i < Check.GetLength(0); i++)
                        {
                            for (int j = 1; j < Check.GetLength(1); j++)
                            {
                                if (Check[i, j] == Preference)
                                {
                                    CountPref++;
                                }
                            }
                        }
                        if (CountPref == 0)
                        {
                            Preference--;
                        }
                    }
                    //Поиск минимального элемента незаполненных ячеек
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
                                Check[i, j] = 3;
                            }
                        }
                    }
                    SumCheck = 0;
                    //Проверка на количество заполненных элементов
                    for (int i = 1; i < Check.GetLength(0); i++)
                    {
                        for (int j = 1; j < Check.GetLength(1); j++)
                        {
                            if (Check[i, j] == 3) //ячейка заполнена
                            {
                                SumCheck++;
                            }
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
                if (V == countV)
                {
                    Console.WriteLine("Невырожденная задача!");
                }
                else
                {
                    Console.WriteLine("Вырожденная задача!");
                }
                Console.WriteLine($"L(X) = {L}");
            }
        }
        static void Main()
        {
            int n, end;
            do
            {
                Console.WriteLine("Выберите каким методом решить транспортную задачу: ");
                Console.WriteLine("1. Метод северно-западного угла");
                Console.WriteLine("2. Метод минимального элемента");
                Console.WriteLine("3. Метод двойного предпочтения");
                Console.WriteLine("4. Метод Фогеля");
                n = Convert.ToInt32(Console.ReadLine());
                switch (n)
                {
                    case 1: Metod1(); break;
                    case 2: Metod2(); break;
                    case 3: Metod3(); break;
                    //case 4: Metod4(); break;
                    default: Console.WriteLine("Такого задания нет"); break;
                }
                Console.WriteLine("Вы хотите выбрать другой метод? (1 - да, 0 - нет).");
                end = Convert.ToInt32(Console.ReadLine());
            } while (end > 0);
        }
    }
}