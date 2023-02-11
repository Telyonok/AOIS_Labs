namespace AOIS_Lab1
{
	internal static class BinaryConverter
	{
		public static string TranslateToBinary(double x, Code code)
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
					return Convert.ToString((int)-x ^ 511, 2);
				case Code.additional:
					return Convert.ToString((int)x, 2)[^10..];
			}

			throw new Exception();
		}

		internal static string TranslateToBinaryFloat(double x2)
		{
			double realPart = (int)x2;
			double fractionPart = x2 - realPart;
			string result = TranslateToBinary(realPart, Code.straight);
			result += ".";
			for (int i = 0; i < 5 && fractionPart != 1; i++)
			{
				fractionPart *= 2;
				if (fractionPart > 1)
				{
					result += "1";
					fractionPart -= 1;
				}
				else
				{
					result += "0";
				}
			}

			return result;
		}
	}
}