namespace ConsoleApp1
{
	public class DijkstrasAlgorithm
	{
		/// <summary>
		/// Выводит лист countPath-1 путей и их минимальный маршрут
		/// </summary>
		/// <param name="countPath">количество точек</param>
		/// <param name="startPoint">Начальная точка из который идем</param>
		/// <param name="ribs">List<(start, end, count)></param>
		public void ShortestPaths(int countPath, int startPoint, List<(int, int, int)> ribs)
		{
			List<int> listPoint = new List<int>(); //лист расстояния до вершин
			List<bool> listCheck = new List<bool>(); //проверка что вершины пройдены
			for (int i = 0; i < countPath; i++)
			{
				listPoint.Add(int.MaxValue);
				listCheck.Add(false);
			}
			listPoint[startPoint - 1] = 0; //стартовая вершина = 0
			while (listCheck.Contains(false))  //Пока есть хотя бы одна непройденная
			{
				int minIndex = 0;
				int minCost = int.MaxValue;
				// Находим вершину с минимальной стоимостью, которая не посещена
				for (int i = 0; i < listPoint.Count; i++)
				{
					if (listPoint[i] < minCost && listCheck[i] == false)
					{
						minCost = listPoint[i];
						minIndex = i;
					}
				}
				//Выбираем все ребра, начало/конец которых = вершине (minIndex + 1)
				var filterRibs = ribs.Where(x => x.Item1 == minIndex + 1 || x.Item2 == minIndex + 1).ToList();
				foreach (var rib in filterRibs)
				{
					//Получаем конeц вершины
					int endPoint = (rib.Item1 == minIndex + 1) ? rib.Item2 : rib.Item1;
					//Складываем стоимость вершины и путь к другой вершине
					int cost = listPoint[minIndex] + rib.Item3;
					//Если число меньше, то меняем
					if (cost < listPoint[endPoint - 1]) //index - 1, т.к. i c 0
					{
						listPoint[endPoint - 1] = cost;
					}
					ribs.Remove(rib); //удаляем ребро
				}
				listCheck[minIndex] = true; //Вершина посещена
			}
			PrintShortestPaths(startPoint, listPoint);
		}

		/// <summary>
		/// Вывод минимальных путей
		/// </summary>
		/// <param name="startPoint"></param>
		/// <param name="listPoint"></param>
		public void PrintShortestPaths(int startPoint, List<int> listPoint)
		{
			Console.WriteLine("Кратчайшие пути:");
			for (int i = 0; i < listPoint.Count; i++)
			{
				if (i + 1 != startPoint)
				{
					Console.WriteLine($"{startPoint} -> {i + 1} = {listPoint[i]}");
				}
			}
		}
	}
}
