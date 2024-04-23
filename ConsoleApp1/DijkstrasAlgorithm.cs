using System;

namespace ConsoleApp1
{
	public class DijkstrasAlgorithm
	{
		/// <summary>
		/// Возвращяет лист countPath-1 точек и 
		/// </summary>
		/// <param name="countPath">количество точек</param>
		/// <param name="startPoint">Начальная точка из который идем</param>
		/// <param name="ribs">List<(start, end, count)></param>
		public void GetListPaths(int countPath, int startPoint, List<(int, int, int)> ribs)
		{
			int[,] listPaths = new int[countPath,2];
			//Оценка начальных вершин
			for (int i = 0; i < listPaths.GetLength(0); i++)
			{
				if (i + 1 != startPoint)
				{
					listPaths[i, 0] = i + 1;
					listPaths[i, 1] = int.MaxValue;
				}
				else
				{
					listPaths[i, 0] = startPoint;
					listPaths[i, 1] = 0;
				}
			}
			while (ribs.Count > 0)
			{
				int index = ribs.FindIndex(x => x.Item1 == startPoint);
				for (int i = 0; i < listPaths.GetLength(0); i++)
				{
					if (listPaths[i, 0] == ribs[index].Item2)
					{
						if (ribs[index].Item2 < listPaths[i, 1])
						{
							listPaths[i, 1] = ribs[index].Item3;
						}
					}
				}
				ribs.RemoveAt(index);
			}
		}
	}
}
