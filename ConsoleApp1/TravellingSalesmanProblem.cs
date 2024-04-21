
using ConsoleApp1.models;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

		/// <summary>
		/// Вывод введенной матрицы
		/// </summary>
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

		/// <summary>
		/// Алгоритм Литта
		/// </summary>
		public void LittsAlgorithm()
		{
			//Корневая нижняя граница H0
			int H = GetSumReduction(matrix);
			Console.WriteLine(H);
			//Условие продолжения цикла
			bool checkLength = true;
			//Лист следящий за всеми конечными путями
			List<Track> tracks = new List<Track>
			{
				new Track(null, null, H) //Текущий путь
			};
			int head = 0; //Указатель на ветку
			List<int> way = new List<int>();
			int CostWay = 0;
			while (checkLength)
			{
				//Лист кортежей i,j,оценка
				(int, int, int) maxСellNullScore = (0, 0, 0);
				//Оценка нулевых ячеек
				for (int i = 1; i < matrix.GetLength(0); i++)
				{
					for (int j = 1; j < matrix.GetLength(1); j++)
					{
						int minStr = Int32.MaxValue;
						int minColumn = Int32.MaxValue;
						if (matrix[i, j] == 0)
						{
							//Поиск минимального элемента в строке и столбце
							for (int i1 = 1; i1 < matrix.GetLength(0); i1++)
							{
								//Не учитываем саму нулевую клетку
								if (i1 != i)
								{
									if (matrix[i1, j] < minColumn)
									{
										minColumn = matrix[i1, j];
									}
								}
							}
							for (int j1 = 1; j1 < matrix.GetLength(1); j1++)
							{
								//Не учитываем саму нулевую клетку
								if (j1 != j)
								{
									if (matrix[i, j1] < minStr)
									{
										minStr = matrix[i, j1];
									}
								}
							}
							//Поиск максимальной оценки
							int sum = minStr + minColumn;
							if (sum > maxСellNullScore.Item3)
							{
								maxСellNullScore = (i, j, sum);
							}
						}
					}
				}
				//Нашли максимальную оценку (наш маршрут)
				//Не включаем ветку (trackNo)
				int[,] newMatrixNo = CopyMatrix(matrix); //Копируем матрицу
				newMatrixNo[maxСellNullScore.Item1, maxСellNullScore.Item2] = M; //Удаляем вариант пути
				//Подсчет нижней локальной границы
				int HLocalNo = H + maxСellNullScore.Item3;
				Track trackNo = new Track(way, newMatrixNo, HLocalNo);

				//Включаем ветку (trackYes)
				//Добавляем Начало и конец пути
				way.Add(matrix[maxСellNullScore.Item1, 0]);
				way.Add(matrix[0, maxСellNullScore.Item2]);
				//Подсчет нижней локальной границы
				int HLocal = H + GetSumReduction(matrix);
				//Сначала запрещаем возвращение (т.к. i И j к этой матрице)
				matrix[maxСellNullScore.Item2, maxСellNullScore.Item1] = M;
				//Новая уменьшенная матрица
				int[,] newMatrix = ReducingMatrix(matrix, maxСellNullScore.Item1, maxСellNullScore.Item2);
				Track trackYes = new Track(way, newMatrix, HLocal);

				//ВЫБИРАЕМ ПУТЬ
				//Остаемся на минимальном пути и записываем другой в лист
				Track minWay = trackNo;
				if (trackNo.Costs <= trackYes.Costs)
				{
					minWay = trackNo;
					tracks.Add(trackYes);
				}
				else
				{
					minWay = trackYes;
					tracks.Add(trackNo);
				}
				//Смотрим есть ли другие ветки с меньшей стоимостью
				bool reset = false; //Остаемся на этой ветке
				for (int i = 0; i < tracks.Count; i++)
				{
					//Перемещаемся на другую ветку
					if (i != head && tracks[i].Costs < minWay.Costs)
					{
						reset = true;
						head = i; //переносим указатель
						break;
					}
				}
				if (reset)
				{
					//Перемещаемся на другую ветку
					way = tracks[head].Way.ToList(); //Вспоминаем путь (копируем)
					matrix = CopyMatrix(tracks[head].Matrix); //Копируем матрицу
					H = tracks[head].Costs; //Меняем цену H
					CostWay = tracks[head].Costs;
				}
				else
				{
					//Изменяем существующий путь
					tracks[head].Way = minWay.Way;
					tracks[head].Matrix = CopyMatrix(minWay.Matrix);
					tracks[head].Costs = minWay.Costs;
					//Изменяем переменные
					way = minWay.Way;
					matrix = CopyMatrix(minWay.Matrix);
					H = minWay.Costs;
					CostWay = minWay.Costs;
				}
				//Цикл продолжается до тех пор, пока размер матрицы не станет == 2
				if (matrix.GetLength(0) == 2)
				{
					checkLength = false;
				}
			}
			Console.WriteLine(CostWay);
		}

		/// <summary>
		/// Уменьшение матрицы на строку и столбец
		/// </summary>
		/// <param name="arrayM"></param>
		/// <returns></returns>
		public int[,] ReducingMatrix(int[,] arrayM, int indexI, int indexJ)
		{
			int[,] newMatrix = new int[arrayM.GetLength(0)-1, arrayM.GetLength(1)-1];
			for (int i = 0, k = 0; i < arrayM.GetLength(0); i++)
			{
				if (i != indexI)
				{
					for (int j = 0, m = 0; j < arrayM.GetLength(1); j++)
					{
						if (j != indexJ)
						{
							newMatrix[k, m] = arrayM[i, j];
							m++;
						}
					}
					k++;
				}
			}
			return newMatrix;
		}

		/// <summary>
		/// Копирование матрицы (для избежания проблемы ссылок)
		/// </summary>
		/// <param name="arrayM"></param>
		/// <returns></returns>
		public int[,] CopyMatrix(int[,] arrayM)
		{
			int[,] newMatrix = new int[arrayM.GetLength(0), arrayM.GetLength(1)];
			for (int i = 0; i < arrayM.GetLength(0); i++)
			{
				for (int j = 0; j < arrayM.GetLength(1); j++)
				{
					newMatrix[i, j] = arrayM[i, j];
				}
			}
			return newMatrix;
		}

		/// <summary>
		/// Делает редукцию по строкам и столбцам
		/// </summary>
		/// <returns>Возвращает сумму редукци di и dj</returns>
		public int GetSumReduction(int[,] arrayM) => GetReduction(0, 1, arrayM) + GetReduction(1, 0, arrayM);

		/// <summary>
		/// Редукция по строкам и столбцам
		/// </summary>
		/// <param name="forOne">Индекс длины первого цикла</param>
		/// <param name="forTwo">Индекс длины второго цикла</param>
		/// <returns>Сумма di или dj</returns>
		public int GetReduction(int forOne, int forTwo, int[,] arrayM)
		{
			int min = Int32.MaxValue;
			int dij = 0;
			for (int i = 1; i < arrayM.GetLength(forOne); i++)
			{
				min = Int32.MaxValue;
				for (int j = 1; j < arrayM.GetLength(forTwo); j++)
				{
					if (arrayM[i, j] < min)
					{
						min = arrayM[i, j];
					}
				}
				if (min > 0)
				{
					for (int j = 1; j < arrayM.GetLength(forTwo); j++)
					{
						if (arrayM[i, j] != M)
						{
							arrayM[i, j] = arrayM[i, j] - min;
						}
					}
					//Сумма
					dij += min;
				}
			}
			return dij;
		}
	}
}
