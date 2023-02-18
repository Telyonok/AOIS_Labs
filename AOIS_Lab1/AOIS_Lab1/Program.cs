namespace AOIS_Lab1
{
	class Program
	{
		const double initialX1 = 13;
		const double initialX2 = 27;
		const double mantissa1 = 0.1;
		const double mantissa2 = 0.101;
		const int initialFloat1Exp = 13;
		const int initialFloat2Exp = 27;
		const string float1 = "Мантисса = 0.1, порядок = 13";
		const string float2 = "Мантисса = 0.101, порядок = 27";
		static void Main()
		{
			if (GetUserInputVariant() == 2)
			{
				InitiateFloatSpecificWorkflow();
			}
			else
			{
				InitiateGeneralWorkflow();
			}
		}

		private static void InitiateFloatSpecificWorkflow()
		{
			var firstNumber = BinaryConverter.AssembleBinaryFloatString(mantissa1, initialFloat1Exp, 24, 8);
			var secondNumber = BinaryConverter.AssembleBinaryFloatString(mantissa2, initialFloat2Exp, 24, 8);
			var result = BinaryCalculator.SumFloatingPointNumbers(firstNumber, secondNumber, 24, 8);

			Console.WriteLine($"\nСумма = {result}");
		}

		private static void InitiateGeneralWorkflow()
		{
			double x1 = initialX1;
			double x2 = initialX2;
			Operation operation = GetUserInputOperation();
			ApplyUserInputSignCombination(ref x1, ref x2);
			Console.WriteLine($"\nПолученные числа: {x1}, {x2}.");
			Code code = GetUserInputCode();
			string binaryX1 = BinaryConverter.TranslateDexToBinaryString(x1, code);
			string binaryX2 = BinaryConverter.TranslateDexToBinaryString(x2, code);
			Console.WriteLine($"\nПереведённые в двоичную систему числа: {binaryX1}, {binaryX2}.");
			var variableMinus = '+';
			if (operation != Operation.Sum)
			{
				if (x1 * x2 < 0)
					variableMinus = '-';
				binaryX1 = BinaryConverter.TranslateDexToBinaryString(Math.Abs(x1), code);
				binaryX2 = BinaryConverter.TranslateDexToBinaryString(Math.Abs(x2), code);
			}
			string result = BinaryCalculator.Calculate(binaryX1, binaryX2, operation, code);
			double dexValue = TranslateResultToDex(result, operation, x2);
			Console.WriteLine($"\nРезультат операции в дополнительном коде: {result}\n" +
			$"или {dexValue}\nБез модуля результат имеет знак {variableMinus}");
		}

		private static double TranslateResultToDex(string result, Operation operation, double x2)
		{
			if (operation == Operation.Division)
				return BinaryConverter.TranslateBinaryNonFloatToDexFloatString(result, Math.Abs(x2));
			else
				return BinaryConverter.TranslateBinaryStringToDex(result);
		}

		private static int GetUserInputVariant()
		{
			Console.WriteLine($"Выберите вариант:\n1) Числа 13 и 27\n2) Числа {float1} и {float2}\n");
			var key = Console.ReadKey();
			Console.WriteLine();
			switch (key.KeyChar)
			{
				case '1':
					return 1;
				case '2':
					return 2;
				default:
					WrongInputMessage();
					return GetUserInputVariant();
			}
		}

		private static Operation GetUserInputOperation()
		{
			Console.WriteLine("Выберите операцию:\n1) Сложение\n2) Произведение\n3) Деление\n");
			var key = Console.ReadKey();
			switch (key.KeyChar)
			{
				case '1':
					return Operation.Sum;
				case '2':
					return Operation.Multiplication;
				case '3':
					return Operation.Division;
				default:
					WrongInputMessage();
					return GetUserInputOperation();
			}
		}

		private static Code GetUserInputCode()
		{
			Console.WriteLine("Выберите кодировку:\n1) Прямая\n2) Обратная\n3) Дополнительная\n");
			var key = Console.ReadKey();
			switch (key.KeyChar)
			{
				case '1':
					return Code.straight;
				case '2':
					return Code.reversed;
				case '3':
					return Code.additional;
				default:
					WrongInputMessage();
					return GetUserInputCode();
			}
		}

		private static void ApplyUserInputSignCombination(ref double x1, ref double x2)
		{
			Console.WriteLine("\nВыберите знаки:\n1) +/+\n2) +/-\n3) -/+\n4) -/-\n");
			var key = Console.ReadKey();
			switch (key.KeyChar)
			{
				case '1':
					break;
				case '2':
					x2 = -x2;
					break;
				case '3':
					x1 = -x1;
					break;
				case '4':
					x1 = -x1;
					x2 = -x2;
					break;
				default:
					WrongInputMessage();
					ApplyUserInputSignCombination(ref x1, ref x2);
					break;
			}
		}

		private static void WrongInputMessage()
		{
			Console.WriteLine("\n******Wrong input. Try again.******\n");
		}
	}

	enum Code
	{
		straight,
		reversed,
		additional
	}

	enum Operation
	{
		Sum,
		Multiplication,
		Division
	}
}