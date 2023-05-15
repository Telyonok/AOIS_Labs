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
            if (replacedPart.Length > 0)
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
            List<string> splittedCopy = new List<string>(splittedSdnf);
            if (splittedSdnf.Count < 2)
                return sdnf;
            StringBuilder stringBuilder = new StringBuilder();
            GlueParts(splittedSdnf, operandList, stringBuilder, false);
            GlueParts(stringBuilder.ToString().Split(CharSet.disjunction).ToList<string>(), operandList, stringBuilder.Clear(), false);
            if (operandList.Contains("v"))
                return splittedCopy[0];
            else
                return splittedCopy[0] + CharSet.disjunction + splittedCopy[^1];;
        }
    }
}
