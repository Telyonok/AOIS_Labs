namespace AOIS_Lab1
{
	class Program
	{
		static void Main()
		{
			if (GetUserInputVariant() == 2)
			{
				double initialFloatX1;
				double initialFloatX2;
				Console.WriteLine("Введите первое вещественное число");
				initialFloatX1 = Convert.ToDouble(Console.ReadLine());
				Console.WriteLine("Введите второе вещественное число");
				initialFloatX2 = Convert.ToDouble(Console.ReadLine());
				InitiateFloatSpecificWorkflow(initialFloatX1, initialFloatX2);
			}
			else
			{
				double initialX1;
				double initialX2;
				Console.WriteLine("Введите первое число");
				initialX1 = Convert.ToInt32(Console.ReadLine());
				Console.WriteLine("Введите второе число");
				initialX2 = Convert.ToInt32(Console.ReadLine());
				InitiateGeneralWorkflow(initialX1, initialX2);
			}
		}

		private static void InitiateFloatSpecificWorkflow(double initialFloatX1, double initialFloatX2)
		{
			var firstNumber = BinaryConverter.TranslateDecimalToBinaryFloat(initialFloatX1);
			var secondNumber = BinaryConverter.TranslateDecimalToBinaryFloat(initialFloatX2);
			Console.WriteLine($"\nПервое число: {firstNumber}\nВторое число: {secondNumber}");
			var result = BinaryCalculator.SumFloatingPointNumbers(firstNumber, secondNumber);

			Console.WriteLine($"\nСумма = {result}");
		}

		private static void InitiateGeneralWorkflow(double initialX1, double initialX2)
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
			Console.WriteLine($"Выберите вариант:\n1) Операции над целыми числами\n2) Операции над числами с плавающей запятой\n");
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

	public enum Code
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