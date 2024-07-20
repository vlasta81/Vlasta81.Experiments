using System.Text.RegularExpressions;

namespace Bin2Dec
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(Help.GetUseHelp());
                return;
            }
            switch (args[0])
            {
                case "-h":               
                case "-help":
                    Console.Clear();
                    Console.WriteLine(Help.GetHelp());
                    break;
                case "-d":
                    if (args.Length > 1 && int.TryParse(args[1].Trim(), out int DecimalNumber))
                    {
                        Console.WriteLine("Decimal: " + DecimalNumber);
                        Console.WriteLine(" Binary: " + Convert.ToString(DecimalNumber, 2));
                    }
                    else
                    {
                        Console.WriteLine(Help.GetUseHelp());
                    }
                    break;
                case "-b":
                    if (args.Length > 1 && Regex.IsMatch(args[1].Trim(), "^[01]{1,8}$"))
                    {
                        Console.WriteLine(" Binary: " + args[1].Trim());
                        Console.WriteLine("Decimal: " + Convert.ToInt32(args[1].Trim(), 2).ToString());
                    }
                    else
                    {
                        Console.WriteLine(Help.GetUseHelp());
                    }
                    break;
                default:
                    Console.WriteLine(Help.GetUseHelp());
                    break;
            }
        }
    }
}
