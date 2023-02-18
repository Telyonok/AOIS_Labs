using System.Numerics;
using System.Text;

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
				{
					result = "0" + result;
				}
				else
				{
					result = "1" + result;
				}

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

			int quotientInt = dividendInt / divisorInt;
			int remainderInt = dividendInt % divisorInt;

			string quotient = BinaryConverter.TranslateDexToBinaryString(quotientInt, Code.additional);
			string remainder = BinaryConverter.TranslateDexToBinaryString(remainderInt, Code.additional);

			return quotient + "." + remainder;
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

		internal static string SumFloatingPointNumbers(string binaryNumber1, string binaryNumber2, int mantissaBits, int exponentBits)
		{
			int sign1 = int.Parse(binaryNumber1.Substring(0, 1));
			int exponent1 = Convert.ToInt32(binaryNumber1.Substring(1, exponentBits), 2);
			int mantissa1 = Convert.ToInt32(binaryNumber1.Substring(exponentBits + 1), 2);
			if (sign1 == 1)
			{
				mantissa1 = -mantissa1;
			}

			int sign2 = int.Parse(binaryNumber2.Substring(0, 1));
			int exponent2 = Convert.ToInt32(binaryNumber2.Substring(1, exponentBits), 2);
			int mantissa2 = Convert.ToInt32(binaryNumber2.Substring(exponentBits + 1), 2);
			if (sign2 == 1)
			{
				mantissa2 = -mantissa2;
			}

			int newExponent;
			int exponentDiff = exponent1 - exponent2;
			if (exponent1 > exponent2)
			{
				newExponent = exponent1;
				mantissa2 = mantissa2 >> exponentDiff;
			}
			else
			{
				newExponent = exponent2;
				mantissa1 = mantissa1 >> -exponentDiff;
			}

			int newMantissa = mantissa1 + mantissa2;

			while (newMantissa >= (1 << mantissaBits))
			{
				newMantissa = newMantissa >> 1;
				newExponent++;
			}
			while (newMantissa < (1 << mantissaBits - 1))
			{
				newMantissa = newMantissa << 1;
				newExponent--;
			}

			int newSign = newMantissa < 0 ? 1 : 0;
			newMantissa = Math.Abs(newMantissa);
			string newExponentBinary = Convert.ToString(newExponent, 2).PadLeft(exponentBits, '0');
			string newMantissaBinary = Convert.ToString(newMantissa, 2).PadLeft(mantissaBits, '0');
			string newBinaryNumber = newSign.ToString() + newExponentBinary + newMantissaBinary;

			return newBinaryNumber;
		}
	}
}
