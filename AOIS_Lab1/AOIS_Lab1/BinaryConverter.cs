using System.Text;

namespace AOIS_Lab1
{
	public static class BinaryConverter
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

		public static string TranslateReversedToAdditionalCode(string binary)
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

		public static string TranslateStraightToAdditionalCode(string binary)
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

		public static int TranslateBinaryStringToDex(string binary)
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

		public static double TranslateBinaryNonFloatToDexFloatString(string result, double divider)
		{
			var splittedResult = result.Split('.');
			var quotient = TranslateBinaryStringToDex(splittedResult[0]);
			var remainder = Double.Parse(TranslateBinaryStringToDex('0' + splittedResult[1]).ToString()) / divider;
			return remainder + quotient;
		}

		public static string TranslateDecimalToBinaryFloat(double number)
		{
			int bits = BitConverter.ToInt32(BitConverter.GetBytes((float)number), 0);
			bool sign = (bits >> 31) != 0;
			int exponent = (int)((bits >> 23) & 255);
			int fraction = bits & 8388607;
			exponent -= 127;
			string signString = sign ? "1" : "0";
			string exponentString = Convert.ToString(exponent + 127, 2).PadLeft(8, '0');
			string fractionString = Convert.ToString(fraction, 2).PadLeft(23, '0');

			return signString + exponentString + fractionString;
		}

		public static string BinaryIntArrayToBinaryString(int[] result, int maxLength)
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

		public static string TranslateDexFloatToBinaryFloat(float num)
		{
			int intPart = (int)num;
			string intPartBinary = Convert.ToString(intPart, 2);

			float fractionPart = num - intPart;
			string fractionPartBinary = "";
			int i = 10;
			while (fractionPart > 0 && i > 0)
			{
				fractionPart *= 2;
				if (fractionPart >= 1)
				{
					fractionPartBinary += "1";
					fractionPart -= 1;
				}
				else
					fractionPartBinary += "0";
				i--;
			}

			return intPartBinary + "." + fractionPartBinary;
		}
	}
}