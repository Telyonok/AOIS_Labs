using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOIS_Lab2
{
	internal static class ConverterClass
	{
		const char inv = '!';
		const char con = '*';
		const char dis = '+';
		const string trueCon = "/\\";
		const string trueDis = "\\/";
		public static void ExecuteFreeFormToSdnfSknfTask(string input, List<string> operandList, out string sdnf, out string sknf)
		{
			Dictionary<string, int> tableSdnfIndexCombinations = FillTableSdnfCombinations(operandList);
			Dictionary<int, string> tableIndexSknfCombinations = FillTableSknfCombinations(operandList);
			List<string> processedLevel1 = ProcessExpressionPart(input, out char operation);
			sdnf = ConvertToSdnf(processedLevel1, operandList);
			int functionIndex = GetFunctionIndexAndConstructSknf(sdnf, tableSdnfIndexCombinations, tableIndexSknfCombinations, out sknf);
			AssembleNumericForms(out string numericSdnf, out string numericSknf, Convert.ToString(functionIndex, 2));
			Console.WriteLine($"СДНФ: {PrepareForOutput(sdnf)}\nЧисловая форма: {numericSdnf}\nСКНФ: {PrepareForOutput(sknf)}\nЧисловая форма: {numericSknf}\nИндекс функции: {functionIndex}");
		}

		private static string PrepareForOutput(string str)
		{
			str = str.Replace(dis.ToString(), trueDis);
			str = str.Replace(con.ToString(), trueCon);
			return str;
		}

		private static Dictionary<int, string> FillTableSknfCombinations(List<string> operandList)
		{
			Dictionary<int, string> tableCombinations = new Dictionary<int, string>();
			tableCombinations.Add(128, "(" + operandList[0] + dis + operandList[1] + dis + operandList[2] + ")");
			tableCombinations.Add(64, "(" + operandList[0] + dis + operandList[1] + dis + inv + operandList[2] + ")");
			tableCombinations.Add(32, "(" + operandList[0] + dis + inv + operandList[1] + dis + operandList[2] + ")");
			tableCombinations.Add(16, "(" + operandList[0] + dis + inv + operandList[1] + dis + inv + operandList[2] + ")");
			tableCombinations.Add(8, "(" + inv + operandList[0] + dis + operandList[1] + dis + operandList[2] + ")");
			tableCombinations.Add(4, "(" + inv + operandList[0] + dis + operandList[1] + dis + inv + operandList[2] + ")");
			tableCombinations.Add(2, "(" + inv + operandList[0] + dis + inv + operandList[1] + dis + operandList[2] + ")");
			tableCombinations.Add(1, "(" + inv + operandList[0] + dis + inv + operandList[1] + dis + inv + operandList[2] + ")");
			return tableCombinations;
		}

		private static void AssembleNumericForms(out string numericSdnf, out string numericSknf, string index)
		{
			numericSdnf = trueDis + "(";
			numericSknf = trueCon + "(";
			for (int i = 0; i < index.Length; i++)
			{
				if (index[i] == '1')
					numericSdnf += i.ToString() + ',';
				else
					numericSknf += i.ToString() + ",";
			}
			numericSdnf = numericSdnf[..^1] + ")";
			numericSknf = numericSknf[..^1] + ")";
		}

		private static int GetFunctionIndexAndConstructSknf(string sdnf, Dictionary<string, int> tableSdnfCombinations, Dictionary<int, string> tableIndexSknfCombinations, out string sknf)
		{
			int index = 0;
			sknf = string.Empty;
			var splittedSdnf = sdnf.Split(dis);

			foreach (var key in tableSdnfCombinations.Keys)
			{
				if (splittedSdnf.Contains(key))
				{
					index += tableSdnfCombinations[key];
				}
				else
					sknf += tableIndexSknfCombinations[tableSdnfCombinations[key]] + con;
			}
			sknf = sknf[..^1];
			return index;
		}

		private static Dictionary<string, int> FillTableSdnfCombinations(List<string> operandList)
		{
			Dictionary<string, int> tableCombinations = new Dictionary<string, int>();
			tableCombinations.Add(inv + operandList[0] + con + inv + operandList[1] + con + inv + operandList[2], 128);
			tableCombinations.Add(inv + operandList[0] + con + inv + operandList[1] + con + operandList[2], 64);
			tableCombinations.Add(inv + operandList[0] + con + operandList[1] + con + inv + operandList[2], 32);
			tableCombinations.Add(inv + operandList[0] + con + operandList[1] + con + operandList[2], 16);
			tableCombinations.Add(operandList[0] + con + inv + operandList[1] + con + inv + operandList[2], 8);
			tableCombinations.Add(operandList[0] + con + inv + operandList[1] + con + operandList[2], 4);
			tableCombinations.Add(operandList[0] + con + operandList[1] + con + inv + operandList[2], 2);
			tableCombinations.Add(operandList[0] + con + operandList[1] + con + operandList[2], 1);
			return tableCombinations;
		}

		private static string ConvertToSdnf(List<string> processedLevel1, List<string> operandList)
		{
			string sdnf = string.Empty;
			foreach (var part in processedLevel1)
			{
				foreach (var operand in operandList)
				{
					if (!part.Contains(operand))
					{
						int position = operandList.IndexOf(operand);
						if (position == 2)
							sdnf += part + con + operand + dis + part + con + inv + operand;
						else if (position == 0)
							sdnf += operand + con + part + dis + inv + operand + con + part;
						else
						{
							var splittedPart = part.Split(con);
							sdnf += splittedPart[0] + con + operand + con + splittedPart[1] + dis + splittedPart[0] + con + inv + operand + con + splittedPart[1];
						}
						sdnf += dis;
						break;
					}
				}
			}
			return sdnf[..^1];
		}

		private static List<string> ProcessExpressionPart(string part, out char operation)
		{
			bool inversed = part[0] == inv ? true : false;
			string workString;
			if (inversed)
				workString = part[2..^1];
			else
				workString = part;
			List<string> splittedWorkString = SplitString(workString, out operation);

			if (inversed)
			{
				splittedWorkString[0] = InverseExpression(splittedWorkString[0]);
				splittedWorkString[1] = InverseExpression(splittedWorkString[1]);
				if (operation == con)
					operation = dis;
				else
					operation = con;
			}
			splittedWorkString.Append(operation.ToString());
			return splittedWorkString;
		}

		private static string InverseExpression(string expression)
		{
			if (expression[0] == inv)
				expression = expression[2..^1];
			else
			{
				expression = expression.Replace("(", inv.ToString());
				expression = expression.Replace(con.ToString(), con + inv.ToString());
				expression = expression.Replace(dis.ToString(), dis + inv.ToString());
				expression = expression.Replace(inv.ToString() + inv, "");
				if (expression.Contains(dis))
					expression = expression.Replace(dis, con);
				else
					expression = expression.Replace(con, dis);
			}
			return expression.Replace(")", "");
		}

		private static List<string> SplitString(string workString, out char operation)
		{
			List<string> splittedString = new List<string>();
			bool inBrackets = false;
			operation = ' ';
			for (int i = 0; i < workString.Length; i++)
			{
				if (workString[i] == '(')
					inBrackets = true;
				else if (workString[i] == ')')
					inBrackets = false;
				else if (workString[i] == con || workString[i] == dis)
					if (!inBrackets)
					{
						string[] splitted = new string[2];
						splitted[0] = workString[..i];
						splitted[1] = workString[(i + 1)..];
						if (workString[i] == con)
						{
							operation = con;
						}
						else
						{
							operation = dis;
						}
						splittedString.Add(splitted[0]);
						splittedString.Add(splitted[1]);
						break;
					}
			}

			return splittedString;
		}
	}
}
