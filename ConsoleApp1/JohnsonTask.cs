
namespace ConsoleApp1
{
    public class JohnsonTask
    {
        /// <summary>
        /// Возвращает оптимальный ответ, время простоя в начале, время простоя в конце
        /// </summary>
        /// <param name="listJohnson"></param>
        public void GetAnswerJohnson(List<(int, int, bool Access)> listJohnson) {
            string downtimeBefore = GetMaxDowntime(listJohnson);
            int startIndex = 0;
            int endIndex = listJohnson.Count - 1;
            while (startIndex != endIndex)
            {
                int min = int.MaxValue;
                for (int i = 0; i < listJohnson.Count; i++)
                {
                    if (listJohnson[i].Access && (listJohnson[i].Item1 < min || listJohnson[i].Item2 < min))
                    {
                        if (listJohnson[i].Item1 < listJohnson[i].Item2)
                        {
                            min = listJohnson[i].Item1;
                        }
                        else
                        {
                            min = listJohnson[i].Item2;
                        }
                    }
                }
                int count1 = listJohnson.Where(lj => lj.Item1 == min && lj.Access).Count();
                while (count1 > 0)
                {
                    if (count1 == 1)
                    {
                        for (int i = 0; i < listJohnson.Count; i++)
                        {
                            if (listJohnson[i].Item1 == min && listJohnson[i].Access)
                            {
                                listJohnson[i] = (listJohnson[i].Item1, listJohnson[i].Item2, false);
                                (int, int, bool) itemTemp = listJohnson[startIndex];
                                listJohnson[startIndex] = listJohnson[i];
                                listJohnson[i] = itemTemp;
                                startIndex++;
                                break;
                            }
                        }
                        count1--;
                    }
                    else
                    {
                        int MaxEl2 = listJohnson.Where(lj => lj.Item1 == min && lj.Access).Max(lj => lj.Item2);
                        (int, int, bool) MaxEl = listJohnson.Where(lj => lj.Item1 == min && lj.Access).Where(lj => lj.Item2 == MaxEl2).FirstOrDefault();
                        for (int i = 0; i < listJohnson.Count; i++)
                        {
                            if (listJohnson[i].Item1 == MaxEl.Item1 && listJohnson[i].Item2 == MaxEl.Item2 && listJohnson[i].Access)
                            {
                                listJohnson[i] = (listJohnson[i].Item1, listJohnson[i].Item2, false);
                                (int, int, bool) itemTemp = listJohnson[startIndex];
                                listJohnson[startIndex] = listJohnson[i];
                                listJohnson[i] = itemTemp;
                                startIndex++;
                                break;
                            }
                        }
                        count1--;
                    }
                }
                int count2 = listJohnson.Where(lj => lj.Item2 == min && lj.Access).Count();
                while (count2 > 0)
                {
                    if (count2 == 1)
                    {
                        for (int i = 0; i < listJohnson.Count; i++)
                        {
                            if (listJohnson[i].Item2 == min && listJohnson[i].Access)
                            {
                                listJohnson[i] = (listJohnson[i].Item1, listJohnson[i].Item2, false);
                                (int, int, bool) itemTemp = listJohnson[endIndex];
                                listJohnson[endIndex] = listJohnson[i];
                                listJohnson[i] = itemTemp;
                                endIndex--;
                                break;
                            }
                        }
                        count2--;
                    }
                    else
                    {
                        int MaxEl2 = listJohnson.Where(lj => lj.Item2 == min && lj.Access).Max(lj => lj.Item1);
                        (int, int, bool) MaxEl = listJohnson.Where(lj => lj.Item2 == min && lj.Access).Where(lj => lj.Item1 == MaxEl2).FirstOrDefault();
                        for (int i = 0; i < listJohnson.Count; i++)
                        {
                            if (listJohnson[i].Item1 == MaxEl.Item1 && listJohnson[i].Item2 == MaxEl.Item2 && listJohnson[i].Access)
                            {
                                listJohnson[i] = (listJohnson[i].Item1, listJohnson[i].Item2, false);
                                (int, int, bool) itemTemp = listJohnson[endIndex];
                                listJohnson[endIndex] = listJohnson[i];
                                listJohnson[i] = itemTemp;
                                endIndex--;
                                break;
                            }
                        }
                        count2--;
                    }
                }
            }
            string downtimeAfter = GetMaxDowntime(listJohnson);
            WriteToFile(listJohnson, downtimeBefore, downtimeAfter);
        }

        /// <summary>
        /// Возвращает максимальное время простоя второй машины
        /// </summary>
        /// <param name="listJohnson"></param>
        /// <returns></returns>
        public string GetMaxDowntime(List<(int, int, bool)> listJohnson)
        {
            List<int> downtime = new List<int>();
            int count = listJohnson.Count;
            int startIndex = 0;
            int endIndex = -1;
            while (count > 0)
            {
                int sumMachine1 = 0;
                int sumMachine2 = 0;
                for (int i = 0; i <= startIndex; i++)
                {
                    sumMachine1 = sumMachine1 + listJohnson[i].Item1;
                }
                startIndex++;
                for (int i = 0; i <= endIndex; i++)
                {
                    sumMachine2 = sumMachine2 + listJohnson[i].Item2;
                }
                endIndex++;
                downtime.Add(sumMachine1 - sumMachine2);
                count--;
            }
            int MaxDowntime = downtime.Max();
            return $"max({MaxDowntime})";
        }

        /// <summary>
        /// Записывает ответ в файл
        /// </summary>
        /// <param name="listJohnson"></param>
        /// <param name="downtimeBefore"></param>
        /// <param name="downtimeAfter"></param>
        public void WriteToFile(List<(int, int, bool)> listJohnson, string downtimeBefore, string downtimeAfter)
        {
            string path = @"files/johnsonTaskAnswer.txt";
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(downtimeBefore);
                writer.WriteLine("Оптимальный план:");
                foreach (var item in listJohnson)
                {
                    writer.WriteLine($"{item.Item1} {item.Item2}");
                }
                writer.WriteLine(downtimeAfter);
            }
        }
    }
}
