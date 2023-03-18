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
		static int[] Weights = { 128, 64, 32, 16, 8, 4, 2, 1 };
        static string[] Sets = { "000", "001", "010", "011", "100", "101", "110", "111" };
		static int MaxSetWeight = Weights[0];
		static Dictionary<string, int> setsAndWeights = new()
		{
            {Sets[0], Weights[0]},
            {Sets[1], Weights[1]},
            {Sets[2], Weights[2]},
            {Sets[3], Weights[3]},
            {Sets[4], Weights[4]},
            {Sets[5], Weights[5]},
            {Sets[6], Weights[6]},
            {Sets[7], Weights[7]}
		};
		public static void ExecuteFreeFormToSdnfSknfTask(string input, List<string> operandList, out string sdnf, out string sknf)
		{
			Dictionary<int, string> tableIndexSdnfCombinations = FillTableSdnfCombinations(operandList);
			Dictionary<int, string> tableIndexSknfCombinations = FillTableSknfCombinations(operandList);
			Console.WriteLine();
			int functionIndex = GetFunctionIndex(input, operandList);
			Console.WriteLine();
            ConstructCompleteForms(functionIndex, tableIndexSknfCombinations, tableIndexSdnfCombinations, out sdnf, out sknf);
            AssembleNumericForms(out string numericSdnf, out string numericSknf, Convert.ToString(functionIndex, 2).PadLeft(setsAndWeights.Count,'0'));
			Console.WriteLine($"СДНФ: {PrepareForOutput(sdnf)}\nЧисловая форма: {numericSdnf}\nСКНФ: {PrepareForOutput(sknf)}\nЧисловая форма: {numericSknf}\nИндекс функции: {functionIndex}");
		}

        private static void ConstructCompleteForms(int functionIndex, Dictionary<int, string> tableIndexSknfCombinations, Dictionary<int, string> tableIndexSdnfCombinations, out string sdnf, out string sknf)
        {
			sknf = "";
            sdnf = "";
            int currentWeight = MaxSetWeight;
            while (currentWeight > 0)
			{
				if (functionIndex < currentWeight)
					sknf += tableIndexSknfCombinations[currentWeight] + CharSet.conjunction;
                else
                {
                    sdnf += tableIndexSdnfCombinations[currentWeight] + CharSet.disjunction;
                    functionIndex -= currentWeight;
                }
                currentWeight /= 2;
			}
            sknf = sknf[..^1];
            sdnf = sdnf[..^1];
        }

        private static string PrepareForOutput(string str)
		{
			str = str.Replace(CharSet.disjunction.ToString(), CharSet.trueDisjunction);
			str = str.Replace(CharSet.conjunction.ToString(), CharSet.trueConjunction);
			return str;
		}

		private static void AssembleNumericForms(out string numericSdnf, out string numericSknf, string index)
		{
			numericSdnf = CharSet.trueDisjunction + "(";
			numericSknf = CharSet.trueConjunction + "(";
			for (int index1 = 0; index1 < index.Length; index1++)
			{
				if (index[index1] == '1')
					numericSdnf += index1.ToString() + ',';
				else
					numericSknf += index1.ToString() + ",";
			}
			numericSdnf = numericSdnf[..^1] + ")";
			numericSknf = numericSknf[..^1] + ")";
		}

		private static int GetFunctionIndex(string input, List<string> operandList)
		{
			int index = 0;
            Console.WriteLine("Таблица:");
            for (int index1 = 0; index1 < setsAndWeights.Count; index1++)
            {
                var set = setsAndWeights.Keys.ElementAt(index1);
                Console.Write($"{set}: ");
                string temp = input.Replace(operandList[0], set[0].ToString()).Replace(operandList[1], set[1].ToString()).Replace(operandList[2], set[2].ToString());
				if (Evaluate(temp) == "1")
                {
                    index += setsAndWeights[set];
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
            for (int index2 = 0; index2 < temp.Length; index2++)
            {
                if (temp[index2] == '(')
                    openBracketIndex.Push(index2);
                else if (temp[index2] == ')')
                {
					int index = openBracketIndex.Pop();
                    int length = index2 - index + 1;
                    temp = temp.Substring(0, index) + Evaluate(temp.Substring(index, length)) + temp.Substring(index2 + 1);
					index2 = index;
                }
            }
            temp = temp.Replace(CharSet.inversion + "0", "1").Replace(CharSet.inversion + "1", "0");
            return (temp.Contains(CharSet.disjunction) && temp.Contains("1")) || !temp.Contains("0")? "1" : "0";
        }

		private static Dictionary<int, string> FillTableSdnfCombinations(List<string> operandList)
		{
            Dictionary<int, string> tableCombinations = new Dictionary<int, string>
            {
                { Weights[0], CharSet.inversion + operandList[0] + CharSet.conjunction + CharSet.inversion + operandList[1] + CharSet.conjunction + CharSet.inversion + operandList[2] },
                { Weights[1], CharSet.inversion + operandList[0] + CharSet.conjunction + CharSet.inversion + operandList[1] + CharSet.conjunction + operandList[2] },
                { Weights[2], CharSet.inversion + operandList[0] + CharSet.conjunction + operandList[1] + CharSet.conjunction + CharSet.inversion + operandList[2] },
                { Weights[3], CharSet.inversion + operandList[0] + CharSet.conjunction + operandList[1] + CharSet.conjunction + operandList[2] },
                { Weights[4], operandList[0] + CharSet.conjunction + CharSet.inversion + operandList[1] + CharSet.conjunction + CharSet.inversion + operandList[2] },
                { Weights[5], operandList[0] + CharSet.conjunction + CharSet.inversion + operandList[1] + CharSet.conjunction + operandList[2] },
                { Weights[6], operandList[0] + CharSet.conjunction + operandList[1] + CharSet.conjunction + CharSet.inversion + operandList[2] },
                { Weights[7], operandList[0] + CharSet.conjunction + operandList[1] + CharSet.conjunction + operandList[2] }
            };
            return tableCombinations;
        }

        private static Dictionary<int, string> FillTableSknfCombinations(List<string> operandList)
        {
            Dictionary<int, string> tableCombinations = new Dictionary<int, string>
            {
                { Weights[0], "(" + operandList[0] + CharSet.disjunction + operandList[1] + CharSet.disjunction + operandList[2] + ")" },
                { Weights[1], "(" + operandList[0] + CharSet.disjunction + operandList[1] + CharSet.disjunction + CharSet.inversion + operandList[2] + ")" },
                { Weights[2], "(" + operandList[0] + CharSet.disjunction + CharSet.inversion + operandList[1] + CharSet.disjunction + operandList[2] + ")" },
                { Weights[3], "(" + operandList[0] + CharSet.disjunction + CharSet.inversion + operandList[1] + CharSet.disjunction + CharSet.inversion + operandList[2] + ")" },
                { Weights[4], "(" + CharSet.inversion + operandList[0] + CharSet.disjunction + operandList[1] + CharSet.disjunction + operandList[2] + ")" },
                { Weights[5], "(" + CharSet.inversion + operandList[0] + CharSet.disjunction + operandList[1] + CharSet.disjunction + CharSet.inversion + operandList[2] + ")" },
                { Weights[6], "(" + CharSet.inversion + operandList[0] + CharSet.disjunction + CharSet.inversion + operandList[1] + CharSet.disjunction + operandList[2] + ")" },
                { Weights[7], "(" + CharSet.inversion + operandList[0] + CharSet.disjunction + CharSet.inversion + operandList[1] + CharSet.disjunction + CharSet.inversion + operandList[2] + ")" }
            };
            return tableCombinations;
        }
    }
}

// !((a+!b)*(!c+b)*!(a*c))