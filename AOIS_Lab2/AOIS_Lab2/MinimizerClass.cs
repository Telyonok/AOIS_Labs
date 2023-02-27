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
		}

		private static string MinimizeSknf(string sknf, List<string> operandList)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<string> splittedSknf = new List<string>(sknf.Split(con));
			foreach (var operand in operandList)
			{
				for (int i = 0; i < splittedSknf.Count; i++)
				{
					var replacedPart = splittedSknf[i].Replace(inv + operand, "").Replace(operand, "");
					for (int j = i + 1; j < splittedSknf.Count; j++)
					{
						if (splittedSknf[j].Replace(inv + operand, "").Replace(operand, "") == replacedPart)
						{
							replacedPart = replacedPart.Replace(dis + ")", ")").Replace("(" + dis, "(").Replace(dis + dis.ToString(), dis.ToString());
							stringBuilder.Append(replacedPart);
							stringBuilder.Append(con);
							splittedSknf.RemoveAt(i);
							splittedSknf.RemoveAt(j - 1);
							break;
						}
					}
				}
			}
			return stringBuilder.ToString()[..^1];
		}

		private static string MinimizeSdnf(string sdnf, List<string> operandList)
		{

			StringBuilder stringBuilder = new StringBuilder();
			List<string> splittedSdnf = new List<string>(sdnf.Split(dis));
			foreach (var operand in operandList)
			{
				for (int i = 0; i < splittedSdnf.Count; i++)
				{
					var replacedPart = splittedSdnf[i].Replace(inv + operand, "").Replace(operand, "");
					for (int j = i + 1; j < splittedSdnf.Count; j++)
					{
						if (splittedSdnf[j].Replace(inv + operand, "").Replace(operand, "") == replacedPart)
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
							break;
						}
					}
				}
			}

			return stringBuilder.ToString()[..^1];
		}
	}
}
