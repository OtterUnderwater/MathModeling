
using System.Collections.Generic;

namespace ConsoleApp1
{
	class Control
	{
		static void Main()
		{
			try
			{
				int n;
				ConsoleKeyInfo end;
				do
				{
					Console.Clear();
					Console.WriteLine("Выберите нужную задачу: ");
					Console.WriteLine("1. Транспортные задачи");
					Console.WriteLine("2. Симплекс метод");
					Console.WriteLine("3. Задача коммивояжера");
					Console.WriteLine("4. Алгоритм Дейкстры");
					n = Convert.ToInt32(Console.ReadLine());
					switch (n)
					{
						case 1: СallMetods(); break;
						case 2: СallSimpleTable(); break;
						case 3: СallTSP(); break;
						case 4: СallDijkstra(); break;
						default: Console.WriteLine("Такой задачи нет"); break;
					}
					Console.WriteLine("\n Нажмите Esc для завершения");
					end = Console.ReadKey();
				} while (end.Key != ConsoleKey.Escape);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Тип ошибки: {e.GetType().Name}");
				Console.WriteLine($"Строка: {e.StackTrace}");
			}
		}
		static void СallMetods()
		{
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
			int[,] Tariff = new int[N + 1, M + 1]; //Заполняем поставки и потребителей
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
			Console.WriteLine("Введите матрицу тарифов:");
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
			Methods method = new Methods(Tariff);
			int n;
			ConsoleKeyInfo end;
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
					case 1: method.MNorthwestCorner(Tariff); break;
					case 2: method.MMinimumCost(Tariff); break;
					case 3: method.MDoublePreference(Tariff); break;
					case 4: method.MFogelApproximations(Tariff); break;
					default: Console.WriteLine("Такого метода нет"); break;
				}
				Console.WriteLine("\nНажмите Esc для выхода в меню");
				end = Console.ReadKey();
			} while (end.Key != ConsoleKey.Escape);
		}
		static void СallSimpleTable()
		{
			Console.WriteLine("Введите количество базисных переменных:");
			int countBasis = Convert.ToInt32(Console.ReadLine()); //базисные = стр уравнений
			Console.WriteLine("Введите количество всех переменных:");
			int countX = Convert.ToInt32(Console.ReadLine());
			decimal[,] STable = new decimal[countBasis + 2, countX + 2];
			// Заполняем номера X в первой строке 
			for (int j = 0; j < STable.GetLength(1); j++)
			{
				STable[0, j] = j;
			}
			// Цикл по строкам
			int basis = (countX - countBasis) + 1;
			for (int i = 1; i < STable.GetLength(0); i++)
			{
				if (i != STable.GetLength(0) - 1)
				{
					Console.WriteLine($"Введите значение уравнения {i} через пробел:");
					string buffer = Console.ReadLine();
					decimal[] temp = new decimal[STable.GetLength(1)];
					temp = buffer.Split(" ").Select(decimal.Parse).ToArray(); //Переводим в массив double
					STable[i, 0] = basis;
					basis++;
					for (int j = 1, t = 0; j < STable.GetLength(1); j++, t++)
					{
						STable[i, j] = temp[t];
					}
				}
				else //Строка дельта j
				{
					Console.WriteLine("Введите уравнение L(x) через пробел:");
					string buffer = Console.ReadLine();
					int N = buffer.Split(" ").Length;
					decimal[] temp = new decimal[N];
					temp = buffer.Split(" ").Select(decimal.Parse).ToArray();
					for (int j = 1, t = 0; j < STable.GetLength(1); j++, t++)
					{
						if (t < N)
						{
							STable[i, j] = -temp[t];
						}
						else
						{
							STable[i, j] = 0;
						}
					}
				}
			}
			SimpleTable ST = new SimpleTable(STable, countX - countBasis);
			Console.WriteLine("Для чего используем симплекс-метод:");
			Console.WriteLine("1. Максимальное значение целевой функции");
			Console.WriteLine("2. Минимальное значение целевой функции");
			int n = Convert.ToInt32(Console.ReadLine());
			switch (n)
			{
				case 1: ST.MaxObjectiveFunction(); break;
				case 2: ST.MinObjectiveFunction(); break;
				default: Console.WriteLine("Такого варианта нет"); break;
			}
		}
		static void СallTSP()
        {
            Console.WriteLine("Введите количество пунктов:");
            int N = Convert.ToInt32(Console.ReadLine()) + 1;
            int[,] matrix = new int[N, N];
            // Нумеруем пункты
            for (int i = 0; i < matrix.GetLength(0); i++)
            { 
				matrix[i,0] = i; 
			}
            for (int j = 0; j < matrix.GetLength(1); j++)
            { 
				matrix[0, j] = j; 
			}
            Console.WriteLine("Введите матрицу:");
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                string buffer = Console.ReadLine();
				string[] temp = new string[N-1];
                temp = buffer.Split(" ").ToArray();
                for (int j = 1, t = 0; j < matrix.GetLength(1); j++, t++)
                {
					if (temp[t] == "M")
					{
						matrix[i, j] = Int32.MaxValue;
					}
					else
					{
						matrix[i, j] = Convert.ToInt32(temp[t]);
                    }
                }
            }
            TravellingSalesmanProblem task = new TravellingSalesmanProblem(matrix);
			task.LittsAlgorithm();
        }
		static void СallDijkstra()
		{
			Console.WriteLine("Введите количество точек:");
			int countPoints = Convert.ToInt32(Console.ReadLine());
			Console.WriteLine("Введите номер начальной точки:");
			int startPoint = Convert.ToInt32(Console.ReadLine());
			Console.WriteLine("Введите ребра с весом через пробел: (0 для выхода)");
			List<(int, int, int)> ribs = new List<(int, int, int)>();	
			string buffer;
			while ((buffer = Console.ReadLine()) != "0")
			{
				int[] temp = buffer.Split(" ").Select(int.Parse).ToArray();
				ribs.Add((temp[0], temp[1], temp[2]));
			}
			DijkstrasAlgorithm dijkstrasAlgorithm = new DijkstrasAlgorithm();
			dijkstrasAlgorithm.ShortestPaths(countPoints, startPoint, ribs);
		}
	}
}