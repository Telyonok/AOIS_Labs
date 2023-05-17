using AOIS_Lab8;

namespace Program
{
    internal class Program
    {
        static void printVector(List<int> vector)
        {
            foreach (var bit in vector)
            {
                Console.Write(bit);
                Console.Write(" ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        static void Main()
        {
            Processor processor = new Processor(16);
            Console.WriteLine("Матрица 16x16 с диагональной адресацией");
            processor.Print();
            processor.Add(0, new List<int>() { 1, 1, 1, 1, 1, 1 });
            Console.WriteLine();
            Console.WriteLine("Матрица после добавления слова 1 1 1 1 1 1 по адресу 0");
            processor.Print();
            Console.WriteLine();
            var a = processor.GetNearestLower(new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1});
            Console.WriteLine("Ближайшее меньшее слово к слову 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1");
            printVector(a);
            var b = processor.GetNearestUpper(new List<int> { 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1});
            Console.WriteLine("Ближайшее большее слово к слову 1 0 0 0 1 1 1 1 1 1 1 1 1 1 1 1");
            printVector(b);
            Console.WriteLine();
            processor.Sum(new List<int> { 1, 1, 1});
            Console.WriteLine("Матрица после суммации вида 1 1 1");
            processor.Print();

            processor.Operate(0, 1, 2, Processor.Conjunction);
            Console.WriteLine("Матрица после операции конъюкции между первыи и вторым словами, записанной в третье");
            processor.Print();
            Console.WriteLine();
            Console.WriteLine("Где:");
            Console.WriteLine("Первое слово");
            printVector(processor.Get(0));
            Console.WriteLine("Второе слово");
            printVector(processor.Get(1));
            Console.WriteLine("Третье слово (результат)");
            printVector(processor.Get(2));
        }
    }
}