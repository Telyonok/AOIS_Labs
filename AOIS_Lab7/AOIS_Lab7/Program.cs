namespace AOIS_Lab7
{
    internal class Program
    {
        static void Main()
        {
            List<List<int>> matrix = GenerateMatrix(8, 3);
            List<int> vector = GenerateVector(3);
            Print(matrix);
            var a = Compare(matrix, matrix[1]);
            var b = FilterWithRange(matrix, matrix[0], matrix[1]);
            var c = FilterWithFunction(matrix, matrix[2], XOR);
            var d = FilterWithFunction(matrix, matrix[2], OR);
            var e = FilterWithFunction(matrix, matrix[2], AND);
            var f = FilterWithFunction(matrix, matrix[2], Equivalance);
        }

        public static void Print(List<List<int>> matrix)
        {
            for (int rowIterator = 0; rowIterator < matrix.Count; rowIterator++)
            {
                for (int columnIterator = 0; columnIterator < matrix[0].Count; columnIterator++)
                {
                    Console.Write(matrix[rowIterator][columnIterator]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public static List<int> Compare(List<List<int>> matrix, List<int> register)
        {
            List<int> output = new List<int>();
            int wordLength = matrix[0].Count;

            for (int matrixIterator = 0; matrixIterator < matrix.Count; matrixIterator++)
            {
                int totalMatrixSuperiority = 0;
                int totalRegisterSuperiority = 0;
                int currentMatrixSuperiority;
                int currentRegisterSuperiority;

                for (int wordIterator = 0; wordIterator < wordLength; wordIterator++)
                {
                    currentMatrixSuperiority = totalMatrixSuperiority | (~register[wordIterator] & matrix[matrixIterator][wordIterator] & ~totalRegisterSuperiority);
                    currentRegisterSuperiority = totalRegisterSuperiority | (register[wordIterator] & ~matrix[matrixIterator][wordIterator] & ~totalMatrixSuperiority);
                    totalMatrixSuperiority = currentMatrixSuperiority;
                    totalRegisterSuperiority = currentRegisterSuperiority;
                }

                if (totalMatrixSuperiority == 0 && totalRegisterSuperiority == 0)
                    output.Add(0);
                else if (totalMatrixSuperiority == 1 && totalRegisterSuperiority == 0)
                    output.Add(1);
                else if (totalMatrixSuperiority == 0 && totalRegisterSuperiority == 1)
                    output.Add(-1);
            }
            return output;
        }

        public static List<int> FilterWithRange(List<List<int>> matrix, List<int> lower, List<int> greater)
        {
            List<int> output = new List<int>();
            List<int> lowerFlag = Compare(matrix, lower);
            List<int> upperFlag = Compare(matrix, greater);

            for (int matrixIterator = 0; matrixIterator < matrix.Count; matrixIterator++)
            {
                if (lowerFlag[matrixIterator] >= 0 && upperFlag[matrixIterator] <= 0)
                    output.Add(1);
                else
                    output.Add(0);
            }

            return output;
        }

        public static List<int> FilterWithFunction(List<List<int>> matrix, List<int> mask, Func<List<int>, List<int>, List<int>, bool> filterFunction)
        {
            List<int> output = new List<int>();
            int wordPartLength = matrix[0].Count / 3;
            for (int matrixIterator = 0; matrixIterator < matrix.Count; matrixIterator++)
            {
                List<int> word = matrix[matrixIterator];
                List<int> firstPart = word.GetRange(word.Count - wordPartLength, wordPartLength);
                List<int> secondPart = word.GetRange(word.Count - 2 * wordPartLength, wordPartLength);
                List<int> resultPart = word.GetRange(word.Count - 3 * wordPartLength, wordPartLength);
                if (filterFunction(firstPart, secondPart, resultPart))
                    output.Add(1);
                else
                    output.Add(0);
            }
            return output;
        }

        public static bool XOR(List<int> firstPart, List<int> secondPart, List<int> resultPart)
        {
            for (int i = 0; i < firstPart.Count; i++)
                if ((firstPart[i] ^ secondPart[i]) != resultPart[i])
                    return false;
            return true;
        }

        public static bool AND(List<int> firstPart, List<int> secondPart, List<int> resultPart)
        {
            for (int i = 0; i < firstPart.Count; i++)
                if ((firstPart[i] & secondPart[i]) != resultPart[i])
                    return false;
            return true;
        }

        public static bool OR(List<int> firstPart, List<int> secondPart, List<int> resultPart)
        {
            for (int i = 0; i < firstPart.Count; i++)
                if ((firstPart[i] | secondPart[i]) != resultPart[i])
                    return false;
            return true;
        }

        public static bool Equivalance(List<int> firstPart, List<int> secondPart, List<int> resultPart)
        {
            for (int i = 0; i < firstPart.Count; i++)
                if ((firstPart[i] == secondPart[i]) != Convert.ToBoolean(resultPart[i]))
                    return false;
            return true;
        }

        public static List<List<int>> GenerateMatrix(int height, int width)
        {
            List<List<int>> matrix = new List<List<int>>();
            Random random = new Random();
            for (int heightCounter = 0; heightCounter < height; heightCounter++)
            {
                matrix.Add(new List<int>());
                for (int widthCounter = 0; widthCounter < width; widthCounter++)
                    matrix[heightCounter].Add(random.Next(2));
            }
            return matrix;
        }

        public static List<int> GenerateVector(int length)
        {
            List<int> vector = new List<int>();
            Random random = new Random();
            for (int lengthCounter = 0; lengthCounter < length; lengthCounter++)
                vector.Add(random.Next(2));
            return vector;
        }
    }
}