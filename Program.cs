using System;
using System.Collections.Generic;
using System.Data;

class Calculator
{
    static void Main()
    {
        List<string> history = new List<string>();

        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ══════════════════════ ");
            Console.WriteLine("       CALCULATOR       ");
            Console.WriteLine("  ═════════════════════ ");
            Console.WriteLine("                        ");
            Console.WriteLine("   Enter calculation    ");
            Console.WriteLine("   C = Clear            ");
            Console.WriteLine("   H = History          ");
            Console.WriteLine("   Q = Quit             ");
            Console.WriteLine(" ══════════════════════ ");
            Console.ResetColor();

            Console.Write("\n > ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.ToUpper() == "Q")
                break;

            if (input.ToUpper() == "C")
                continue;

            if (input.ToUpper() == "H")
            {
                Console.WriteLine("\n--- HISTORY ---");

                if (history.Count == 0)
                    Console.WriteLine("No calculations.");
                else
                    foreach (string item in history)
                        Console.WriteLine(item);

                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
                continue;
            }

            try
            {
                object answer = new DataTable().Compute(input, "");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n = " + answer);
                Console.ResetColor();

                history.Add(input + " = " + answer);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Invalid calculation!");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        Console.Clear();
        Console.WriteLine("Calculator Closed.");
    }
}