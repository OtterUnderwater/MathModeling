
namespace ConsoleApp1
{
	public class Methods
	{
		int[,] referencePlan;
		int[,] tariff;

		public Methods(int[,] tariff)
		{
			this.tariff = new int[tariff.GetLength(0), tariff.GetLength(1)];
			this.tariff = tariff;
		}

		/// <summary>
		/// Проверка на открытость задачи
		/// </summary>
		/// <param name="Array">Введенные тарифы</param>
		/// <returns> "true" если задача закрытая </returns>
		bool CheckClosed(int[,] Array)
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

		/// <summary>
		/// Поиск максимального элемента массива
		/// </summary>
		/// <param name="Array">Изменяющийся массив тарифов</param>
		/// <returns>Максимальный элемент</returns>
		int FindMaxEl(int[,] Array)
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

		/// <summary>
		/// Проверка что все ячейки заполнены
		/// </summary>
		/// <param name="Check">Массив проверки</param>
		/// <returns>Количество заполненных ячеек</returns>
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

		/// <summary>
		/// Устанавливает значения массиву referencePlan (опорный план)
		/// </summary>
		/// <param name="Plan">Опорный план</param>
		/// <param name="Arr">Потребители и Склады</param>
		void SetReferencePlan(int[,] Plan)
		{
			referencePlan = new int[Plan.GetLength(0), Plan.GetLength(1)];
			for (int i = 0; i < Plan.GetLength(0); i++)
			{
				for (int j = 0; j < Plan.GetLength(1); j++)
				{
					if (j == 0 || i == 0)
					{
						referencePlan[i, j] = tariff[i, j];
					}
					else
					{
						referencePlan[i, j] = Plan[i, j];
					}
				}
			}
		}

		/// <summary>
		/// Изменяет значение массиву referencePlan (опорный план)
		/// </summary>
		void InputReferencePlan()
		{
			Console.WriteLine();
			string str;
			int[] temp;
			PrintArray(tariff);
			Console.WriteLine("Введите перераспределенный опорный план:");
			for (int i = 1; i < referencePlan.GetLength(0); i++)
			{
				str = Console.ReadLine();
				temp = new int[str.Split(" ").Length];
				temp = str.Split(" ").Select(int.Parse).ToArray();
				for (int j = 1, t = 0; j < referencePlan.GetLength(1); j++, t++)
				{
					referencePlan[i, j] = temp[t];
				}
			}
		}

		/// <summary>
		/// Красивый вывод матрицы тарифов и опорного плана
		/// </summary>
		/// <param name="arrayNum">Введенные пользователем тарифы или опорный план</param>
		void PrintArray(int[,] arrayNum)
		{
			int n = 1 + 6 * arrayNum.GetLength(1); //сколько нужно тире
			string str = new string('-', n);
			Console.WriteLine("+" + str + "+"); //верхняя рамка таблицы
			for (int i = 0; i < arrayNum.GetLength(0); i++)
			{
				for (int j = 0; j < arrayNum.GetLength(1); j++)
				{
					if (i == 1 && j == 0)
					{
						Console.WriteLine("+" + str + "+"); //рамка, ограничивающая 1ю строку
					}
					if (j == 0)
					{
						Console.Write("|{0,6}|", arrayNum[i, j]); //первый столбец с 2х сторон окружен "|"
					}
					else
					{
						Console.Write("{0,6}", arrayNum[i, j]);
					}
				}
				Console.WriteLine("|"); //переход на следующую строку
			}
			Console.WriteLine("+" + str + "+"); //нижняя рамка таблицы
		}

