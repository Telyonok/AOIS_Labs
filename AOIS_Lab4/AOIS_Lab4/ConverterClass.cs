namespace AOIS_Lab2
{
	public static class ConverterClass
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

        private static string PrepareForOutput(string formula)
		{
			formula = formula.Replace(CharSet.disjunction.ToString(), CharSet.trueDisjunction);
			formula = formula.Replace(CharSet.conjunction.ToString(), CharSet.trueConjunction);
			return formula;
		}

		private static void AssembleNumericForms(out string numericSdnf, out string numericSknf, string index)
		{
			numericSdnf = CharSet.trueDisjunction + "(";
			numericSknf = CharSet.trueConjunction + "(";
			for (int charIterator = 0; charIterator < index.Length; charIterator++)
			{
				if (index[charIterator] == '1')
					numericSdnf += charIterator.ToString() + ',';
				else
					numericSknf += charIterator.ToString() + ",";
			}
			numericSdnf = numericSdnf[..^1] + ")";
			numericSknf = numericSknf[..^1] + ")";
		}

		private static int GetFunctionIndex(string input, List<string> operandList)
		{
			int index = 0;
            Console.WriteLine("Таблица:");
            for (int setIterator = 0; setIterator < setsAndWeights.Count; setIterator++)
            {
                var set = setsAndWeights.Keys.ElementAt(setIterator);
                Console.Write($"{set}: ");
                string formula = input.Replace(operandList[0], set[0].ToString()).Replace(operandList[1], set[1].ToString()).Replace(operandList[2], set[2].ToString());
                if (Evaluate(formula) == "1")
                {
                    index += setsAndWeights[set];
                    Console.WriteLine("1");
                }
                else
                    Console.WriteLine("0");
            }
            return index;
        }

        private static string Evaluate(string formula)
        {
            if (formula[0] == '(' && formula[^1] == ')')
                formula = formula[1..^1];
            if (formula.Length == 1)
                return formula;
            Stack<int> openBracketIndex = new Stack<int>();
            for (int charIterator = 0; charIterator < formula.Length; charIterator++)
            {
                if (formula[charIterator] == '(')
                    openBracketIndex.Push(charIterator);
                else if (formula[charIterator] == ')')
                {
                    int bracketIndex = openBracketIndex.Pop();
                    int length = charIterator - bracketIndex + 1;
                    formula = formula.Substring(0, bracketIndex) + Evaluate(formula.Substring(bracketIndex, length)) + formula.Substring(charIterator + 1);
                    charIterator = bracketIndex;
                }
            }
            formula = formula.Replace(CharSet.inversion + "0", "1").Replace(CharSet.inversion + "1", "0");
            return (formula.Contains(CharSet.disjunction) && formula.Contains("1")) || !formula.Contains("0")? "1" : "0";
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