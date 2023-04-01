using AOIS_Lab2;

namespace AOIS_Lab4
{
    internal class Program
    {
        const string carrySDNF = "!x1*!x2*x3+!x1*x2*!x3+!x1*x2*x3+x1*x2*x3";
        const string resultSDNF = "!x1*!x2*x3+!x1*x2*!x3+x1*!x2*!x3+x1*x2*x3";
        const string y1SDNF = "!x1*!x2*x3*!x4+!x1*!x2*x3*x4+!x1*x2*!x3*!x4+!x1*x2*!x3*x4+!x1*x2*x3*!x4+!x1*x2*x3*x4+x1*!x2*!x3*!x4+x1*!x2*!x3*x4";
        const string y2SDNF = "!x1*!x2*!x3*!x4+!x1*!x2*!x3*x4+!x1*x2*x3*!x4+!x1*x2*x3*x4+x1*!x2*!x3*!x4+x1*!x2*!x3*x4";
        const string y3SDNF = "!x1*!x2*!x3*!x4+!x1*!x2*!x3*x4+!x1*x2*!x3*!x4+!x1*x2*!x3*x4+x1*!x2*!x3*!x4+x1*!x2*!x3*x4";
        const string y4SDNF = "!x1*!x2*!x3*x4+!x1*!x2*x3*x4+!x1*x2*!x3*x4+!x1*x2*x3*x4+x1*!x2*!x3*x4";
        static void Main()
        {
            List<string> operandListTask1 = FillOperandList(carrySDNF);
            List<string> operandListTask2 = FillOperandList(y1SDNF);
            DoTask1(operandListTask1);
            DoTask2(operandListTask2);
        }

        private static void DoTask1(List<string> operandList)
        {
            Console.WriteLine($"Таблица истинности для двоичного вычитателя:");
            DrawSubstractorTruthTable(operandList);
            Console.WriteLine($"Функция остатка b (carry) в СДНФ: {carrySDNF}");
            Console.WriteLine($"Функция результата d (result) в СДНФ: {resultSDNF}");
            string minbSDNF = MinimizerClass.MinimizeSdnf(carrySDNF, operandList);
            Console.WriteLine($"Минимизированная функция b (carry): {minbSDNF}");
            string uniquePartInbSDNF = FindUniquePart(carrySDNF.Split(CharSet.disjunction), resultSDNF.Split(CharSet.disjunction));
            string uniquePartIndSDNF = FindUniquePart(resultSDNF.Split(CharSet.disjunction), carrySDNF.Split(CharSet.disjunction));
            AssembleCoreFormulas(minbSDNF, uniquePartInbSDNF, uniquePartIndSDNF, out string carry, out string result);
            Console.WriteLine($"Конечная функция остатка b (carry): {carry}");
            Console.WriteLine($"Конечная функция результата d (result): {result}");
        }

        private static void DoTask2(List<string> operandList)
        {
            Console.WriteLine($"\nТаблица истинности для Д8421 -> Д8421+6:");
            Draw8421Plus6TruthTable(operandList);
            Console.WriteLine("Формулы y1, y2, y3, y4 в СДНФ:");
            Console.WriteLine(y1SDNF);
            Console.WriteLine(y2SDNF);
            Console.WriteLine(y3SDNF);
            Console.WriteLine(y4SDNF);
            Console.WriteLine("!x1*x4+!x2*!x3*x4");
            Console.WriteLine("Минимизированные y1, y2, y3, y4:");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(y1SDNF, operandList));
            Console.WriteLine(MinimizerClass.MinimizeSdnf(y2SDNF, operandList));
            Console.WriteLine(MinimizerClass.MinimizeSdnf(y3SDNF, operandList));
            Console.WriteLine(MinimizerClass.MinimizeSdnf(y4SDNF, operandList));
        }

