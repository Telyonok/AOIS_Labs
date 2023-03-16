using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOIS_Lab2
{
	internal static class ConverterClass
	{
		static Dictionary<string, int> setNValues = new()
		{
            {"000", 128},
            {"001", 64},
            {"010", 32},
            {"011", 16},
            {"100", 8},
            {"101", 4},
            {"110", 2},
            {"111", 1}
		};

		const char inv = '!';
		const char con = '*';
		const char dis = '+';
		const string trueCon = "/\\";
		const string trueDis = "\\/";
		public static void ExecuteFreeFormToSdnfSknfTask(string input, List<string> operandList, out string sdnf, out string sknf)
		{
			Dictionary<int, string> tableIndexSdnfCombinations = FillTableSdnfCombinations(operandList);
			Dictionary<int, string> tableIndexSknfCombinations = FillTableSknfCombinations(operandList);
			Console.WriteLine();
			int functionIndex = GetFunctionIndex(input, operandList);
			Console.WriteLine();
			sdnf = ConstructSdnf(functionIndex, tableIndexSdnfCombinations);
            sknf = ConstructSknf(functionIndex, tableIndexSknfCombinations);
            AssembleNumericForms(out string numericSdnf, out string numericSknf, Convert.ToString(functionIndex, 2).PadLeft(8,'0'));
			Console.WriteLine($"СДНФ: {PrepareForOutput(sdnf)}\nЧисловая форма: {numericSdnf}\nСКНФ: {PrepareForOutput(sknf)}\nЧисловая форма: {numericSknf}\nИндекс функции: {functionIndex}");
		}

        private static string ConstructSknf(int functionIndex, Dictionary<int, string> tableIndexSknfCombinations)
        {
			string sknf = "";
			int sc = 128;
            while (sc > 0)
			{
				if (functionIndex < sc)
					sknf += tableIndexSknfCombinations[sc] + con;
                else
                    functionIndex -= sc;
                sc /= 2;
			}

			return sknf.Length > 0? sknf[..^1]: "";
        }

        private static string ConstructSdnf(int functionIndex, Dictionary<int, string> tableIndexSdnfCombinations)
        {
            string sdnf = "";
            int sc = 128;
            while (sc > 0)
            {
                if (functionIndex >= sc)
                {
                    sdnf += tableIndexSdnfCombinations[sc] + dis;
                    functionIndex -= sc;
                }
                sc /= 2;
            }

            return sdnf.Length > 0 ? sdnf[..^1] : "";
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

		private static int GetFunctionIndex(string input, List<string> operandList)
		{
			int index = 0;
            Console.WriteLine("Таблица:");
            for (int i = 0; i < setNValues.Count; i++)
            {
                var set = setNValues.Keys.ElementAt(i);
                Console.Write($"{set}: ");
                string temp = input.Replace(operandList[0], set[0].ToString()).Replace(operandList[1], set[1].ToString()).Replace(operandList[2], set[2].ToString());
				if (Evaluate(temp) == "1")
                {
                    index += setNValues[set];
                    Console.WriteLine("1");
                }
                else
                    Console.WriteLine("0");
            }
			return index;
		}

		private static string Evaluate(string temp)
		{
			if (temp[0] == '(' && temp[^1] == ')')
				temp = temp[1..^1];
			if (temp.Length == 1)
				return temp;
            Stack<int> openBracketIndex = new Stack<int>();
            for (int j = 0; j < temp.Length; j++)
            {
                if (temp[j] == '(')
                    openBracketIndex.Push(j);
                else if (temp[j] == ')')
                {
					int index = openBracketIndex.Pop();
                    int length = j - index + 1;
                    temp = temp.Substring(0, index) + Evaluate(temp.Substring(index, length)) + temp.Substring(j + 1);
					j = index;
                }
            }
            temp = temp.Replace(inv + "0", "1").Replace(inv + "1", "0");
            return (temp.Contains(dis) && temp.Contains("1")) || !temp.Contains("0")? "1" : "0";
        }

		private static Dictionary<int, string> FillTableSdnfCombinations(List<string> operandList)
		{
			Dictionary<int, string> tableCombinations = new Dictionary<int, string>();
			tableCombinations.Add(128, inv + operandList[0] + con + inv + operandList[1] + con + inv + operandList[2]);
			tableCombinations.Add(64, inv + operandList[0] + con + inv + operandList[1] + con + operandList[2]);
			tableCombinations.Add(32, inv + operandList[0] + con + operandList[1] + con + inv + operandList[2]);
			tableCombinations.Add(16, inv + operandList[0] + con + operandList[1] + con + operandList[2]);
			tableCombinations.Add(8, operandList[0] + con + inv + operandList[1] + con + inv + operandList[2]);
			tableCombinations.Add(4, operandList[0] + con + inv + operandList[1] + con + operandList[2]);
			tableCombinations.Add(2, operandList[0] + con + operandList[1] + con + inv + operandList[2]);
			tableCombinations.Add(1, operandList[0] + con + operandList[1] + con + operandList[2]);
			return tableCombinations;
		}
    }
}

// !((a+!b)*(!c+b)*!(a*c))