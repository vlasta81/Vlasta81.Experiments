using System.Security;

namespace ConfigurationStorage
{
    /// <summary>
    /// Represents the method that will handle an event when data changes.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="DataChangedEventArgs"/> that contains the event data.</param>
    public delegate void DataChangedEventHandler(object sender, DataChangedEventArgs e);

    /// <summary>
    /// Represents a storage for configuration data.
    /// </summary>
    public partial class ConfigurationStorage
    {
        /// <summary>
        /// Path to the directory where configuration files are stored.
        /// </summary>
        private readonly string _directoryPath;

        /// <summary>
        /// Indicates whether the storage is secured.
        /// </summary>
        private readonly bool _secured;

        /// <summary>
        /// Instance of the HybridEncryptor used for encryption.
        /// </summary>
        private readonly HybridEncryptor _hybridEncryptor;

        /// <summary>
        /// Dictionary to store configuration data.
        /// </summary>
        private Dictionary<string, string> _data;

        /// <summary>
        /// Gets the file extension used for configuration files.
        /// </summary>
        public string FileExtension { get; init; }

        /// <summary>
        /// Event triggered when data changes.
        /// </summary>
        public event DataChangedEventHandler? DataChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationStorage"/> class.
        /// </summary>
        /// <param name="directoryPath">The path to the directory where configuration files are stored.</param>
        /// <param name="fileExtension">The file extension used for configuration files.</param>
        /// <param name="secured">Indicates whether the storage is secured.</param>
        private ConfigurationStorage(string directoryPath, string fileExtension = "config", bool secured = false)
        {
            _data = new Dictionary<string, string>();
            _directoryPath = directoryPath;
            _secured = secured;
            _hybridEncryptor = new HybridEncryptor();
            FileExtension = fileExtension.Trim().TrimStart('.');
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            LoadFiles();
        }

        /// <summary>
        /// Loads configuration files from the directory.
        /// </summary>
        private void LoadFiles()
        {
            string[] files = Directory.GetFiles(_directoryPath, $"*.{FileExtension}", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string content = File.ReadAllText(file);
                _data.Add(fileName, content);
            }
        }

