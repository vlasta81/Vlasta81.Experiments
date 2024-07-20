
namespace Bin2Dec
{
    internal static class Help
    {
        public static string GetUseHelp()
        {
            return "Enter \"Bin2Dec -h\" or \"Bin2Dec -help\" to view help.";
        }

        public static string GetHelp()
        {
            return $@"
Bin2Dec - Convert binary numbers to decimal and vice versa.

USING:
    Bin2Dec -b [binary number] 
        Convert binary number to decimal.
        [binary number] can be only 0 or 1 in the range of 1 to 8 characters

    Bin2Dec -d [decimal number]
        Convert decimal number to binary.
        [decimal number] can be only integers in the range of 1 to {Int32.MaxValue}

    Bin2Dec -h [or] -help
        Show this help.

EXAMPLES:
    Bin2Dec -b 10111
    Bin2Dec -d 3452
    Bin2Dec -h
    Bin2Dec -help
";
        }
    }
}