        private static void DrawSubstractorTruthTable(List<string> operandList)
        {
            Console.WriteLine($"|{operandList[0]} |{operandList[1]} |{operandList[2]} | d | b |");
            Console.WriteLine("|---|---|---|---|---|");
            Console.WriteLine("| 0 | 0 | 0 | 0 | 0 |");
            Console.WriteLine("| 0 | 0 | 1 | 1 | 1 |");
            Console.WriteLine("| 0 | 1 | 0 | 1 | 1 |");
            Console.WriteLine("| 0 | 1 | 1 | 0 | 1 |");
            Console.WriteLine("| 1 | 0 | 0 | 1 | 0 |");
            Console.WriteLine("| 1 | 0 | 1 | 0 | 0 |");
            Console.WriteLine("| 1 | 1 | 0 | 0 | 0 |");
            Console.WriteLine("| 1 | 1 | 1 | 1 | 1 |");
        }

        private static void Draw8421Plus6TruthTable(List<string> operandList)
        {
            Console.WriteLine($"|{operandList[0]} |{operandList[1]} |{operandList[2]} |{operandList[3]} |y1 |y2 |y3 |y4 |");
            Console.WriteLine("|---|---|---|---|---|---|---|---|");
            Console.WriteLine("| 0 | 0 | 0 | 0 | 1 | 1 | 1 | 0 |");
            Console.WriteLine("| 0 | 0 | 0 | 1 | 1 | 1 | 1 | 1 |");
            Console.WriteLine("| 0 | 0 | 1 | 0 | 1 | 0 | 0 | 0 |");
            Console.WriteLine("| 0 | 0 | 1 | 1 | 1 | 0 | 0 | 1 |");
            Console.WriteLine("| 0 | 1 | 0 | 0 | 1 | 0 | 1 | 0 |");
            Console.WriteLine("| 0 | 1 | 0 | 1 | 1 | 0 | 1 | 1 |");
            Console.WriteLine("| 0 | 1 | 1 | 0 | 1 | 1 | 0 | 0 |");
            Console.WriteLine("| 0 | 1 | 1 | 1 | 1 | 1 | 0 | 1 |");
            Console.WriteLine("| 1 | 0 | 0 | 0 | 1 | 1 | 1 | 0 |");
            Console.WriteLine("| 1 | 0 | 0 | 1 | 1 | 1 | 1 | 1 |");
            Console.WriteLine("| 1 | 0 | 1 | 0 |---|---|---|---|");
            Console.WriteLine("| 1 | 0 | 1 | 1 |---|---|---|---|");
            Console.WriteLine("| 1 | 1 | 0 | 0 |---|---|---|---|");
            Console.WriteLine("| 1 | 1 | 0 | 1 |---|---|---|---|");
            Console.WriteLine("| 1 | 1 | 1 | 0 |---|---|---|---|");
            Console.WriteLine("| 1 | 1 | 1 | 1 |---|---|---|---|");
        }

        private static void AssembleCoreFormulas(string minCarrySDNF, string uniquePartInbSDNF, string uniquePartIndSDNF, out string carry, out string result)
        {
            carry = minCarrySDNF;
            result = "(" + carry + ")" + CharSet.conjunction + "(" + uniquePartInbSDNF.Replace(CharSet.conjunction, CharSet.disjunction) + ")" + CharSet.disjunction + uniquePartIndSDNF;
        }

        private static string FindUniquePart(string[] arrayToSearch, string[] arrayToCompareTo)
        {
            foreach (string part in arrayToSearch)
                if (!arrayToCompareTo.Contains(part))
                    return part;
            return string.Empty;
        }

        private static List<string> FillOperandList(string input)
        {
            List<string> operandList = new List<string>();
            input = input.Replace("(", "");
            input = input.Replace(")", "");
            input = input.Replace(CharSet.inversion.ToString(), "");
            input = input.Replace(CharSet.conjunction, CharSet.disjunction);
            var splittedInput = input.Split(CharSet.disjunction);
            foreach (var operand in splittedInput)
                if (!operandList.Contains(operand))
                    operandList.Add(operand);
            operandList.Sort();
            return operandList;
        }
    }
}