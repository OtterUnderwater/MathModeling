using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

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
		decimal[,] STable;

		public SimpleTable(decimal[,] STable)
		{
			this.STable = STable;
			Console.WriteLine("Симплекс-таблица:");
			PrintSimpleTable();
		}

		public void PrintSimpleTable()
		{
			for (int i = 0; i < STable.GetLength(0); i++)
			{
				for (int j = 0; j < STable.GetLength(1); j++)
				{
					Console.Write("{0,6:0.0}", STable[i, j]);
				}
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Поиск максимального значения L(x)
		/// </summary>
		public void MaxObjectiveFunction()
		{
			List<decimal> deltaJ = new List<decimal>();
			for (int j = 1; j < STable.GetLength(1) - 1; j++)
			{
				deltaJ.Add(STable[STable.GetLength(0) - 1, j]);
			}
			//Работает, пока в строке дельта j есть значения < 0
			while (deltaJ.Any(x => x < 0))
			{
				// Определяем ведущий столбец
				decimal minElem = STable[STable.GetLength(0) - 1, 1];
				int indexColumn = 1;
				int indexStroke = 0;
				for (int j = 1; j < STable.GetLength(1) - 1; j++)
				{
					if (STable[STable.GetLength(0) - 1, j] < minElem)
					{
						minElem = STable[STable.GetLength(0) - 1, j];
						indexColumn = j;
					}
				}
				// Определяем ведущую строку
				indexStroke = GetIndexStroke(indexColumn);
				// Заменяем название базиса
				STable[indexStroke, 0] = STable[0, indexColumn];
				// Вычисляем новую симплекс-таблицу методом Жордана-Гаусса

				}
			// Решение найдено
			// Считаем L(x), а далее выводим знач базисов x1, x2...
		}

		/// <summary>
		/// Поиск миниального значения L(x)
		/// </summary>
		public void MinObjectiveFunction()
		{
			List<decimal> deltaJ = new List<decimal>();
			for (int j = 1; j < STable.GetLength(1) - 1; j++)
			{
				deltaJ.Add(STable[STable.GetLength(0) - 1, j]);
			}
			//Работает, пока в строке дельта j есть значения > 0
			while (deltaJ.Any(x => x > 0))
			{
				// Определяем ведущий столбец
				decimal maxElem = STable[STable.GetLength(0) - 1, 1];
				int indexColumn = 1;
				int indexStroke = 0;
				for (int j = 1; j < STable.GetLength(1) - 1; j++)
				{
					if (STable[STable.GetLength(0) - 1, j] > maxElem)
					{
						maxElem = STable[STable.GetLength(0) - 1, j];
						indexColumn = j;
					}
				}
				indexStroke = GetIndexStroke(indexColumn);
				// Определяем ведущую строку
				indexStroke = GetIndexStroke(indexColumn);
				// Заменяем название базиса
				STable[indexStroke, 0] = STable[0, indexColumn];
			}
		}

		/// <summary>
		/// Возвращает индекс ведущей строки
		/// (минимальное положительное число при делении L(x) на ведущий столбец)
		/// </summary>
		/// <param name="indexColumn"></param>
		/// <returns></returns>
		public int GetIndexStroke(int indexColumn)
		{
			List<Znach> result = new List<Znach>();
			for (int i = 1; i < STable.GetLength(0) - 1; i++)
			{
				decimal L = STable[i, STable.GetLength(1) - 1];
				decimal leaderX = STable[i, indexColumn];
				decimal otb = L / leaderX;
				if (otb > 0)
				{
					result.Add(new Znach(i, otb));
				}
			}
			//Aggregate: применяет к элементам последовательности агрегатную функцию, которая сводит их к одному объекту
			int index = result.Aggregate((x, y) => x.Value < y.Value ? x : y).Index; //Индекс минимального элемента в списке
			return index;
		}

	}

}
