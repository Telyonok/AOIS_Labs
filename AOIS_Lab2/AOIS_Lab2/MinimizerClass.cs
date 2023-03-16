using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_Lab2
{
	internal static class MinimizerClass
	{
		const char inv = '!';
		const char con = '*';
		const char dis = '+';
		const string trueCon = "/\\";
		const string trueDis = "\\/";

		public static void ExecuteMinimizeSdnfSknfTask(List<string> operandList, string sdnf, string sknf)
		{
			string minimizedSdnf = MinimizeSdnf(sdnf, operandList);
			string minimizedSknf = MinimizeSknf(sknf, operandList);
			Console.WriteLine($"Минимизированная СДНФ {minimizedSdnf}");
			Console.WriteLine($"Минимизированная СКНФ {minimizedSknf}");
		}

		private static string MinimizeSknf(string sknf, List<string> operandList)
        {
            List<string> splittedSknf = new List<string>(sknf.Split(con));
            if (splittedSknf.Count < 2)
                return sknf;
            StringBuilder stringBuilder = new StringBuilder();
			GlueSknfParts(splittedSknf, operandList, stringBuilder);
			GlueSknfParts(stringBuilder.ToString().Split(con).ToList<string>(), operandList, stringBuilder.Clear());
			return string.Join(con, RemoveRedundantParts(operandList, stringBuilder.ToString().Split(con), dis));
		}

        private static void GlueSknfParts(List<string> splittedSknf, List<string> operandList, StringBuilder stringBuilder)
		{
            foreach (var operand in operandList)
            {
                for (int i = 0; i < splittedSknf.Count; i++)
                {
                    var replacedPart = splittedSknf[i].Replace(inv + operand, "").Replace(operand, "");
                    for (int j = i + 1; j < splittedSknf.Count; j++)
                    {
                        if (splittedSknf[j].Replace(inv + operand, "").Replace(operand, "") == replacedPart)
                        {
                            MergeSknfParts(replacedPart, stringBuilder, splittedSknf, i, j);
                            break;
                        }
                    }
                }
            }
            foreach (string s in splittedSknf)
                stringBuilder.Append(s).Append(con);
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        private static void MergeSknfParts(string replacedPart, StringBuilder stringBuilder, List<string> splittedSknf, int i, int j)
        {
            replacedPart = replacedPart.Replace(dis + ")", ")").Replace("(" + dis, "(").Replace(dis + dis.ToString(), dis.ToString());
            stringBuilder.Append(replacedPart).Append(con);
            splittedSknf.RemoveAt(i);
            splittedSknf.RemoveAt(j - 1);
        }
        
        private static void MergeSdnfParts(string replacedPart, StringBuilder stringBuilder, List<string> splittedSdnf, int i, int j)
        {
            if (replacedPart[0] == con)
                stringBuilder.Append(replacedPart[1..]);
            else if (replacedPart[^1] == con)
                stringBuilder.Append(replacedPart[..^1]);
            else
                stringBuilder.Append(replacedPart.Replace(con + con.ToString(), con.ToString()));
            stringBuilder.Append(dis);
            splittedSdnf.RemoveAt(i);
            splittedSdnf.RemoveAt(j - 1);
        }

        private static void GlueSdnfParts(List<string> splittedSdnf, List<string> operandList, StringBuilder stringBuilder)
		{
            foreach (var operand in operandList)
            {
                for (int i = 0; i < splittedSdnf.Count; i++)
                {
                    var replacedPart = splittedSdnf[i].Replace(inv + operand, "").Replace(operand, "");
                    for (int j = i + 1; j < splittedSdnf.Count; j++)
                    {
                        if (splittedSdnf[j].Replace(inv + operand, "").Replace(operand, "") == replacedPart)
                        {
                            MergeSdnfParts(replacedPart, stringBuilder, splittedSdnf, i, j);
                            break;
                        }
                    }
                }
            }
            foreach (string s in splittedSdnf)
                stringBuilder.Append(s).Append(dis);
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

		private static string[] RemoveRedundantParts(List<string> operandList, string[] splittedString, char sign)
		{
            foreach (var operand in operandList)
            {
                for (int i = 0; i < splittedString.Length; i++)
                {
                    if (splittedString[i].Contains(operand))
                    {
                        var removedInverseS = splittedString[i].Replace(inv + operand, operand);
                        for (int j = i + 1; j < splittedString.Length; j++)
                        {
                            if (splittedString[j].Replace(inv + operand, operand).Contains(removedInverseS.Replace("(","").Replace(")","")))
                            {
                                splittedString[j] = splittedString[j].Replace(inv + operand, operand).Replace(sign + operand, "").Replace(operand + sign, "");
                            }
                        }
                    }
                }
            }
            return splittedString;
		}

		private static string MinimizeSdnf(string sdnf, List<string> operandList)
        {
            List<string> splittedSdnf = new List<string>(sdnf.Split(dis));
            if (splittedSdnf.Count < 2)
                return sdnf;
            StringBuilder stringBuilder = new StringBuilder();
            GlueSdnfParts(splittedSdnf, operandList, stringBuilder);
            GlueSdnfParts(stringBuilder.ToString().Split(dis).ToList<string>(), operandList, stringBuilder.Clear());
            return string.Join(dis, RemoveRedundantParts(operandList, stringBuilder.ToString().Split(dis), con));
        }
	}
}
