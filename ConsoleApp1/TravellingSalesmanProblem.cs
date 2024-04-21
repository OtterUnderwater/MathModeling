﻿using ConsoleApp1.models;

namespace ConsoleApp1
{
	public class TravellingSalesmanProblem
	{
		const int M = Int32.MaxValue;
		int[,] matrix;
		//Лист следящий за всеми конечными путями
		List<Track> tracks = new List<Track>();

		/// <summary>
		/// Конструктор для вывода и заполнения массиыв
		/// </summary>
		/// <param name="matrix"></param>
		public TravellingSalesmanProblem(int[,] matrix)
		{
			this.matrix = matrix;
			PrintTable();
		}

		/// <summary>
		/// Вывод введенной матрицы
		/// </summary>
		private void PrintTable()
		{
			Console.WriteLine("Матрица с исходными данными:");
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					if (matrix[i, j] == M)
					{
						Console.Write("   M");
					}
					else
					{
						Console.Write("{0,4}", matrix[i, j]);
					}
				}
				Console.WriteLine();
			}
		}
		
		/// <summary>
		/// Вывод оптимального значения и цикла в графе
		/// </summary>
		private void PrintWayAndCosts(Track way)
		{
			List<int> startWay = new List<int>();
			List<int> endWay = new List<int>();
			//Заполняем список начала и конца пути
			for (int i = 0; i < way.Way.Count; i++)
			{
				((i % 2 == 0) ? startWay : endWay).Add(way.Way[i]);
			}
			List<int> cycle = new List<int>
			{
				startWay[0],
				endWay[0]
			};
			startWay.Remove(startWay[0]);
			endWay.Remove(endWay[0]);
			while (startWay.Count > 0 && endWay.Count > 0)
			{
				int index = startWay.IndexOf(cycle[cycle.Count - 1]);
				cycle.Add(endWay[index]);
				startWay.Remove(startWay[index]);
				endWay.Remove(endWay[index]);
			}
			string result = string.Join(" -> ", cycle); 
			Console.WriteLine($"Путь: {result}");
			Console.WriteLine($"Оптимальное значение = {way.Costs}");
		}

		/// <summary>
		/// Алгоритм Литта
		/// </summary>
		public void LittsAlgorithm()
		{
			//Корневая нижняя граница H0
			int H = GetSumReduction(ref matrix);
			//Условие продолжения цикла
			bool checkLength = true;
			//Лист следящий за всеми конечными путями
			tracks.Add(new Track(null, null, H)); //Текущий путь
			int head = 0; //Указатель на ветку
			List<int> way = new List<int>();
			List<int> noway = new List<int>();
			while (checkLength)
			{
				//Оценка нулевых ячеек (Находим макс штраф)
				(int, int, int) maxСellNullScore = ZeroСellЕvaluation();

				//Не включаем ветку (trackNo)
				int[,] newMatrixNo = CopyMatrix(matrix); //Копируем
				newMatrixNo[maxСellNullScore.Item1, maxСellNullScore.Item2] = M; //Удаляем вариант пути
				MakeReduction(ref newMatrixNo); //Редукция
				//Подсчет нижней локальной границы
				int HLocalNo = (maxСellNullScore.Item3 != M) ? (H + maxСellNullScore.Item3): M;
				noway = way.ToList();
				Track trackNo = new Track(noway, newMatrixNo, HLocalNo);

				//Включаем ветку (trackYes)
				//Запоминаем значение пути
				int nameStart = matrix[maxСellNullScore.Item1, 0];
				int nameEnd = matrix[0, maxСellNullScore.Item2];
				//Добавляем Начало и конец пути
				way.Add(nameStart);
				way.Add(nameEnd);
				//Новая уменьшенная матрица
				matrix = ReducingMatrix(matrix, maxСellNullScore.Item1, maxСellNullScore.Item2);
				//Проверяем, что обратный путь существует
				int indEnd = 0, indStart = 0;
				CheckExistenceReturnPath(out indStart, out indEnd, nameStart, nameEnd);
				//Запрещаем возвращение
				if (indStart != 0 && indEnd != 0)
				{
					matrix[indEnd, indStart] = M;
				}
				//Подсчет нижней локальной границы
				int HLocal = H + GetSumReduction(ref matrix);
				Track trackYes = new Track(way, CopyMatrix(matrix), HLocal);

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
				SetTrackHead(head, minWay); //Изменяем существующий путь
				//Смотрим есть ли другие ветки с меньшей стоимостью
				if (CheckFindMinBranch(ref head))
				{
					//Перемещаемся на другую ветку
					way = tracks[head].Way.ToList(); //Вспоминаем путь (копируем)
					matrix = CopyMatrix(tracks[head].Matrix); //Копируем матрицу
					H = tracks[head].Costs; //Меняем цену H
				}
				else
				{
					SetTrackHead(head, minWay); //Изменяем путь
					//Изменяем переменные
					way = minWay.Way.ToList();
					matrix = CopyMatrix(minWay.Matrix);
					H = minWay.Costs;
				}
				//Цикл продолжается до тех пор, пока размер матрицы не станет == 2
				if (matrix.GetLength(0) == 2)
				{
					checkLength = false;
					//Добавляем конец пути
					tracks[head].Way.Add(matrix[1, 0]);
					tracks[head].Way.Add(matrix[0, 1]);
				}
			}
			PrintWayAndCosts(tracks[head]);
		}

		/// <summary>
		/// Проверка есть ли другие ветки с меньшей ценой
		/// </summary>
		/// <param name="head"></param>
		/// <param name="costs"></param>
		/// <returns></returns>
		private bool CheckFindMinBranch(ref int head)
		{
			for (int i = 0; i < tracks.Count; i++)
			{
				if (i != head && tracks[i].Costs < tracks[head].Costs)
				{
					head = i; //переносим указатель
					return true; //Перемещаемся
				}
			}
			return false; //Остаемся на этой ветке
		}

		/// <summary>
		/// Устанавливает значения текущей ветки
		/// </summary>
		/// <param name="head">указатель на ветку</param>
		/// <param name="way">путь</param>
		/// <param name="matrix">матрица</param>
		/// <param name="costs">стоимость</param>
		private void SetTrackHead(int head, Track needWay)
		{
			tracks[head].Way = needWay.Way;
			tracks[head].Matrix = CopyMatrix(needWay.Matrix);
			tracks[head].Costs = needWay.Costs;
		}

		/// <summary>
		/// Проверка существования обратного пути и возвращение индексов
		/// </summary>
		/// <param name="indStart"></param>
		/// <param name="indEnd"></param>
		/// <param name="NameStart"></param>
		/// <param name="NameEnd"></param>
		private void CheckExistenceReturnPath(out int indStart, out int indEnd, int NameStart, int NameEnd)
		{
			indEnd = 0;
			indStart = 0;
			for (int i = 1; i < matrix.GetLength(0); i++)
			{
				if (matrix[i, 0] == NameEnd)
				{
					indEnd = i;
				}
			}
			for (int j = 1; j < matrix.GetLength(1); j++)
			{
				if (matrix[0, j] == NameStart)
				{
					indStart = j;
				}
			}
		}

		/// <summary>
		/// Оценка нулевых ячеек
		/// </summary>
		/// <returns></returns>
		private (int, int, int) ZeroСellЕvaluation()
		{
			//Лист кортежей i, j, оценка
			(int, int, int) maxСellNullScore = (0, 0, 0);
			int sum = 0;
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
						if (minStr != M && minColumn != M)
						{
							sum = minStr + minColumn;
						}
						else
						{
							sum = M;
						}
						if (sum > maxСellNullScore.Item3)
						{
							maxСellNullScore = (i, j, sum);
						}
					}
				}
			}
			return maxСellNullScore;
		}

		/// <summary>
		/// Уменьшение матрицы на строку и столбец
		/// </summary>
		/// <param name="arrayM"></param>
		/// <returns></returns>
		private int[,] ReducingMatrix(int[,] arrayM, int indexI, int indexJ)
		{
			int[,] newMatrix = new int[arrayM.GetLength(0) - 1, arrayM.GetLength(1) - 1];
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
		private int[,] CopyMatrix(int[,] arrayM)
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
		/// Редукция без возврата суммы редукций
		/// </summary>
		/// <param name="arrayM"></param>
		private void MakeReduction(ref int[,] arrayM) => GetSumReduction(ref arrayM);

		/// <summary>
		/// Делает редукцию по строкам и столбцам
		/// </summary>
		/// <returns>Возвращает сумму редукци di и dj</returns>
		private int GetSumReduction(ref int[,] arrayM) => GetReductionStroke(ref arrayM) + GetReductionColumn(ref arrayM);

		/// <summary>
		/// Редукция по строкам
		/// </summary>
		/// <returns></returns>
		private int GetReductionStroke(ref int[,] arrayM)
		{
			int min;
			int di = 0;
			for (int i = 1; i < arrayM.GetLength(0); i++)
			{
				min = Int32.MaxValue;
				for (int j = 1; j < arrayM.GetLength(1); j++)
				{
					if (arrayM[i, j] < min)
					{
						min = arrayM[i, j];
					}
				}
				if (min > 0)
				{
					for (int j = 1; j < arrayM.GetLength(1); j++)
					{
						if (arrayM[i, j] != M)
						{
							arrayM[i, j] = arrayM[i, j] - min;
						}
					}
					//Сумма
					di += min;
				}
			}
			return di;
		}

		/// <summary>
		/// Редукция по столбцам
		/// </summary>
		/// <returns></returns>
		private int GetReductionColumn(ref int[,] arrayM)
		{
			int min;
			int dj = 0;
			for (int j = 1; j < arrayM.GetLength(1); j++)
			{
				min = Int32.MaxValue;
				for (int i = 1; i < arrayM.GetLength(0); i++)
				{
					if (arrayM[i, j] < min)
					{
						min = arrayM[i, j];
					}
				}
				if (min > 0)
				{
					for (int i = 1; i < arrayM.GetLength(0); i++)
					{
						if (arrayM[i, j] != M)
						{
							arrayM[i, j] = arrayM[i, j] - min;
						}
					}
					//Сумма
					dj += min;
				}
			}
			return dj;
		}
	}
}
