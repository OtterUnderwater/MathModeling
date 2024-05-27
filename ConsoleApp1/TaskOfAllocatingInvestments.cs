using ConsoleApp1.models;

namespace ConsoleApp1
{
    public class TaskOfAllocatingInvestments
    {
        /// <summary>
        /// Задача распределения инвестиций рассчитывает в какое предприятие
        /// и с какой ставкой нужно инвестировать, чтобы получить максимальную прибль
        /// </summary>
        public void AllocatingInvestments(List<List<int>> profitMatrix)
        {
            List<List<int>> saveMatrix = CopyMatrix(profitMatrix);
            List<Distribution> lDistribute = new List<Distribution>();
            //Находим разность - шаг между ставками
            int dif = profitMatrix[1][0] - profitMatrix[0][0];
            int maxLength = dif;
            //Заполняем начало таблицы распределения инвестиций
            while (maxLength <= profitMatrix[profitMatrix.Count - 1][0])
            {
                List<(int, int)> splitRate = new List<(int, int)>();
                for (int i = 0, j = maxLength; i <= maxLength && j >= 0; i += dif, j -= dif)
                {
                    splitRate.Add((i, j));
                }
                lDistribute.Add(new Distribution(maxLength, splitRate));
                maxLength += dif;
            }
            //Далее делаем цикл
            List<List<SplitProfitAndRate>> tableProfitsRates = new List<List<SplitProfitAndRate>>();
            int maxIndex = profitMatrix[0].Count - 1; //Количество столбцов. Цикл работает, пока их > 2 (0, 1)
            while (maxIndex >= 2)
            {
                List <int> lMaxProfits = new List<int>();
                List<SplitProfitAndRate> profitsMaxRate = new List<SplitProfitAndRate>();
                for (int i = 0; i < lDistribute.Count; i++)
                {
                    if (maxIndex == 2) //Когда остается 2 столб => считаем только max ставку
                    {
                        i = lDistribute.Count - 1;
                    }
                    SplitProfitAndRate splitProfitAndRate = new SplitProfitAndRate();
                    List<int> splitProfit = new List<int>();
                    for (int j = 0; j < lDistribute[i].SplitRate.Count; j++)
                    {
                        int summand1 = 0;
                        int summand2 = 0;
                        for (int k = 0; k < profitMatrix.Count; k++)
                        {
                            if (profitMatrix[k][0] == lDistribute[i].SplitRate[j].Item1)
                            {
                                summand1 = profitMatrix[k][maxIndex - 1];
                            }
                            if (profitMatrix[k][0] == lDistribute[i].SplitRate[j].Item2)
                            {
                                summand2 = profitMatrix[k][maxIndex];
                            }
                        }
                        splitProfit.Add(summand1 + summand2);
                    }
                    splitProfitAndRate.SplitProfit = splitProfit;
                    int maxProfit = splitProfit.Max();
                    lMaxProfits.Add(maxProfit);
                    int indexMaxProfit = splitProfit.IndexOf(maxProfit);
                    splitProfitAndRate.RateMaxProfit = lDistribute[i].SplitRate[indexMaxProfit].Item1;
                    profitsMaxRate.Add(splitProfitAndRate);
                }
                tableProfitsRates.Add(profitsMaxRate);
                //После нахождения первого столбца - изменяем данные (ищем уже между 2 и 3)
                maxIndex--;
                if (maxIndex >= 2) //Так как нам не нужно уменьшать матрицу меньше, чем на 2 + 1 столбец
                {
                    for (int k = 1; k < profitMatrix.Count; k++)
                    {
                        profitMatrix[k][maxIndex] = lMaxProfits[k - 1];
                    }
                }
            }
            int startIndex = 0;
            bool first = true;
            int rate = 0;
            List<int> companyRate = new List<int>();
            for (int i = 1; i < saveMatrix.Count-1; i++)
            {
                companyRate.Add(tableProfitsRates[tableProfitsRates.Count - 1][startIndex].RateMaxProfit);
                int max = tableProfitsRates[tableProfitsRates.Count - 1][startIndex].SplitProfit.Max();
                int indexMaxProfit = tableProfitsRates[tableProfitsRates.Count - 1][startIndex].SplitProfit.IndexOf(max);
                if (first)
                {
                    startIndex = lDistribute.Count - 1;
                    first = false;
                }
                rate = lDistribute[startIndex].SplitRate[indexMaxProfit].Item2;
                startIndex = lDistribute.IndexOf(lDistribute.FirstOrDefault(x => x.KeyRate == rate));
                tableProfitsRates.RemoveAt(tableProfitsRates.Count - 1);
            }
            companyRate.Add(rate);
            int maxProfitAnswer = 0;
            //Считаем максимальную прибыль с итоговой матрицы
            for (int i = 0; i < companyRate.Count; i++)
            {
                foreach (List<int> item in saveMatrix)
                {
                    if (item[0] == companyRate[i])
                    {
                        maxProfitAnswer += item[i + 1];
                    }
                }
            }
			WriteToFile(companyRate, maxProfitAnswer);
        }

        private List<List<int>> CopyMatrix(List<List<int>> profitMatrix)
        {
            List<List<int>> matrix = new List<List<int>>();
            foreach (var row in profitMatrix)
            {
                List<int> newRow = new List<int>(row);
                matrix.Add(newRow);
            }
            return matrix;
        }

		private void WriteToFile(List<int> companyRate, int F)
		{
			string path = @"files/taskOfAllocatingInvestmentsAnswer.txt";
			using (StreamWriter writer = new StreamWriter(path, false))
            {
				for (int i = 0; i < companyRate.Count; i++)
				{
					writer.WriteLine($"{i + 1} = {companyRate[i]}");
					Console.WriteLine($"{i + 1} = {companyRate[i]}");
				}
				writer.WriteLine($"F = {F}");
				Console.WriteLine($"F = {F}");
			}
		}
	}
}
