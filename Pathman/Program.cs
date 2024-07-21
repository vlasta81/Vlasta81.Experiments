using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Pathman
{
    internal class Program
    {
        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator);
        }
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(Help.GetUseHelp());
                return;
            }
            if (!Directory.Exists(args[1].Trim()))
            {
                Console.WriteLine("Directory does not exist!");
                return;
            }
            bool AddToBeginning = (args.Length > 2 && args[2].Trim() == "-f") ? true : false;

            var name = "PATH";
            var scope = EnvironmentVariableTarget.Machine; // or User
            var oldValue = Environment.GetEnvironmentVariable(name, scope);
            var newValue = oldValue + @";C:\Program Files\MySQL\MySQL Server 5.1\bin\\";
            Environment.SetEnvironmentVariable(name, newValue, scope);

            switch (args[0])
            {
                case "-h":
                case "-help":
                    Console.Clear();
                    Console.WriteLine(Help.GetHelp());
                    break;
                case "-s":
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
                case "-u":
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
