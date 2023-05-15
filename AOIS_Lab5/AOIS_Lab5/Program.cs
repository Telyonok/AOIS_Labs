using AOIS_Lab2;
using System;

namespace AOIS_Lab5
{
    internal class Program
    {
        const string h1SDNF = "v+v+v+v+v+v+v+v";
        const string h2SDNF = "v*!q1'+v*!q1'+v*!q1'+v*!q1'";
        const string h3SDNF = "v*!q1'*!q2'+v*!q1'*!q2'";
        const string q1SDNF = "!h1*q1p+h1*!q1p+!h1*q1p+h1*!q1p+!h1*q1p+h1*!q1p";
        const string q2SDNF = "!h2*q2p+!h2*q2p+!h2*q2p+h2*!q2p+!h2*q2p+h2*!q2p+!h2*q2p+h2*!q2p";
        const string q3SDNF = "!h3*q3p+!h3*q3p+!h3*q3p+!h3*q3p+!h3*q3p+!h3*q3p+!h3*q3p+h3*!q3p";
        const string q1rSDNF = "!h1*q1+h1*!q1+!h1*q1+h1*!q1+!h1*q1+h1*!q1+!h1*q1+h1*!q1";
        const string q2rSDNF = "!h2*q2+!h2*!q2+!h2*q2+h2*!q2+!h2*q2+!h2*!q2+!h2*q2+h2*!q2";
        const string q3rSDNF = "!h3*q3+!h3*q3+!h3*q3+!h3*q3+!h3*q3+!h3*q3+!h3*q3+h3*!q3";
        static void Main()
        {
            Console.WriteLine("Таблица истинности:");
            DrawTruthTable(new List<string>() { "q1", "q2", "q3", "v", "h1", "h2", "h3", "q1'", "q2'", "q3'" });
            Console.Write("h1 = ");
            Console.WriteLine(h1SDNF);
            Console.Write("h2 = ");
            Console.WriteLine(h2SDNF);
            Console.Write("h3 = ");
            Console.WriteLine(h3SDNF);
            Console.Write("q1 = ");
            Console.WriteLine(q1SDNF);
            Console.Write("q2 = ");
            Console.WriteLine(q2SDNF);
            Console.Write("q3 = ");
            Console.WriteLine(q3SDNF);
            Console.Write("q1' = ");
            Console.WriteLine(q1rSDNF);
            Console.Write("q2' = ");
            Console.WriteLine(q2rSDNF);
            Console.Write("q3' = ");
            Console.WriteLine(q3rSDNF);
            Console.WriteLine("\nМинимизированные функции:");
            Console.Write("h1 = ");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(h1SDNF, new() {"v"}));
            Console.Write("h2 = ");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(h2SDNF, new() {"v", "q1'"}));
            Console.Write("h3 = ");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(h3SDNF, new() {"v", "q1'", "q2'"}));
            Console.Write("q1 = ");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(q1SDNF, new() {"h1", "q1p"}));
            Console.Write("q2 = ");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(q2SDNF, new() {"h2", "q2p"}));
            Console.Write("q3 = ");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(q3SDNF, new() {"h3", "q3p"}));
            Console.Write("q1' = ");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(q1rSDNF, new() {"h1", "q1"}));
            Console.Write("q2' = ");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(q2rSDNF, new() {"h2", "q2"}));
            Console.Write("q3' = ");
            Console.WriteLine(MinimizerClass.MinimizeSdnf(q3rSDNF, new() {"h3", "q3"}));
        }

        private static void DrawTruthTable(List<string> operands)
        {
            Console.WriteLine($"| {operands[3]} |{operands[2]} |{operands[1]} |{operands[0]} |{operands[6]} |{operands[5]} |{operands[4]} |{operands[9]}|{operands[8]}|{operands[7]}|");
            Console.WriteLine("| 0 | 1 | 1 | 1 | 0 | 0 | 0 | 1 | 1 | 1 |");
            Console.WriteLine("| 1 | 1 | 1 | 0 | 0 | 0 | 1 | 1 | 1 | 1 |");
            Console.WriteLine("| 0 | 1 | 1 | 0 | 0 | 0 | 0 | 1 | 1 | 0 |");
            Console.WriteLine("| 1 | 1 | 0 | 1 | 0 | 1 | 1 | 1 | 1 | 0 |");
            Console.WriteLine("| 0 | 1 | 0 | 1 | 0 | 0 | 0 | 1 | 0 | 1 |");
            Console.WriteLine("| 1 | 1 | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 |");
            Console.WriteLine("| 0 | 1 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 |");
            Console.WriteLine("| 1 | 0 | 1 | 1 | 1 | 1 | 1 | 1 | 0 | 0 |");
            Console.WriteLine("| 0 | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 1 | 1 |");
            Console.WriteLine("| 1 | 0 | 1 | 0 | 0 | 0 | 1 | 0 | 1 | 1 |");
            Console.WriteLine("| 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 1 | 0 |");
            Console.WriteLine("| 1 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 |");
            Console.WriteLine("| 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 1 |");
            Console.WriteLine("| 1 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 |");
            Console.WriteLine("| 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |");
            Console.WriteLine("| 1 | 1 | 1 | 1 | 1 | 1 | 1 | 0 | 0 | 0 |");
        }
    }
}