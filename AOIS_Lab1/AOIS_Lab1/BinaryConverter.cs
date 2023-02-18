using System.Text;

namespace AOIS_Lab1
{
	internal static class BinaryConverter
	{
		public static string TranslateDexToBinaryString(double x, Code code)
		{
			if (x >= 0)
			{
				return Convert.ToString((int)x | 1024, 2)[1..];
			}
			switch (code)
			{
				case Code.straight:
					return Convert.ToString((int)-x | 512, 2);
				case Code.reversed:
					return Convert.ToString((int)-x ^ 511, 2).PadLeft(10, '1');
				case Code.additional:
					return Convert.ToString((int)x, 2)[^10..];
			}

			throw new Exception();
		}

		internal static string TranslateReversedToAdditionalCode(string binary)
		{
			if (binary[0] == '0')
			{
				return binary;
			}
			StringBuilder newString = new StringBuilder(binary);
			for (int i = newString.Length - 1; i >= 0; i--)
			{
				if (binary[i] == '0') 
				{
					newString[i] = '1';
					break;		
				}
			}

			return newString.ToString();
		}

		internal static string TranslateStraightToAdditionalCode(string binary)
		{
			if (binary[0] == '0')
			{
				return binary;
			}

			StringBuilder newString = new StringBuilder(binary);
			for (int i = 1; i < newString.Length; i++)
			{
				if (newString[i] == '0')
				{
					newString[i] = '1';
				}
				else
				{
					newString[i] = '0';
				}
			}

			return TranslateReversedToAdditionalCode(newString.ToString());
		}

		internal static int TranslateBinaryStringToDex(string binary)
		{
			bool isNegative = false;
			if (binary[0] == '1')
			{
				isNegative = true;
			}

			int intValue = Convert.ToInt32(binary, 2);

			if (isNegative)
			{
				int complement = intValue ^ ((1 << binary.Length) - 1);
				intValue = -(complement + 1);
			}

			return intValue;
		}

		internal static double TranslateBinaryNonFloatToDexFloatString(string result, double divider)
		{
			var splittedResult = result.Split('.');
			var quotient = BinaryConverter.TranslateBinaryStringToDex(splittedResult[0]);
			var remainder = Double.Parse(BinaryConverter.TranslateBinaryStringToDex('0' + splittedResult[1]).ToString()) / divider;
			return remainder + quotient;
		}

		internal static string AssembleBinaryFloatString(double mantissa, int exponent, int mantissaBits, int exponentBits)
		{
			int signBit = mantissa < 0 ? 1 : 0;

			int bias = (int)Math.Pow(2, exponentBits - 1) - 1;

			int exponentValue = exponent + bias;
			string exponentBinary = Convert.ToString(exponentValue, 2).PadLeft(exponentBits, '0');
			string mantissaBinary = Convert.ToString(mantissa)[2..].PadRight(mantissaBits, '0');

			string binaryString = signBit.ToString() + exponentBinary + mantissaBinary;

			int totalBits = signBit + exponentBits + mantissaBits;
			if (binaryString.Length < totalBits)
			{
				binaryString = binaryString.PadRight(totalBits, '0');
			}

			return binaryString;
		}

		internal static double TranslateBinaryFloatToDexFloat(string binary)
		{
			int sign = binary[0] == '0' ? 1 : -1;
			int exponent = Convert.ToInt32(binary.Substring(1, 8), 2) - 127;
			string mantissaBinary = binary.Substring(9);
			float mantissa = 1;

			for (int i = 0; i < mantissaBinary.Length; i++)
			{
				if (mantissaBinary[i] == '1')
				{
					mantissa += (float)Math.Pow(2, -(i + 1));
				}
			}

			return sign * mantissa * (float)Math.Pow(2, exponent);
		}

		internal static string BinaryIntArrayToBinaryString(int[] result, int maxLength)
		{
			StringBuilder sb = new StringBuilder();
			bool foundLeadingOne = false;
			for (int i = 0; i < result.Length; i++)
			{
				if (result[i] == 1)
				{
					foundLeadingOne = true;
				}

				if (foundLeadingOne)
				{
					sb.Append(result[i]);
				}
			}

			return sb.ToString().PadLeft(maxLength, '0');
		}
	}
}