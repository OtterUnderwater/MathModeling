namespace ConsoleApp1.models
{
    //Универсальная модель для хранения значения по строке
    public class Znach
    {
        public int Index;
        public decimal Value;
        public Znach(int index, decimal znach)
        {
            Index = index;
            Value = znach;
        }
    }
}
