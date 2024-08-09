using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Security.Principal;

namespace Pathman
{
    internal static class Helpers
    {
        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    internal class AppCommands
    {
        public static void AddPathCommand(string directoryPath, bool systemEnvironment, bool addToBeginning)
        {
            Console.WriteLine($"(i) Starting.");
            string systemEnvironmentName = systemEnvironment ? "system" : "user";
            EnvironmentVariableTarget environmentType = systemEnvironment ? EnvironmentVariableTarget.Machine : EnvironmentVariableTarget.User;
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            Console.WriteLine($"(i) Getting \"Path\" {systemEnvironmentName} environment variable.");
            List<string> paths = Environment.GetEnvironmentVariable("Path", environmentType).Split(';').ToList();
            Console.WriteLine($"(i) Looking for directory path in \"Path\" {systemEnvironmentName} environment variable.");
            if (paths.FindIndex(a => a.Contains(di.FullName)) == -1) // -1 == not found
            {
                if (addToBeginning)
                {                    
                    Console.WriteLine($"(i) The directory path is added to the beginning {systemEnvironmentName} environment variable.");
                    paths.Insert(0, di.FullName);
                }
                else
                {
                    Console.WriteLine($"(i) The directory path is added to the {systemEnvironmentName} environment variable.");
                    paths.Add(di.FullName);
                }
                Console.WriteLine($"(i) The \"Path\" environment is stored.");
                string newPaths = string.Join(";", paths.ToArray());
                if (environmentType == EnvironmentVariableTarget.User)
                {
                    Environment.SetEnvironmentVariable("Path", newPaths, EnvironmentVariableTarget.User);
                    Console.WriteLine($"(i) Done.");
                }
                else if (Helpers.IsAdministrator() && environmentType == EnvironmentVariableTarget.Machine)
                {
                    Environment.SetEnvironmentVariable("Path", newPaths, EnvironmentVariableTarget.Machine);
                    Console.WriteLine($"(i) Done.");
                }
                else
                {
                    Console.WriteLine($"(e) You need administrator permissions to remove the directory path from the {systemEnvironmentName} environment variable!");
                }
            }
            else
            {
                Console.WriteLine($"(w) Directory path is already exists in \"Path\" {systemEnvironmentName} environment variable.");
            }
        }
        public static void RemovePathCommand(string directoryPath, bool systemEnvironment)
        {
            Console.WriteLine($"(i) Starting.");
            string systemEnvironmentName = systemEnvironment ? "system" : "user";
            EnvironmentVariableTarget environmentType = systemEnvironment ? EnvironmentVariableTarget.Machine : EnvironmentVariableTarget.User;
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            Console.WriteLine($"(i) Getting \"Path\" {systemEnvironmentName} environment variable.");
            List<string> paths = Environment.GetEnvironmentVariable("Path", environmentType).Split(';').ToList();
            Console.WriteLine($"(i) Looking for directory path in \"Path\" {systemEnvironmentName} environment variable.");
            int index = paths.FindIndex(a => a.Contains(di.FullName)); // -1 == not found
            if (index == -1)
            {
                Console.WriteLine($"(w) Directory path does not exists in \"Path\" {systemEnvironmentName} environment variable.");
            }
            else
            {
                Console.WriteLine($"(i) The directory path is removed from the {systemEnvironmentName} environment variable.");
                paths.RemoveAt(index);
                Console.WriteLine($"(i) The \"Path\" environment is stored.");
                string newPaths = string.Join(";", paths.ToArray());
                if (environmentType == EnvironmentVariableTarget.User)
                {
                    Environment.SetEnvironmentVariable("Path", newPaths, EnvironmentVariableTarget.User);
                    Console.WriteLine($"(i) Done.");
                }
                else if (Helpers.IsAdministrator() && environmentType == EnvironmentVariableTarget.Machine)
                {
                    Environment.SetEnvironmentVariable("Path", newPaths, EnvironmentVariableTarget.Machine);
                    Console.WriteLine($"(i) Done.");
                }
                else
                {
                    Console.WriteLine($"(e) You need administrator permissions to remove the directory path from the {systemEnvironmentName} environment variable!");
                }
            }
        }
    }

    public class Program
    {
        static async Task Main(string[] args)
        {
            Option<string> directoryOption = new Option<string>("--directory", "The directory path that is added to the \"Path\" environment variable.");
            directoryOption.IsRequired = true;
            directoryOption.ArgumentHelpName = "path";
            directoryOption.AddAlias("-d");

            Option<bool> systemEnvironmentOption = new Option<bool>("--system", "Select if you want to add/remove the directory path to/from the system instead of user environment \"Path\" variable. (Required administrator permissions!)");
            systemEnvironmentOption.SetDefaultValue(false);
            systemEnvironmentOption.AddAlias("-s");

            Option<bool> addToBeginningOption = new Option<bool>("--first", "Select if you want to add the directory path to the beginning of the \"Path\" environment variable.");
            addToBeginningOption.SetDefaultValue(false);
            addToBeginningOption.AddAlias("-f");

            RootCommand rootCommand = new RootCommand("Pathman is a CLI tool managing the windows \"Path\" environment variable.");

            Command addCommand = new Command("add", "Add path to environment variable.");
            addCommand.AddOption(directoryOption);
            addCommand.AddOption(systemEnvironmentOption);
            addCommand.AddOption(addToBeginningOption);
            addCommand.SetHandler(AppCommands.AddPathCommand, directoryOption, systemEnvironmentOption, addToBeginningOption);
            rootCommand.Add(addCommand);

            Command removeCommand = new Command("remove", "Remove path from environment variable.");
            removeCommand.AddOption(directoryOption);
            removeCommand.AddOption(systemEnvironmentOption);
            removeCommand.SetHandler(AppCommands.RemovePathCommand, directoryOption, systemEnvironmentOption);
            rootCommand.Add(removeCommand);

            CommandLineBuilder commandLineBuilder = new CommandLineBuilder(rootCommand);

            commandLineBuilder.AddMiddleware(async (context, next) =>
            {
                await next(context);
            });

            commandLineBuilder.UseDefaults();

            Parser parser = commandLineBuilder.Build();

            await parser.InvokeAsync(args);
        }

    }
}
