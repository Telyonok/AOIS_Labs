using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_Lab1
{
	internal static class BinaryCalculator
	{
		private static string Sum(string x1, string x2)
		{
			int result = Convert.ToInt32(x1, 2) + Convert.ToInt32(x2, 2);
			return Convert.ToString(result, 2);
		}

		private static string Multiply(string x1, string x2)
		{
			int result = Convert.ToInt32(x1, 2) * Convert.ToInt32(x2, 2);
			return Convert.ToString(result, 2);
		}

		private static string Divide(string x1, string x2)
		{
			double result = (double)Convert.ToInt32(x1, 2) / Convert.ToInt32(x2, 2);
			return BinaryConverter.TranslateToBinaryFloat(Math.Abs(result));
		}

		internal static string Calculate(string binaryX1, string binaryX2, Operation operation)
		{
			switch (operation)
			{
				case Operation.Sum:
					return Sum(binaryX1, binaryX2);
				case Operation.Multiplication:
					return Multiply(binaryX1, binaryX2);
				case Operation.Division:
					return Divide(binaryX1, binaryX2);
			}

			throw new Exception();
		}

		internal static double CalculateFloatingPoint(double float1, double float2)
		{
			double result = float1 + float2;
			return result;
		}
	}
}
