using System.Xml;

namespace AOIS_Lab1
{
	class Program
	{
		const double initialX1 = 13;
		const double initialX2 = 27;
		const double initialFloat1 = 16.3660304;
		const double initialFloat2 = 34.0693434;
		const string float1 = "13e0.1";
		const string float2 = "27e0.101";
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
			var answer = BinaryCalculator.CalculateFloatingPoint(initialFloat1, initialFloat2);
			Console.WriteLine($"\nСумма = {answer} или {BinaryConverter.TranslateToBinaryFloat(answer)}");
		}

		private static void InitiateGeneralWorkflow()
		{
			double x1 = initialX1;
			double x2 = initialX2;

			ApplyUserInputSignCombination(ref x1, ref x2);
			Console.WriteLine($"\nПолученные числа: {x1}, {x2}.");

			Code code = GetUserInputCode();
			string binaryX1 = BinaryConverter.TranslateToBinary(x1, code);
			string binaryX2 = BinaryConverter.TranslateToBinary(x2, code);
			Console.WriteLine($"\nПереведённые в двоичную систему числа: {binaryX1}, {binaryX2}.");

			Operation operation = GetUserInputOperation();
			string result = BinaryCalculator.Calculate(Convert.ToString((int)x1, 2), Convert.ToString((int)x2, 2), operation);
			Console.WriteLine();
			if (operation != Operation.Division)
			{
				Console.Write(BinaryConverter.TranslateToBinary(Convert.ToInt32(result, 2), code));
				Console.WriteLine($" или {Convert.ToInt32(result, 2)}");
			}
			else
				Console.Write(result);
		}

		private static int GetUserInputVariant()
		{
			Console.WriteLine($"Выберите вариант:\n1) Числа 13 и 27\n2) Числа {float1} и {float2}\n");
			var key = Console.ReadKey();
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
			Console.WriteLine("Выберите кодировку:\n1) Прямая\n2) Обратная\n3) Дополнительная\n4) С плавающей точкой\n");
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