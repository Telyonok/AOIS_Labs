using AOIS_Lab8;

namespace Program
{
    internal class Program
    {
        static void Main()
        {
            Processor processor = new Processor(16);
            processor.Print();
            processor.Add(15, new List<int>() { 1, 1, 1, 1, 1, 1 });
            processor.Add(1, new List<int>() { 1, 1, 1, 1, 1, 1 });
            Console.WriteLine();
            processor.Print();
            Console.WriteLine();
            var a = processor.GetNearestLower(new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1});
            var b = processor.GetNearestUpper(new List<int> { 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1});
            processor.Sum(new List<int> { 1, 1, 1});
        }
    }
}