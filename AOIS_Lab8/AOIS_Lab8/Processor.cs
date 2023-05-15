namespace AOIS_Lab8
{
    public class Processor
    {
        public List<List<int>> matrix;
        public Processor(int size)
        {
            matrix = ToDiagonal(GenerateMatrix(size, size));
        }

        public void Operate(int firstAddress, int secondAddress, int destinationAddress, Func<List<int>, List<int>, List<int>> logicalFunction)
        {
            var firstList = Get(firstAddress);
            var secondList = Get(secondAddress);
            Add(destinationAddress, logicalFunction(firstList, secondList));
        }

        public void Sum(List<int> filter)
        {
            for (int wordIterator = 0; wordIterator < matrix.Count; wordIterator++)
            {
                var word = Get(wordIterator);
                if (word[0] == filter[0] && word[1] == filter[1] && word[2] == filter[2])
                    SumWord(wordIterator);
            } 
        }

        private void SumWord(int wordIterator)
        {
            var wordToSum = Get(wordIterator);
            var a = new List<int>() { wordToSum[3], wordToSum[4], wordToSum[5], wordToSum[6] };
            var b = new List<int>() { wordToSum[7], wordToSum[8], wordToSum[9], wordToSum[10] };
            var sum = Sum(a, b);
            wordToSum[11] = sum[0];
            wordToSum[12] = sum[1];
            wordToSum[13] = sum[2];
            wordToSum[14] = sum[3];
            wordToSum[15] = sum[4];
            Add(wordIterator, wordToSum);
        }

        private static List<int> Sum(List<int> firstList, List<int> secondList)
        {
            int carry = 0;
            List<int> sum = new List<int>();

            for (int i = firstList.Count - 1; i >= 0; i--)
            {
                int digit1 = firstList[i];
                int digit2 = secondList[i];

                int digitSum = digit1 + digit2 + carry;
                sum.Insert(0, digitSum % 2);
                carry = digitSum / 2;
            }

            sum.Insert(0, carry);

            return sum;
        }

        public static List<int> FirstArgumentRepition(List<int> firstList, List<int> secondList)
        {
            List<int> result = new List<int>();
            for (int digitIterator = 0; digitIterator < firstList.Count(); digitIterator++) 
            {
                result.Add(firstList[digitIterator]);
            }
            return result;
        }

        public List<int> GetNearestLower(List<int> bound)
        {
            var compareResult = Compare(matrix, bound);
            var lowerList = new List<List<int>>();
            for (int resultIterator = 0; resultIterator < compareResult.Count(); resultIterator++)
                if (compareResult[resultIterator] == -1)
                    lowerList.Add(Get(resultIterator));
            if (lowerList.Count == 0)
                return new List<int>();
            return GetMaxList(lowerList);
        }

        public List<int> GetNearestUpper(List<int> bound)
        {
            var compareResult = Compare(matrix, bound);
            var upperList = new List<List<int>>();
            for (int resultIterator = 0; resultIterator < compareResult.Count(); resultIterator++)
                if (compareResult[resultIterator] == 1)
                    upperList.Add(Get(resultIterator));
            if (upperList.Count == 0)
                return new List<int>();
            return GetMinList(upperList);
        }

        public static List<int> GetMaxList(List<List<int>> lists)
        {
            if (lists.Count == 1)
                return lists[0];
            List<List<int>> processedLists = new List<List<int>>();
            var listToCompareBy = lists[0];
            foreach (var list in lists)
                if (CompareLists(list, listToCompareBy) == 1)
                    processedLists.Add(list);
            if (processedLists.Count == 0)
                return listToCompareBy;
            return GetMaxList(processedLists);
        }

        public static List<int> GetMinList(List<List<int>> lists)
        {
            if (lists.Count == 1)
                return lists[0];
            List<List<int>> processedLists = new List<List<int>>();
            var listToCompareBy = lists[0];
            foreach (var list in lists)
                if (CompareLists(list, listToCompareBy) == -1)
                    processedLists.Add(list);
            if (processedLists.Count == 0)
                return listToCompareBy;
            return GetMinList(processedLists);
        }

        public static List<int> Sheffer(List<int> firstList, List<int> secondList)
        {
            List<int> result = new List<int>();
            for (int digitIterator = 0; digitIterator < firstList.Count(); digitIterator++)
            {
                result.Add(Convert.ToInt32(!Convert.ToBoolean(firstList[digitIterator] * secondList[digitIterator])));
            }
            return result;
        }

        public static List<int> FirstArgumentInverse(List<int> firstList, List<int> secondList)
        {
            List<int> result = new List<int>();
            for (int digitIterator = 0; digitIterator < firstList.Count(); digitIterator++)
            {
                result.Add(Convert.ToInt32(!Convert.ToBoolean(firstList[digitIterator])));
            }
            return result;
        }

        public static List<int> Conjunction(List<int> firstList, List<int> secondList)
        {
            List<int> result = new List<int>();
            for (int digitIterator = 0; digitIterator < firstList.Count(); digitIterator++)
            {
                result.Add(firstList[digitIterator] * secondList[digitIterator]);
            }
            return result;
        }

        public void Print()
        {
            for (int rowIterator = 0; rowIterator < matrix.Count; rowIterator++)
            {
                for (int columnIterator = 0; columnIterator < matrix.Count; columnIterator++)
                {
                    Console.Write(matrix[rowIterator][columnIterator]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public List<int> Get(int address)
        {
            List<int> value = new List<int>();
            for (int matrixIterator = 0; matrixIterator < matrix.Count; matrixIterator++)
            {
                var shiftedIndex = (matrixIterator + address) % matrix.Count;
                value.Add(matrix[shiftedIndex][matrixIterator]);
            }
            return value;
        }

        public void Add(int address, List<int> value)
        {
            for (int matrixIterator = 0; matrixIterator < value.Count; matrixIterator++)
            {
                var shiftedIndex = (matrixIterator + address) % matrix.Count;
                matrix[shiftedIndex][matrixIterator] = value[matrixIterator];
            }
        }

        public static List<List<int>> ToDiagonal(List<List<int>> matrix)
        {
            List<List<int>> output = GenerateMatrix(16, 16);

            int offset = 0;
            for (int rowIterator = 0; rowIterator < matrix.Count; rowIterator++)
            {
                for (int columnIterator = 0; columnIterator < matrix.Count; columnIterator++)
                {
                    var shiftedIndex = (columnIterator + offset) % matrix.Count;
                    output[shiftedIndex][rowIterator] = matrix[rowIterator][columnIterator];
                }
                offset++;
            }

            return output;
        }

        public static int CompareLists(List<int> firstList, List<int> secondList)
        {
            int totalFirstSuperiority = 0;
            int totalSecondSuperiority = 0;
            int currentMatrixSuperiority;
            int currentRegisterSuperiority;

            for (int lengthCounter = 0; lengthCounter < firstList.Count; lengthCounter++)
            {
                currentMatrixSuperiority = totalFirstSuperiority | (~secondList[lengthCounter] & firstList[lengthCounter] & ~totalSecondSuperiority);
                currentRegisterSuperiority = totalSecondSuperiority | (secondList[lengthCounter] & ~firstList[lengthCounter] & ~totalFirstSuperiority);
                totalFirstSuperiority = currentMatrixSuperiority;
                totalSecondSuperiority = currentRegisterSuperiority;
            }

            if (totalFirstSuperiority == 0 && totalSecondSuperiority == 0)
                return 0;
            else if (totalFirstSuperiority == 1 && totalSecondSuperiority == 0)
                return 1;
            else
                return -1;
        }

        public static List<int> Compare(List<List<int>> matrix, List<int> register)
        {
            List<int> output = new List<int>();

            for (int matrixIterator = 0; matrixIterator < matrix.Count; matrixIterator++)
                output.Add(CompareLists(matrix[matrixIterator], register));
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
