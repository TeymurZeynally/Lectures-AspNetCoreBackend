namespace Lecture04.DI.Repositories
{
    internal class FoodRepository : IFoodRepository
    {
        private readonly string _dbPath;

        public FoodRepository(string dbPath)
        {
            Console.WriteLine($"Создался класс FoodRepository {new Random().Next(0, int.MaxValue)}");
            _dbPath = dbPath;
        }

        public string GetFoodFor(string catName) => "tuna";
    }
}
