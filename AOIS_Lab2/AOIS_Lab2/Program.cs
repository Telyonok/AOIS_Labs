using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace AOIS_Lab2
{
	internal static class Program
	{
		const char inv = '!';
		const char con = '*';
		const char dis = '+';
		const string trueCon = "/\\";
		const string trueDis = "\\/";
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
				throw;
			}
		}

		private static string PrepareForWork(string input)
		{
			input = input.Replace(trueDis, dis.ToString());
			input = input.Replace(trueCon, con.ToString());
			input = Regex.Replace(input, @"\s+", "");
			return input;
		}

		private static List<string> FillOperandList(string input)
		{
			List<string> operandList = new List<string>();
			input = input.Replace("(", "");
			input = input.Replace(")", "");
			input = input.Replace(inv.ToString(), "");
			input = input.Replace(con, dis);
			var splittedInput = input.Split(dis);
			foreach (var e in splittedInput)
			{
				if (!operandList.Contains(e))
					operandList.Add(e);
			}
			operandList.Sort();
			return operandList;
		}
	}
}

//!((!a+!b)*(c+b)*!(!a*c))