
namespace ConfigurationStorage
{
    /// <summary>
    /// Represents a storage for configuration settings.
    /// </summary>
    public partial class ConfigurationStorage
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ConfigurationStorage"/> class.
        /// </summary>
        /// <param name="directoryPath">The path to the directory where the configuration files are stored.</param>
        /// <param name="fileExtension">The file extension for the configuration files.</param>
        /// <param name="secured">Indicates whether the configuration storage is secured.</param>
        /// <returns>A new instance of the <see cref="ConfigurationStorage"/> class.</returns>
        public static ConfigurationStorage Create(string directoryPath, string fileExtension, bool secured)
        {
            return new ConfigurationStorage(directoryPath, fileExtension, secured);
        }
    }
}
