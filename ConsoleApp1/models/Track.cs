namespace ConsoleApp1.models
{
	//Модель для задачи Коммивояжера
	public class Track
	{
		public List<(int, int)>? Way { get; set; } //лист индекса начала и конца пути
		public int[,]? Matrix { get; set; } //матрица на данный момент
		public int Costs { get; set; } //цена нижней границы ветви

		public Track(List<(int, int)> way, int[,] matrix, int costs)
		{
			Way = way;
			Matrix = matrix;
			Costs = costs;
		}
	}
}
