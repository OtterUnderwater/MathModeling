using ConsoleApp1.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class TaskOfAllocatingInvestments
    {
        /// <summary>
        /// Задача распределения инвестиций рассчитывает в какое предприятие
        /// и с какой ставкой нужно инвестировать, чтобы получить максимальную прибль
        /// </summary>
        public void AllocatingInvestments(Dictionary<int, List<int>> profitMatrix)
        {
            int dif = profitMatrix.Keys.ElementAt(1) - profitMatrix.Keys.ElementAt(0);
            List<(int, int)> splitRate = new List<(int, int)>();
            Dictionary<List<(int, int)>, Investments> keyValues = new Dictionary<List<(int, int)>, Investments>();

            int maxLength = dif;
            while (dif <= profitMatrix.Keys.ElementAt(profitMatrix.Count - 1))
            {
                maxLength += dif;
                for (int i = 0, j = maxLength; i <= maxLength && j >= 0; i += dif, j -= dif)
                {
                    splitRate.Add((i, j));
                }
                keyValues.Add(new List<(int, int)>(splitRate), null);
                splitRate.Clear();
            }

        }
    }
}
