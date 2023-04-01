using System.Collections;
using System.Linq;
using System.Text;

namespace AOIS_Lab2
{
	public static class MinimizerClass
	{
		public static void ExecuteMinimizeSdnfSknfTask(List<string> operandList, string sdnf, string sknf)
		{
			string minimizedSdnf = MinimizeSdnf(sdnf, operandList);
			string minimizedSknf = MinimizeSknf(sknf, operandList);
			Console.WriteLine($"Минимизированная СДНФ {minimizedSdnf}");
			Console.WriteLine($"Минимизированная СКНФ {minimizedSknf}");
		}

		public static string MinimizeSknf(string sknf, List<string> operandList)
        {
            List<string> splittedSknf = new List<string>(sknf.Split(CharSet.conjunction));
            if (splittedSknf.Count < 2)
                return sknf;
            StringBuilder stringBuilder = new StringBuilder();
			GlueParts(splittedSknf, operandList, stringBuilder, true);
			GlueParts(stringBuilder.ToString().Split(CharSet.conjunction).ToList<string>(), operandList, stringBuilder.Clear(), true);
			return string.Join(CharSet.conjunction, RemoveRedundantParts(operandList, stringBuilder.ToString().Split(CharSet.conjunction), CharSet.disjunction));
		}

        private static void MergeSknfParts(string replacedPart, StringBuilder stringBuilder, List<string> splittedSknf, int index1, int index2)
        {
            replacedPart = replacedPart.Replace(CharSet.disjunction + ")", ")").Replace("(" + CharSet.disjunction, "(").Replace(CharSet.disjunction + CharSet.disjunction.ToString(), CharSet.disjunction.ToString());
            stringBuilder.Append(replacedPart).Append(CharSet.conjunction);
            splittedSknf.RemoveAt(index1);
            splittedSknf.RemoveAt(index2 - 1);
        }
        
        private static void MergeSdnfParts(string replacedPart, StringBuilder stringBuilder, List<string> splittedSdnf, int index1, int index2)
        {
            if (replacedPart[0] == CharSet.conjunction)
                stringBuilder.Append(replacedPart[1..]);
            else if (replacedPart[^1] == CharSet.conjunction)
                stringBuilder.Append(replacedPart[..^1]);
            else
                stringBuilder.Append(replacedPart.Replace(CharSet.conjunction + CharSet.conjunction.ToString(), CharSet.conjunction.ToString()));
            stringBuilder.Append(CharSet.disjunction);
            splittedSdnf.RemoveAt(index1);
            splittedSdnf.RemoveAt(index2 - 1);
        }

        private static void GlueParts(List<string> splittedParts, List<string> operandList, StringBuilder stringBuilder, bool isSknf)
        {
            char delimiter = isSknf ? CharSet.conjunction : CharSet.disjunction;
            foreach (var operand in operandList)
            {
                for (int partIterator1 = 0; partIterator1 < splittedParts.Count; partIterator1++)
                {
                    var replacedPart = splittedParts[partIterator1].Replace(CharSet.inversion + operand, "").Replace(operand, "");
                    for (int partIterator2 = partIterator1 + 1; partIterator2 < splittedParts.Count; partIterator2++)
                    {
                        if (splittedParts[partIterator2].Replace(CharSet.inversion + operand, "").Replace(operand, "") == replacedPart)
                        {
                            if (isSknf)
                                MergeSknfParts(replacedPart, stringBuilder, splittedParts, partIterator1, partIterator2);
                            else
                                MergeSdnfParts(replacedPart, stringBuilder, splittedParts, partIterator1, partIterator2);
                            partIterator1--;
                            break;
                        }
                    }
                }
            }
            foreach (string part in splittedParts)
                stringBuilder.Append(part).Append(delimiter);
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        private static string[] RemoveRedundantParts(List<string> operandList, string[] splittedString, char sign)
        {
            foreach (var operand in operandList)
            {
                for (int partIterator = 0; partIterator < splittedString.Length; partIterator++)
                {
                    if (splittedString[partIterator].Contains(operand))
                    {
                        var removedInverseS = splittedString[partIterator].Replace(CharSet.inversion + operand, operand);
                        for (int index2 = partIterator + 1; index2 < splittedString.Length; index2++)
                        {
                            if (splittedString[index2].Replace(CharSet.inversion + operand, operand).Contains(removedInverseS.Replace("(","").Replace(")","")))
                            {
                                splittedString[index2] = splittedString[index2].Replace(CharSet.inversion + operand, operand).Replace(sign + operand, "").Replace(operand + sign, "");
                            }
                        }
                    }
                }
            }
            return splittedString;
        }

        public static string MinimizeSdnf(string sdnf, List<string> operandList)
        {
            List<string> splittedSdnf = new List<string>(sdnf.Split(CharSet.disjunction));
            if (splittedSdnf.Count < 2)
                return sdnf;
            StringBuilder stringBuilder = new StringBuilder();
            GlueParts(splittedSdnf, operandList, stringBuilder, false);
            GlueParts(stringBuilder.ToString().Split(CharSet.disjunction).ToList<string>(), operandList, stringBuilder.Clear(), false);
            ExtractRedundantOperands(stringBuilder.ToString().Split(CharSet.disjunction).ToList<string>(), operandList, stringBuilder.Clear());
            return string.Join(CharSet.disjunction, RemoveRedundantParts(operandList, stringBuilder.ToString().Split(CharSet.disjunction), CharSet.conjunction));
        }

        private static void ExtractRedundantOperands(List<string> splittedString, List<string> operandList, StringBuilder stringBuilder)
        {
            foreach (var operand in operandList)
            {
                List<List<string>> partsWithInversedChosenOperand = new List<List<string>>();
                List<List<string>> partsWithChosenOperand = new List<List<string>>();
                Dictionary<List<string>, string> cutPartsToParts = new Dictionary<List<string>, string>();
                foreach (var part in splittedString)
                {
                    var cutPart = part.Split(CharSet.conjunction).ToList();
                    if (part.Contains(CharSet.inversion + operand))
                    {
                        cutPart.Remove(CharSet.inversion + operand);
                        partsWithInversedChosenOperand.Add(cutPart);
                        cutPartsToParts.Add(cutPart, part);
                    }
                    else if (part.Contains(operand))
                    {
                        cutPart.Remove(operand);
                        partsWithChosenOperand.Add(cutPart);
                        cutPartsToParts.Add(cutPart, part);
                    }
                }

                DeleteRedundantOperands(partsWithChosenOperand, partsWithInversedChosenOperand, stringBuilder, splittedString, cutPartsToParts);
            }
            stringBuilder.Append(string.Join(CharSet.disjunction, splittedString));
        }

        private static void DeleteRedundantOperands(List<List<string>> partsWithChosenOperand, List<List<string>> partsWithInversedChosenOperand, StringBuilder stringBuilder, List<string> splittedString, Dictionary<List<string>, string> cutPartsToParts)
        {
            foreach (List<string> otherOperands in partsWithChosenOperand)
            {
                foreach (List<string> otherOperands2 in partsWithInversedChosenOperand)
                {
                    List<string> matchingList = (from op2 in otherOperands2
                                                 where otherOperands.Contains(op2)
                                                 select op2).ToList();
                    if (matchingList.Count() == otherOperands.Count())
                    {
                        stringBuilder.Append(string.Join(CharSet.conjunction, otherOperands2));
                        stringBuilder.Append(CharSet.disjunction);
                        splittedString.Remove(cutPartsToParts[otherOperands2]);
                    }
                }
            }
        }
    }
}
