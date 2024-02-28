using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ConsoleApp1
{
    class Control
    {    
        static void Main()
        {
            Metods metod = new Metods();
            try
            {
                Console.WriteLine("Данная программа решает закрытые транспортные задачи.");
                //Красивый ввод
                Console.WriteLine("Введите поставки через пробел:");
                string buffer1 = Console.ReadLine();
                int N = buffer1.Split(" ").Length; //определяем количество элементов
                int[] temp1 = new int[N]; //создаем временный массив под количество элементов
                temp1 = buffer1.Split(" ").Select(int.Parse).ToArray(); //Переводим в массив int               
                Console.WriteLine("Введите потребителей через пробел:");
                string buffer2 = Console.ReadLine();
                int M = buffer2.Split(" ").Length; //определяем количество элементов
                int[] temp2 = new int[M]; //создаем временный массив под количество элементов
                temp2 = buffer2.Split(" ").Select(int.Parse).ToArray(); //Переводим в массив int             
                //Заполняем поставки и потребителей
                int[,] Tariff = new int[N+1,M+1];
                for (int i = 1, t = 0; i < Tariff.GetLength(0); i++, t++)
                {
                    Tariff[i, 0] = temp1[t];
                }
                for (int j = 1, t = 0; j < Tariff.GetLength(1); j++, t++)
                {
                    Tariff[0, j] = temp2[t];
                }
                //Заполняем тарифы
                string str;
                int[] temp;
                Console.WriteLine($"Введите матрицу тарифов:");
                for (int i = 1; i < Tariff.GetLength(0); i++)
                {
                    str = Console.ReadLine();
                    temp = new int[str.Split(" ").Length];
                    temp = str.Split(" ").Select(int.Parse).ToArray();
                    for (int j = 1, t = 0; j < Tariff.GetLength(1); j++, t++)
                    {
                        Tariff[i, j] = temp[t];
                    }
                }
                int n, end;
                do
                {
                    Console.WriteLine("Выберите каким методом решить транспортную задачу: ");
                    Console.WriteLine("1. Метод северно-западного угла.");
                    Console.WriteLine("2. Метод минимальной стоимости.");
                    Console.WriteLine("3. Метод двойного предпочтения.");
                    Console.WriteLine("4. Метод аппроксимации Фогеля.");
                    n = Convert.ToInt32(Console.ReadLine());
                    switch (n)
                    {
                        case 1: metod.Metod1(Tariff); break;
                        case 2: metod.Metod2(Tariff); break;
                        case 3: metod.Metod3(Tariff); break;
                        case 4: metod.Metod4(Tariff); break;
                        default: Console.WriteLine("Такого метода нет"); break;
                    }
                    Console.WriteLine("Вы хотите выбрать другой метод? (1 - да, 0 - нет).");
                    end = Convert.ToInt32(Console.ReadLine());
                } while (end > 0);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Тип ошибки: {e.GetType().Name}");
                Console.WriteLine($"Строка: {e.StackTrace}");
            }
        }
    }
}