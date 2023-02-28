namespace AOIS_Lab1
{
	internal static class BinaryCalculator
	{
		private static string Sum(string binary1, string binary2)
		{
			int maxLength = Math.Max(binary1.Length, binary2.Length);
			int carry = 0;
			string result = "";

			for (int i = maxLength - 1; i >= 0; i--)
			{
				int sum = carry;
				sum += binary1[i] - '0';
				sum += binary2[i] - '0';

				if (sum % 2 == 0)
					result = "0" + result;
				else
					result = "1" + result;

				carry = sum / 2;
			}

			return result;
		}

		private static string Multiply(string binary1, string binary2)
		{
			int maxLength = Math.Max(binary1.Length, binary2.Length);
			int[] result = new int[maxLength * 2];

			for (int i = maxLength - 1; i >= 0; i--)
			{
				for (int j = maxLength - 1; j >= 0; j--)
				{
					int product = (binary1[i] - '0') * (binary2[j] - '0');
					int sum = product + result[i + j + 1];

					result[i + j + 1] = sum % 2;
					result[i + j] += sum / 2;
				}
			}

			return BinaryConverter.BinaryIntArrayToBinaryString(result, maxLength);
		}

		private static string Divide(string dividend, string divisor)
		{
			int dividendInt = BinaryConverter.TranslateBinaryStringToDex(dividend);
			int divisorInt = BinaryConverter.TranslateBinaryStringToDex(divisor);

			float resultDex = (float)dividendInt / divisorInt;

			return BinaryConverter.TranslateDexFloatToBinaryFloat(resultDex);
		}

		internal static string Calculate(string binaryX1, string binaryX2, Operation operation, Code code)
		{
			if (code == Code.straight)
			{
				binaryX1 = BinaryConverter.TranslateStraightToAdditionalCode(binaryX1);
				binaryX2 = BinaryConverter.TranslateStraightToAdditionalCode(binaryX2);
			}
			else if (code == Code.reversed)
			{
				binaryX1 = BinaryConverter.TranslateReversedToAdditionalCode(binaryX1);
				binaryX2 = BinaryConverter.TranslateReversedToAdditionalCode(binaryX2);
			}

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

		internal static string SumFloatingPointNumbers(string num1, string num2)
		{
			string exponent1 = num1.Substring(1, 8);
			string exponent2 = num2.Substring(1, 8);
			string mantissa1 = "1" + num1.Substring(9, 23);
			string mantissa2 = "1" + num2.Substring(9, 23);
			int exponentDifference = Convert.ToInt32(exponent1, 2) - Convert.ToInt32(exponent2, 2);
			if (exponentDifference > 0)
				mantissa2 = ShiftRight(mantissa2, exponentDifference);
			else if (exponentDifference < 0)
			{
				mantissa1 = ShiftRight(mantissa1, -exponentDifference);
				exponent1 = exponent2;
			}
			string exponentSum = exponent1;
			string mantisSum = SumBinary(mantissa1, mantissa2);
			if (mantisSum[0] == '0')
				Normalize(ref mantisSum, ref exponentSum);
			else if (mantisSum.Length > 24)
			{
				mantisSum = '0' + mantisSum.Substring(1);
				exponentSum = SumBinary(exponentSum, "00000001");
			}
			return "0" + exponentSum + mantisSum.Substring(1);
		}

		private static void Normalize(ref string mantisSum, ref string exponentSum)
		{
			int shift = 1;
			while (mantisSum[shift] == '0')
			{
				shift++;
			}
			mantisSum = ShiftLeft(mantisSum, shift);
			exponentSum = SumBinary(exponentSum, Convert.ToString(shift - 1, 2).PadLeft(8, '0'));
		}

		private static string SumBinary(string num1, string num2)
		{
			string result = "";
			int carry = 0;
			for (int i = num1.Length - 1; i >= 0; i--)
			{
				int sum = Convert.ToInt32(num1[i].ToString()) + Convert.ToInt32(num2[i].ToString()) + carry;
				result = (sum % 2).ToString() + result;
				carry = sum / 2;
			}
			if (carry > 0)
			{
				result = "1" + result;
			}
			return result;
		}

		private static string ShiftRight(string num, int bits)
		{
			return new string('0', bits) + num.Substring(0, num.Length - bits);
		}

		private static string ShiftLeft(string num, int bits)
		{
			return num.Substring(bits, num.Length - bits) + new string('0', bits);
		}
	}
}
