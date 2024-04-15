namespace ConsoleApp1
{
	class Znach
	{
		public int Index;
		public decimal Value;
		public Znach(int index, decimal znach)
		{
			Index = index;
			Value = znach;
		}
	}

	public class SimpleTable
	{
		decimal[,] sTable; //Симплекс-таблица
		int countX; //Количество неизвестных в системе (искомые x)

		public SimpleTable(decimal[,] STable, int countX)
		{
			this.sTable = STable;
			this.countX = countX;
			Console.WriteLine("Симплекс-таблица:");
			PrintSimpleTable();
		}

		/// <summary>
		/// Вывод симплекс-таблицы
		/// </summary>
		public void PrintSimpleTable()
		{
			for (int i = 1; i < sTable.GetLength(0); i++)
			{
				for (int j = 1; j < sTable.GetLength(1); j++)
				{
					Console.Write("{0,6:0.0}", sTable[i, j]);
				}
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Поиск максимального значения L(x)
		/// </summary>
		public void MaxObjectiveFunction()
		{
			List<decimal> deltaJ = GetDeltaJ();
			//Работает, пока в строке дельта j есть значения < 0
			while (deltaJ.Any(x => x < 0))
			{
				// Определяем ведущий столбец
				decimal minElem = sTable[sTable.GetLength(0) - 1, 1];
				int indexColumn = 1;
				int indexStroke = 0;
				for (int j = 1; j < sTable.GetLength(1) - 1; j++)
				{
					if (sTable[sTable.GetLength(0) - 1, j] < minElem)
					{
						minElem = sTable[sTable.GetLength(0) - 1, j];
						indexColumn = j;
					}
				}
				// Определяем ведущую строку
				indexStroke = GetIndexStroke(indexColumn);
				// Заменяем название базиса
				sTable[indexStroke, 0] = sTable[0, indexColumn];
				// Вычисляем новую симплекс-таблицу методом Жордана-Гаусса
				MethodJordanGauss(indexStroke, indexColumn);
				// Проверяем deltaJ
				deltaJ = GetDeltaJ();
			}
			// Считаем L(x), а далее выводим значения базисов
			PrintObjectiveFunction();
		}

		/// <summary>
		/// Поиск миниального значения L(x)
		/// </summary>
		public void MinObjectiveFunction()
		{
			List<decimal> deltaJ = GetDeltaJ();
			//Работает, пока в строке дельта j есть значения < 0
			while (deltaJ.Any(x => x > 0))
			{
				// Определяем ведущий столбец
				decimal maxElem = sTable[sTable.GetLength(0) - 1, 1];
				int indexColumn = 1;
				int indexStroke = 0;
				for (int j = 1; j < sTable.GetLength(1) - 1; j++)
				{
					if (sTable[sTable.GetLength(0) - 1, j] > maxElem)
					{
						maxElem = sTable[sTable.GetLength(0) - 1, j];
						indexColumn = j;
					}
				}
				indexStroke = GetIndexStroke(indexColumn);
				sTable[indexStroke, 0] = sTable[0, indexColumn];
				MethodJordanGauss(indexStroke, indexColumn);
				deltaJ = GetDeltaJ();
			}
			PrintObjectiveFunction();
		}

		/// <summary>
		/// Возвращает лист дельты J
		/// </summary>
		/// <returns></returns>
		public List<decimal> GetDeltaJ()
		{
			List<decimal> deltaJ = new List<decimal>();
			for (int j = 1; j < sTable.GetLength(1) - 1; j++)
			{
				deltaJ.Add(sTable[sTable.GetLength(0) - 1, j]);
			}
			return deltaJ;
		}

		/// <summary>
		/// Возвращает индекс ведущей строки
		/// (минимальное положительное число при делении L(x) на ведущий столбец)
		/// </summary>
		/// <param name="indexColumn"></param>
		/// <returns></returns>
		public int GetIndexStroke(int indexColumn)
		{
			decimal L;
			decimal leaderX;
			decimal otb;
			List<Znach> result = new List<Znach>(); // Хранит в себе индекс строки и значение результата
			for (int i = 1; i < sTable.GetLength(0) - 1; i++)
			{
				otb = 0;
				L = sTable[i, sTable.GetLength(1) - 1];
				leaderX = sTable[i, indexColumn];
				if (leaderX != 0)
				{
					otb = L / leaderX;
				}
				if (otb > 0)
				{
					//Запись всех положительных значений в лист
					result.Add(new Znach(i, otb));
				}
			}
			//Aggregate: применяет к элементам последовательности агрегатную функцию, которая сводит их к одному объекту
			int index = result.Aggregate((x, y) => x.Value < y.Value ? x : y).Index; //Индекс минимального элемента в списке
			return index;
		}

		/// <summary>
		/// Использование метода Жордана-Гаусса на матрице
		/// </summary>
		public void MethodJordanGauss(int iLead, int jLead)
		{
			// Получаем в нужной строке 1 для ведущего элемента
			// Делим всю строку на элемент
			decimal xLead = sTable[iLead, jLead];
			for (int j = 1; j < sTable.GetLength(1); j++)
			{
				sTable[iLead, j] = sTable[iLead, j] / xLead;
			}
			// Зануляем все остальные элементы в столбце
			for (int i = 1; i < sTable.GetLength(0); i++)
			{
				//Ведущую строку больше не изменяем
				if (i != iLead)
				{
					decimal elJLead = -sTable[i, jLead];
					for (int j = 1; j < sTable.GetLength(1); j++)
					{
						sTable[i, j] = sTable[i, j] + (sTable[iLead, j] * elJLead);
					}
				}
			}
		}

		/// <summary>
		/// Вывод целевой функции и неизвестных
		/// </summary>
		public void PrintObjectiveFunction()
		{
			List<Znach> result = new List<Znach>();
			//Проходимся по всему столбцу x и ищем неизвестные
			for (int x = 1; x <= countX; x++)
			{
				for (int i = 1; i < sTable.GetLength(0) - 1; i++)
				{
					if (sTable[i, 0] == x)
					{
						result.Add(new Znach(x, sTable[i, sTable.GetLength(1) - 1]));
						continue;
					}
				}
			}
			for (int i = 0; i < countX; i++)
			{
				if (result[i].Index == i+1)
				{
					Console.WriteLine("x{0} = {1:0.0}", i + 1, result[i].Value);
				}
				else
				{
					Console.WriteLine($"x{i + 1} = 0");
				}
			}
			Console.WriteLine("L(x) = {0:0.00}", sTable[sTable.GetLength(0) - 1, sTable.GetLength(1) - 1]);
		}
	}
}
