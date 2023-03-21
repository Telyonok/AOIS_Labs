using System.Text.RegularExpressions;

namespace AOIS_Lab2
{
	internal static class Program
	{
		static void Main()
		{
			try
            		{
                		Console.WriteLine("Введите логическую функцию:");
                		string? input = Console.ReadLine();
                		if (input == null)
                    			return;
                		input = PrepareForWork(input);
                		List<string> operandList = FillOperandList(input);

                		ConverterClass.ExecuteFreeFormToSdnfSknfTask(input, operandList, out string sdnf, out string sknf);
                		MinimizerClass.ExecuteMinimizeSdnfSknfTask(operandList, sdnf, sknf);
            		}
			catch (Exception)
			{
				Console.WriteLine("Неверный ввод.");
			}
		}

		private static string PrepareForWork(string input)
		{
			input = input.Replace(CharSet.trueDisjunction, CharSet.disjunction.ToString());
			input = input.Replace(CharSet.trueConjunction, CharSet.conjunction.ToString());
			input = Regex.Replace(input, @"\s+", "");
			return input;
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
			{
				if (!operandList.Contains(operand))
					operandList.Add(operand);
			}
			operandList.Sort();
			return operandList;
		}
	}
}

//!((!a+!b)*(c+b)*!(!a*c))