		/// <summary>
		///  Красивый вывод чисто для метода потенциалов
		/// </summary>
		/// <param name="plan"></param>
		/// <param name="v"></param>
		/// <param name="u"></param>
		void PrintPotentials(int[,] plan, int[] v, int[] u)
		{
			int dash = 2 + 6 * (referencePlan.GetLength(1) + 1); //количество нужных тире
			string str = new string('-', dash);
			Console.WriteLine("+" + str + "+"); //выводим с "+"
			for (int i = 0, i1 = 0; i <= referencePlan.GetLength(0); i++)
			{
				for (int j = 0, j1 = 0; j <= referencePlan.GetLength(1); j++) //столбцы
				{
					if ((i == 1 && j == 0) || (i == referencePlan.GetLength(0) && j == 0))
					{
						Console.WriteLine("+" + str + "+");
					}
					if (i != referencePlan.GetLength(0) && j != referencePlan.GetLength(1))
					{
						if (j == 0)
						{
							Console.Write("|{0,6}|", referencePlan[i, j]);
						}
						else
						{
							Console.Write("{0,6}", referencePlan[i, j]);
						}
					}
					else
					{
						if (j == 0 || i == 0 || (i == referencePlan.GetLength(0) && j == referencePlan.GetLength(1)))
						{
							Console.Write("|      |");
						}
						else
						{
							//последняя строка 
							if (j != referencePlan.GetLength(1) && i == referencePlan.GetLength(0))
							{
								Console.Write("{0,6}", v[j1]);
								j1++;

							}
							//последний столбец
							else if (i != referencePlan.GetLength(0) && j == referencePlan.GetLength(1))
							{

								Console.Write("|{0,6}|", u[i1]);
								i1++;
							}
						}
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine("+" + str + "+"); //типа нижняя рамка таблицы
		}

		/// <summary>
		/// Вывод проверки на вырожденность и подсчет целевой функции
		/// </summary>
		/// <param name="Plan">Опорный план</param>
		/// <param name="Tariff">Тарифы</param>
		void OutputAnswer()
		{
			int V = (tariff.GetLength(0) - 1) + (tariff.GetLength(1) - 1) - 1; //Вырожденность
			int countV = 0; //Проверка на вырожденность
			int L = 0; //Целевая функция  
			for (int i = 1; i < referencePlan.GetLength(0); i++)
			{
				for (int j = 1; j < referencePlan.GetLength(1); j++)
				{
					L = L + (referencePlan[i, j] * tariff[i, j]);
					if (referencePlan[i, j] != 0)
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

		/// <summary>
		/// Метод северно-западного угла
		/// </summary>
		/// <param name="Arr"></param>
		public void MNorthwestCorner(int[,] Arr)
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
			if (CheckClosed(Tariff))
			{
				Console.WriteLine("Тарифы:");
				PrintArray(tariff);
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
				SetReferencePlan(Plan);
				Console.WriteLine("Опорный план:");
				PrintArray(referencePlan); //Вывод тарифного плана
				OutputAnswer(); //Вывод целевой функции и проверка на вырожденность   
				СheckingOptimality(); //Проверка на оптимальность
			}
		}

		/// <summary>
		/// Метод минимальной стоимости
		/// </summary>
		/// <param name="Arr"></param>
		public void MMinimumCost(int[,] Arr)
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
			if (CheckClosed(Tariff))
			{
				PrintArray(tariff);
				while (SumCheck < (N - 1) * (M - 1))
				{
					//Поиск минимального элемента
					int minEl = FindMaxEl(Tariff);
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
				SetReferencePlan(Plan);
				Console.WriteLine("Опорный план:");
				PrintArray(referencePlan); //Вывод тарифного плана
				OutputAnswer(); //Вывод целевой функции и проверка на вырожденность   
				СheckingOptimality(); //Проверка на оптимальность
			}
		}

		/// <summary>
		/// Метод двойного предпочтения
		/// </summary>
		/// <param name="Arr"></param>
		public void MDoublePreference(int[,] Arr)
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
			if (CheckClosed(Tariff))
			{
				PrintArray(tariff);
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
					int minEl = FindMaxEl(Tariff);
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
				SetReferencePlan(Plan);
				Console.WriteLine("Опорный план:");
				PrintArray(referencePlan); //Вывод тарифного плана
				OutputAnswer(); //Вывод целевой функции и проверка на вырожденность   
				СheckingOptimality(); //Проверка на оптимальность
			}
		}

		/// <summary>
		/// Метод аппроксимации Фогеля
		/// </summary>
		/// <param name="Arr"></param>
		public void MFogelApproximations(int[,] Arr)
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
			if (CheckClosed(Tariff))
			{
				PrintArray(tariff);
				while (SumCheck < (N - 1) * (M - 1))
				{
					//Считаем штрафы по строкам в доп.столбец тарифов
					for (int i = 1; i < Tariff.GetLength(0) - 1; i++)
					{
						MinEl1 = MinEl2 = FindMaxEl(Tariff);
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
							if (MinEl2 == FindMaxEl(Tariff))
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
						MinEl1 = MinEl2 = FindMaxEl(Tariff);
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
							if (MinEl2 == FindMaxEl(Tariff))
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
					MinEl = FindMaxEl(Tariff);
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
				SetReferencePlan(Plan);
				Console.WriteLine("Опорный план:");
				PrintArray(referencePlan); //Вывод тарифного плана
				OutputAnswer(); //Вывод целевой функции и проверка на вырожденность   
				СheckingOptimality(); //Проверка на оптимальность
			}
		}

		/// <summary>
		/// Проверка плана на оптимальность
		/// </summary>
		public void СheckingOptimality()
		{
			int answer;
			Console.WriteLine("Вы желаете проверить план на оптимальность?");
			Console.Write("1 - да, 0 - нет: ");
			answer = Convert.ToInt32(Console.ReadLine());
			if (answer == 1)
			{
				MPotentials();
			}
		}

		/// <summary>
		/// Метод потенциалов
		/// </summary>
		public void MPotentials()
		{
			int[,] plan = new int[referencePlan.GetLength(0) - 1, referencePlan.GetLength(1) - 1];
			int[,] c = new int[tariff.GetLength(0) - 1, tariff.GetLength(1) - 1];
			//потенциалы по строкам ui
			int[] u = new int[plan.GetLength(0)];
			bool[] checU = new bool[u.Length];
			//потенциалы по столбцам vj
			int[] v = new int[plan.GetLength(1)];
			bool[] checV = new bool[v.Length];
			checU[0] = true; //Так как u[0] = 0;
			int n = 0; //Размер delta
			int[] delta = new int[n];
			int[] indexI = new int[n];
			int[] indexJ = new int[n];
			bool notOptimal = false; //оптмальность плана
            //копирование плана и тарифа
            for (int i = 1, k = 0; i < referencePlan.GetLength(0); i++, k++)
			{
				for (int j = 1, m = 0; j < referencePlan.GetLength(1); j++, m++)
				{
					plan[k, m] = referencePlan[i, j];
				}
			}
			for (int i = 1, k = 0; i < tariff.GetLength(0); i++, k++)
			{
				for (int j = 1, m = 0; j < tariff.GetLength(1); j++, m++)
				{
					c[k, m] = tariff[i, j];
				}
			}
			//Оценка занятых ячеек
			//Работает пока встречается хотя бы одна незаполненная ячейка
			while (checU.Any(x => x == false) || checV.Any(x => x == false))
			{
				for (int i = 0; i < plan.GetLength(0); i++)
				{
					for (int j = 0; j < plan.GetLength(1); j++)
					{
						//Проверяем что ячейка заполнена
						//Потенциалы по столбцам
						if (plan[i, j] != 0 && checU[i] == true && checV[j] == false)
						{
							v[j] = c[i, j] - u[i];
							checV[j] = true;
						}
						//Потенциалы по строкам
						if (plan[i, j] != 0 && checU[i] == false && checV[j] == true)
						{
							u[i] = c[i, j] - v[j];
							checU[i] = true;
						}
					}
				}
			}
			Console.WriteLine("Потенциалы занятых ячеек:");
			PrintPotentials(plan, v, u);
			//Оценка свободных ячеек
			for (int i = 0; i < plan.GetLength(0); i++)
			{
				for (int j = 0; j < plan.GetLength(1); j++)
				{
					if (plan[i, j] == 0)
					{
						Array.Resize(ref delta, delta.Length + 1);
						Array.Resize(ref indexI, indexI.Length + 1);
						Array.Resize(ref indexJ, indexJ.Length + 1);
						delta[n] = u[i] + v[j] - c[i, j];
						indexI[n] = i;
						indexJ[n] = j;
						n++;
					}
				}
			}
			Console.WriteLine("Оценка свободных ячеек:");
			for (int i = 0; i < delta.Length; i++)
			{
				Console.WriteLine($"Дельта({indexI[i]}{indexJ[i]}) = {delta[i]}");
				if (delta[i] > 0)
				{
					notOptimal = true;
				}
			}
			//вести массив с перераспределенным опорным планом и определить,
			//будет ли заново составленный опорный план оптимальным
			if (notOptimal)
			{
				int answer;
				Console.WriteLine("План неоптимальный. Хотите ввести перераспределенный опорный план?");
				Console.Write("1 - да, 0 - нет: ");
				answer = Convert.ToInt32(Console.ReadLine());
				if (answer == 1)
				{
					InputReferencePlan();
					MPotentials();
				}
			}
			else
			{
				Console.WriteLine("План оптимальный.");
			}
		}
	}
}