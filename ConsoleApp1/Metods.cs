using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Metods
    {
        bool Closed(int[,] Array)
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
                return true; //Задача закрытая
            }
            else
            {
                Console.WriteLine("К сожалению, программа пока не умеет решать открытые задачи.");
                return false;
            }
        }
        int Max(int[,] Array)
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
        int AllCheck(int[,] Check)
        {
            int SumCheck = 0;
            for (int i = 1; i < Check.GetLength(0); i++)
            {
                for (int j = 1; j < Check.GetLength(1); j++)
                {
                    SumCheck += Check[i, j];
                }
            }
            return SumCheck;
        }
        void PrintPlan(int[,] Plan, int[,] Arr)
        {
            Console.WriteLine("Тарифый план:");
            int n = 1 + 6 * Plan.GetLength(1); //сколько нужно тире
            string str = new string('-', n);
            Console.WriteLine("+" + str + "+"); //верхняя рамка таблицы
            for (int i = 0; i < Plan.GetLength(0); i++)
            {
                for (int j = 0; j < Plan.GetLength(1); j++)
                {
                    if (i == 1 && j == 0)
                    {
                        Console.WriteLine("+" + str + "+"); //рамка, ограничивающая 1ю строку
                    }
                    if (j == 0)
                    {
                        Console.Write("|{0,6}|", Arr[i, j]); //первый столбец с 2х сторон окружен "|"
                    }
                    else if (i == 0)
                    {
                        Console.Write("{0,6}", Arr[i, j]); //первая строка с 2х сторона окружен "|"
                    }
                    else
                    {
                        Console.Write("{0,6}", Plan[i, j]);
                    }
                }
                Console.WriteLine("|"); //переход на следующую строку
            }
            Console.WriteLine("+" + str + "+"); //нижняя рамка таблицы
        }
        void PrintTariff(int[,] Tariff)
        {
            Console.WriteLine("Тарифы:");
            int n = 1 + 6 * Tariff.GetLength(1); //сколько нужно тире
            string str = new string('-', n);
            Console.WriteLine("+" + str + "+"); //верхняя рамка таблицы
            for (int i = 0; i < Tariff.GetLength(0); i++)
            {
                for (int j = 0; j < Tariff.GetLength(1); j++)
                {
                    if (i == 1 && j == 0)
                    {
                        Console.WriteLine("+" + str + "+"); //рамка, ограничивающая 1ю строку
                    }
                    if (j == 0)
                    {
                        Console.Write("|{0,6}|", Tariff[i, j]); //первый столбец с 2х сторон окружен "|"
                    }
                    else
                    {
                        Console.Write("{0,6}", Tariff[i, j]);
                    }
                }
                Console.WriteLine("|"); //переход на следующую строку
            }
            Console.WriteLine("+" + str + "+"); //нижняя рамка таблицы
        }
        void OutputAnswer(int[,] Plan, int[,] Tariff)
        {
            int V = (Tariff.GetLength(0) - 1) + (Tariff.GetLength(1) - 1) - 1; //Вырожденность
            int countV = 0; //Проверка на вырожденность
            int L = 0; //Целевая функция  
            for (int i = 1; i < Plan.GetLength(0); i++)
            {
                for (int j = 1; j < Plan.GetLength(1); j++)
                {
                    L = L + (Plan[i, j] * Tariff[i, j]);
                    if (Plan[i, j] != 0)
                    {
                        countV++;
                    }
                }
            }
            if (V == countV)
            {
                Console.WriteLine("Невырожденная задача");
                Console.WriteLine($"L(X) = {L}");
            }
            else
            {
                Console.WriteLine("Задача вырожденная!");
            }
        }
        public void Metod1(int[,] Arr)
        {
            int N = Arr.GetLength(0);
            int M = Arr.GetLength(1);
            int[,] Plan = new int[N, M];
            int[,] Check = new int[N, M];
            int SumCheck = 0; // 0 - не заполнен тариф, 1 - заполнен тариф
            //Скопировали полученный массив, чтобы не изменять значения
            int[,] Tariff = new int[N, M];
            Array.Copy(Arr, Tariff, Arr.Length);
            //Подсчет тарифного плана
            if (Closed(Tariff))
            {
                PrintTariff(Tariff);
                while (SumCheck < (N - 1) * (M - 1))
                {
                    //Заполняем ячейки тарифом
                    for (int j = 1; j < Tariff.GetLength(1); j++)
                    {
                        for (int i = 1; i < Tariff.GetLength(0); i++)
                        {
                            if (Check[i, j] == 0) //Ячейка свободна
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
                    //Проверка на количество заполненных элементов
                    SumCheck = AllCheck(Check);
                }
                PrintPlan(Plan, Arr); //Вывод тарифного плана
                OutputAnswer(Plan, Tariff); //Вывод целевой функции и проверка на вырожденность   
            }
        }
        public void Metod2(int[,] Arr)
        {
            int N = Arr.GetLength(0);
            int M = Arr.GetLength(1);
            int[,] Plan = new int[N, M];
            int[,] Check = new int[N, M];
            int SumCheck = 0; // 0 - не заполнен тариф, 1 - заполнен тариф
            //Скопировали полученный массив, чтобы не изменять значения
            int[,] Tariff = new int[N, M];
            Array.Copy(Arr, Tariff, Arr.Length);
            //Подсчет тарифного плана
            if (Closed(Tariff))
            {
                PrintTariff(Tariff);
                while (SumCheck < (N - 1) * (M - 1))
                {
                    //Поиск минимального элемента
                    int minEl = Max(Tariff);
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
                    //Проверка на количество заполненных элементов
                    SumCheck = AllCheck(Check);
                }
                PrintPlan(Plan, Arr); //Вывод тарифного плана
                OutputAnswer(Plan, Tariff); //Вывод целевой функции и проверка на вырожденность    
            }
        }
        public void Metod3(int[,] Arr)
        {
            int N = Arr.GetLength(0);
            int M = Arr.GetLength(1);
            int[,] Plan = new int[N, M];
            int[,] Check = new int[N, M]; //0 - не заполнен, 1 - предпочтение "+", 2 - предпочтение "++", 3 - заполнен
            int MinEl = 0;
            int Preference = 2; //Предпочтение
            int CountPref = 0; //Проверка предпочтений
            //Скопировали полученный массив, чтобы не изменять значения
            int[,] Tariff = new int[N, M];
            Array.Copy(Arr, Tariff, Arr.Length);
            //Подсчет тарифного плана
            if (Closed(Tariff))
            {
                PrintTariff(Tariff);
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
                //Заполняем ячейку тарифом (Начиная с максимального предпочтения, пока оно не закончится)
                while (Preference >= 0)
                {
                    int minEl = Max(Tariff);
                    for (int i = 1; i < Tariff.GetLength(0); i++)
                    {
                        for (int j = 1; j < Tariff.GetLength(1); j++)
                        {
                            if (Tariff[i, j] < minEl && Check[i, j] == Preference)
                            {
                                minEl = Tariff[i, j];
                            }
                        }
                    }
                    for (int i = 1; i < Tariff.GetLength(0); i++)
                    {
                        for (int j = 1; j < Tariff.GetLength(1); j++)
                        {
                            if (Tariff[i, j] == minEl && Check[i, j] == Preference) //preference = 2, потом = 1
                            {
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
                PrintPlan(Plan, Arr); //Вывод тарифного плана
                OutputAnswer(Plan, Tariff); //Вывод целевой функции и проверка на вырожденность   
            }
        }
        public void Metod4(int[,] Arr)
        {
            int N = Arr.GetLength(0);
            int M = Arr.GetLength(1);
            int[,] Plan = new int[N, M];
            int[,] Check = new int[N + 1, M + 1];
            int SumCheck = 0; // 0 - не заполнен тариф, 1 - заполнен тариф
            int MinEl1, MinEl2, MaxEl, MinEl;
            int What = 0; // 0 - строка, 1 - столбец
            int IndexEl = 0; // Запоминает строку или столбец с максимальным штрафом
            int countMin2 = 0; //Счетчик сколько элементов в строке
            //Скопировали массив, чтобы не изменять значения и добавляем + 1 колонку и строку для штрафов
            int[,] Tariff = new int[N + 1, M + 1];
            for (int i = 0; i < Tariff.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < Tariff.GetLength(1) - 1; j++)
                {
                    Tariff[i, j] = Arr[i, j];
                }
            }
            //Подсчет тарифного плана
            if (Closed(Tariff))
            {
                PrintTariff(Arr);
                while (SumCheck < (N - 1) * (M - 1))
                {
                    //Считаем штрафы по строкам в доп.столбец тарифов
                    for (int i = 1; i < Tariff.GetLength(0) - 1; i++)
                    {
                        MinEl1 = MinEl2 = Max(Tariff);
                        countMin2 = 0;
                        for (int j = 1; j < Tariff.GetLength(1) - 1; j++)
                        {
                            if (Tariff[i, j] <= MinEl1 && Check[i, j] == 0)
                            {
                                MinEl1 = Tariff[i, j];
                                countMin2++;
                            }
                        }
                        for (int j = 1; j < Tariff.GetLength(1) - 1; j++)
                        {
                            //Проверка чтобы второй минимальный элемент не был равен первому
                            if (Tariff[i, j] < MinEl2 && Tariff[i, j] > MinEl1 && Check[i, j] == 0)
                            {
                                MinEl2 = Tariff[i, j];
                            }
                        }
                        if (Check[i, Tariff.GetLength(1) - 1] == 1) //Штрафы без учета заполненных
                        {
                            Tariff[i, Tariff.GetLength(1) - 1] = 0;
                        }
                        else
                        {
                            //Проверка пересчета штрафа
                            if (MinEl2 == Max(Tariff))
                            {
                                if (countMin2 == 1)
                                {
                                    //Если остался 1 элемент
                                    Tariff[i, Tariff.GetLength(1) - 1] = MinEl1;
                                }
                                else if (countMin2 == 2)
                                {
                                    //Если осталось 2 равных элемента
                                    Tariff[i, Tariff.GetLength(1) - 1] = 0;
                                }
                            }
                            //Если все норм
                            else
                            {
                                Tariff[i, Tariff.GetLength(1) - 1] = MinEl2 - MinEl1; //Штраф по строке
                            }
                        }
                    }
                    //Считаем штрафы по столбцам в доп. строку тарифов
                    for (int j = 1; j < Tariff.GetLength(1) - 1; j++)
                    {
                        MinEl1 = MinEl2 = Max(Tariff);
                        countMin2 = 0;
                        for (int i = 1; i < Tariff.GetLength(0) - 1; i++)
                        {
                            if (Tariff[i, j] <= MinEl1 && Check[i, j] == 0)
                            {
                                MinEl1 = Tariff[i, j];
                                countMin2++;
                            }
                        }
                        for (int i = 1; i < Tariff.GetLength(0) - 1; i++)
                        {
                            //Проверка чтобы второй минимальный элемент не был равен первому
                            if (Tariff[i, j] < MinEl2 && Tariff[i, j] > MinEl1 && Check[i, j] == 0)
                            {
                                MinEl2 = Tariff[i, j];
                            }
                        }
                        if (Check[Tariff.GetLength(0) - 1, j] == 1) //Штрафы без учета заполненных
                        {
                            Tariff[Tariff.GetLength(0) - 1, j] = 0;
                        }
                        else
                        {
                            //Проверка пересчета штрафа
                            if (MinEl2 == Max(Tariff))
                            {
                                if (countMin2 == 1)
                                {
                                    //Если остался 1 элемент
                                    Tariff[Tariff.GetLength(0) - 1, j] = MinEl1;
                                }
                                else if (countMin2 == 2)
                                {
                                    //Если осталось 2 равных элемента
                                    Tariff[Tariff.GetLength(0) - 1, j] = 0;
                                }
                            }
                            //Если все норм
                            else
                            {
                                Tariff[Tariff.GetLength(0) - 1, j] = MinEl2 - MinEl1; //Штраф по столбцу
                            }
                        }
                    }
                    //Поиск максимального штрафа
                    MaxEl = 0;
                    for (int i = 1; i < Tariff.GetLength(0); i++)
                    {
                        //Максимальное в столбце dj
                        if (Tariff[i, Tariff.GetLength(1) - 1] > MaxEl)
                        {
                            MaxEl = Tariff[i, Tariff.GetLength(1) - 1];
                            What = 0; //Запоминаем строку Max элемента
                            IndexEl = i;
                        }
                    }
                    for (int j = 1; j < Tariff.GetLength(1); j++)
                    {
                        //Максимальное в строке di
                        if (Tariff[Tariff.GetLength(0) - 1, j] > MaxEl)
                        {
                            MaxEl = Tariff[Tariff.GetLength(0) - 1, j];
                            What = 1; //Запоминаем столбец Max элемента
                            IndexEl = j;
                        }
                    }
                    //Заполняем ячейки тарифом
                    MinEl = Max(Tariff);
                    if (What == 0)
                    {
                        //Находим минимальный элемент в строке, где макс штраф
                        for (int j = 1; j < Tariff.GetLength(1) - 1; j++)
                        {
                            if (MinEl > Tariff[IndexEl, j] && Check[IndexEl, j] == 0)
                            {
                                MinEl = Tariff[IndexEl, j];
                            }
                        }
                        //Заполняем ячейку с мин элементом и массив чек
                        for (int j = 1; j < Tariff.GetLength(1) - 1; j++)
                        {
                            if (Check[IndexEl, j] == 0 && Tariff[IndexEl, j] == MinEl)
                            {
                                if (Tariff[0, j] >= Tariff[IndexEl, 0])
                                {
                                    Plan[IndexEl, j] = Tariff[IndexEl, 0];
                                    Tariff[0, j] -= Tariff[IndexEl, 0];
                                    Tariff[IndexEl, 0] -= Tariff[IndexEl, 0];
                                }
                                else if (Tariff[0, j] < Tariff[IndexEl, 0])
                                {
                                    Plan[IndexEl, j] = Tariff[0, j];
                                    Tariff[IndexEl, 0] -= Tariff[0, j];
                                    Tariff[0, j] -= Tariff[0, j];
                                }
                                Check[IndexEl, j] = 1;
                            }
                        }
                    }
                    else
                    {
                        //Находим минимальный незаполненный элемент в столбце, где макс штраф
                        for (int i = 1; i < Tariff.GetLength(0) - 1; i++)
                        {
                            if (MinEl > Tariff[i, IndexEl] && Check[i, IndexEl] == 0)
                            {
                                MinEl = Tariff[i, IndexEl];
                            }
                        }
                        //Заполняем ячейку с мин элементом и массив чек
                        for (int i = 1; i < Tariff.GetLength(0) - 1; i++)
                        {
                            if (Check[i, IndexEl] == 0 && Tariff[i, IndexEl] == MinEl)
                            {
                                if (Tariff[0, IndexEl] >= Tariff[i, 0])
                                {
                                    Plan[i, IndexEl] = Tariff[i, 0];
                                    Tariff[0, IndexEl] -= Tariff[i, 0];
                                    Tariff[i, 0] -= Tariff[i, 0];
                                }
                                else if (Tariff[0, IndexEl] < Tariff[i, 0])
                                {
                                    Plan[i, IndexEl] = Tariff[0, IndexEl];
                                    Tariff[i, 0] -= Tariff[0, IndexEl];
                                    Tariff[0, IndexEl] -= Tariff[0, IndexEl];
                                }
                                Check[i, IndexEl] = 1;
                            }
                        }
                    }
                    //Если склад или потреб стали == 0, то заполняем столбец/строку 0.
                    for (int i = 1; i < Tariff.GetLength(0) - 1; i++)
                    {
                        if (Tariff[i, 0] == 0)
                        {
                            for (int j = 1; j < Tariff.GetLength(1) - 1; j++)
                            {
                                if (Check[i, j] == 0)
                                {
                                    Plan[i, j] = 0;
                                    Check[i, j] = 1;
                                }

                            }
                            //Больше не учитываем штрафы в заполненном столбце
                            Check[i, Tariff.GetLength(1) - 1] = 1;
                        }
                    }
                    for (int j = 1; j < Tariff.GetLength(1) - 1; j++)
                    {
                        if (Tariff[0, j] == 0)
                        {
                            for (int i = 1; i < Tariff.GetLength(0) - 1; i++)
                            {
                                if (Check[i, j] == 0)
                                {
                                    Plan[i, j] = 0;
                                    Check[i, j] = 1;
                                }

                            }
                            //Больше не учитываем штрафы в заполненном столбце
                            Check[Tariff.GetLength(0) - 1, j] = 1;
                        }
                    }
                    SumCheck = 0;
                    //Проверка на количество заполненных элементов
                    for (int i = 1; i < Check.GetLength(0) - 1; i++)
                    {
                        for (int j = 1; j < Check.GetLength(1) - 1; j++)
                        {
                            SumCheck += Check[i, j];
                        }
                    }
                }
                int[,] newTariff = new int[N, M];
                for (int i = 0; i < Tariff.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j < Tariff.GetLength(1) - 1; j++)
                    {
                        newTariff[i, j] = Tariff[i, j];
                    }
                }
                PrintPlan(Plan, Arr); //Вывод тарифного плана
                OutputAnswer(Plan, newTariff); //Вывод целевой функции и проверка на вырожденность   
            }
        }
    }
}
