namespace HashTable
{
    public class MyHashTable
    {
        public int Size { get; private set; }
        public List<List<Tuple<string, string>>> Container { get; private set; }
        public Func<string, int, int> Hash { get; private set; }

        public MyHashTable(int tableSize, Func<string, int, int> hashFunction)
        {
            Size = tableSize;
            Container = new List<List<Tuple<string, string>>>(tableSize);
            for (int i = 0; i < tableSize; i++)
                Container.Add(new List<Tuple<string, string>>());
            Hash = hashFunction;
        }

        public void SetItem(string key, string value)
        {
            int keyHash = Hash(key, Size);
            var keys = Container[keyHash].Select(pair => pair.Item1).ToList();

            if (!keys.Contains(key))
                Container[keyHash].Add(new Tuple<string, string>(key, value));
            else
            {
                int keyId = keys.IndexOf(key);
                Container[keyHash][keyId] = new Tuple<string, string>(key, value);
            }
        }

        public string GetItem(string key)
        {
            int keyHash = Hash(key, Size);
            foreach (var pair in Container[keyHash])
                if (pair.Item1 == key)
                    return pair.Item2;

            throw new ArgumentException("No such key.");
        }

        public void Remove(string key)
        {
            int keyHash = Hash(key, Size);
            var keys = Container[keyHash].Select(pair => pair.Item1).ToList();
            int keyId = keys.IndexOf(key);
            if (keyId == -1)
                throw new ArgumentException("No such key.");
            Container[keyHash].RemoveAt(keyId);
        }

        public override string ToString()
        {
            string outString = "";
            var indicies = Enumerable.Range(0, Size).Where(i => Container[i].Count > 0).ToList();

            foreach (int i in indicies)
            {
                outString += $"Hash {i} \n";
                foreach (var pair in Container[i])
                    outString += $"\tKey: {pair.Item1,-15} | Value: {pair.Item2,-15} \n";
            }
            outString += $"\n\n";
            outString += $"Size of HashTable: {Size} \n";
            outString += $"Occupied hashes: {indicies.Count} \n";
            outString += $"Hashes with collisions: {indicies.Count(i => Container[i].Count > 1)}";

            return outString;
        }
    }
}
