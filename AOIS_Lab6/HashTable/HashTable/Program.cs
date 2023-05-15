using HashTable;
using System.Runtime.ConstrainedExecution;
using System;

namespace Lab_6
{
    internal class Program
    {
        // Коллизии
        const string characters = "0123456789 abcdefghijklmnopqrstuvwxyz!\\\"#$%&'()*+,-./:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`{|}~";
        static void Main()
        {
            MyHashTable hashTable = new MyHashTable(5, Hash);
            hashTable.SetItem("Einstein", "General Theory of Relativity");
            hashTable.SetItem("Maxwell", "Maxwell's Equations");
            hashTable.SetItem("Heisenberg", "Uncertainty Principle");
            hashTable.SetItem("Feynman", "Feynman Diagrams");
            hashTable.SetItem("Bohr", "Bohr Model");
            hashTable.SetItem("Schrödinger", "Schrödinger Equation");
            hashTable.SetItem("Hawking", "Hawking Radiation");
            hashTable.SetItem("Fermi", "Fermi Paradox");
            hashTable.SetItem("Curie", "Curie's Law");
            hashTable.SetItem("Cq", "Perhaps a mistake?");
            Console.WriteLine(hashTable);
            Console.WriteLine(hashTable.GetItem("Curie"));
            hashTable.SetItem("Curie", "Curie's Law 2");
            Console.WriteLine(hashTable.GetItem("Curie"));
            hashTable.Remove("Curie");
        }

        public static int Hash(string value, int tableSize)
        {
            var charsToIndexes = new Dictionary<char, int>();
            for (int i = 0; i < characters.Length; i++)
                charsToIndexes.Add(characters[i], i);
            int output = 0;
            value = value.ToLower();
            for (int i = 0; i < 2; i++)
                output += charsToIndexes[value[i]] * (int)Math.Pow(31, 1 - i);
            return output % tableSize;
        }
    }
}