        /// <summary>
        /// Invokes the DataChanged event.
        /// </summary>
        /// <param name="dataKeyName">The name of the data key that changed.</param>
        /// <param name="oldValue">The old value of the data key.</param>
        /// <param name="newValue">The new value of the data key.</param>
        protected virtual void OnDataChanged(string dataKeyName, object? oldValue, object? newValue)
        {
            DataChanged?.Invoke(this, new DataChangedEventArgs(dataKeyName, oldValue, newValue));
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        private string GetValue(string key)
        {
            return _data[key];
        }

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="content">The value to set.</param>
        private void SetValue(string key, string content)
        {
            string? oldValue = _data[key] ?? null;
            File.WriteAllText(Path.Combine(_directoryPath, key + $".{FileExtension}"), content);
            _data[key] = content;
            OnDataChanged(key, oldValue, content);
        }

        /// <summary>
        /// Reads the value associated with the specified key as a secure string.
        /// </summary>
        /// <param name="key">The key of the value to read.</param>
        /// <returns>The value associated with the specified key as a secure string.</returns>
        public SecureString ReadAsSecureString(string key)
        {
            string content = GetValue(key).Split("::")[1];
            SecureString secureString = new SecureString();
            string preparedContent = _hybridEncryptor.DecryptString(_secured, key, content);
            foreach (char c in preparedContent)
            {
                secureString.AppendChar(c);
            }
            secureString.MakeReadOnly();
            return secureString;
        }

        /// <summary>
        /// Gets the type of the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value type is to be retrieved.</param>
        /// <returns>The type of the value associated with the specified key.</returns>
        /// <exception cref="NotSupportedException">Thrown when the type is not supported.</exception>
        public Type Type(string key)
        {
            string valueType = GetValue(key).Split("::")[0];
            if (valueType == typeof(int).ToString())
            {
                return typeof(int);
            }
            if (valueType == typeof(string).ToString())
            {
                return typeof(string);
            }
            if (valueType == typeof(double).ToString())
            {
                return typeof(double);
            }
            if (valueType == typeof(bool).ToString())
            {
                return typeof(bool);
            }
            throw new NotSupportedException($"Not supported type: {valueType}!");
        }

        /// <summary>
        /// Reads the value associated with the specified key and converts it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the value should be converted.</typeparam>
        /// <param name="key">The key whose value is to be read.</param>
        /// <returns>The value associated with the specified key, converted to the specified type.</returns>
        /// <exception cref="NotSupportedException">Thrown when the type is not supported.</exception>
        public T Read<T>(string key)
        {
            string[] content = GetValue(key).Split("::");
            string preparedContent = _hybridEncryptor.DecryptString(_secured, key, content[1]);
            string genericType = typeof(T).ToString();
            if (content[0] == genericType && typeof(T) == typeof(int))
            {
                return (T)(object)int.Parse(preparedContent);
            }
            if (content[0] == genericType && typeof(T) == typeof(string))
            {
                return (T)(object)preparedContent;
            }
            if (content[0] == genericType && typeof(T) == typeof(double))
            {
                return (T)(object)Convert.ToDouble(preparedContent);
            }
            if (content[0] == genericType && typeof(T) == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(preparedContent);
            }
            throw new NotSupportedException($"Cannot read unsupported type: {content[0]}!");
        }

        /// <summary>
        /// Writes a string value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to which the value should be associated.</param>
        /// <param name="value">The string value to be written.</param>
        public void Write(string key, string value)
        {
            string preparedContent = _hybridEncryptor.EncryptString(_secured, key, value);
            string content = typeof(string).ToString() + "::" + preparedContent;
            SetValue(key, content);
            //File.WriteAllText(Path.Combine(_directoryPath, key + $".{FileExtension}"), content);
        }

        /// <summary>
        /// Writes an integer value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to which the value should be associated.</param>
        /// <param name="value">The integer value to be written.</param>
        public void Write(string key, int value)
        {
            string preparedContent = _hybridEncryptor.EncryptString(_secured, key, value.ToString());
            string content = typeof(int).ToString() + "::" + preparedContent;
            SetValue(key, content);
            //File.WriteAllText(Path.Combine(_directoryPath, key + $".{FileExtension}"), content);
        }

        /// <summary>
        /// Writes a boolean value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to which the value should be associated.</param>
        /// <param name="value">The boolean value to be written.</param>
        public void Write(string key, bool value)
        {
            string preparedContent = _hybridEncryptor.EncryptString(_secured, key, value.ToString());
            string content = typeof(bool).ToString() + "::" + preparedContent;
            SetValue(key, content);
            //File.WriteAllText(Path.Combine(_directoryPath, key + $".{FileExtension}"), content);
        }

        /// <summary>
        /// Writes a double value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to which the value should be associated.</param>
        /// <param name="value">The double value to be written.</param>
        public void Write(string key, double value)
        {
            string preparedContent = _hybridEncryptor.EncryptString(_secured, key, value.ToString());
            string content = typeof(double).ToString() + "::" + preparedContent;
            SetValue(key, content);
            //File.WriteAllText(Path.Combine(_directoryPath, key + $".{FileExtension}"), content);
        }

        /// <summary>
        /// Deletes the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value is to be deleted.</param>
        public void Delete(string key)
        {
            string? oldValue = _data[key] ?? null;
            File.Delete(Path.Combine(_directoryPath, key + $".{FileExtension}"));
            _data.Remove(key);
            OnDataChanged(key, oldValue, null);
        }

        /// <summary>
        /// Checks if a value associated with the specified key exists.
        /// </summary>
        /// <param name="key">The key to check for existence.</param>
        /// <returns>True if the value exists, otherwise false.</returns>
        public bool Exists(string key)
        {
            if (_data.ContainsKey(key) && File.Exists(Path.Combine(_directoryPath, key + $".{FileExtension}")))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Lists all key-value pairs in the storage.
        /// </summary>
        /// <returns>A dictionary containing all key-value pairs in the storage.</returns>
        public Dictionary<string, string> ListAll()
        {
            return _data;
        }
    }

}
