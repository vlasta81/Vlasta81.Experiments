### Documentation for ConfigurationStorage Library

#### Description
The `ConfigurationStorage` library is used for securely storing and retrieving configuration data as files. It supports various data types such as `int`, `string`, `double`, and `bool`. Data is encrypted using hybrid encryption and stored in files.

#### Usage
1. **Initialization**:
   ```csharp
   var configStorage = new ConfigurationStorage("path/to/directory", hybridEncryptorInstance);
   ```

2. **Writing data**:
   ```csharp
   configStorage.Write("key1", 123);
   configStorage.Write("key2", "some string");
   configStorage.Write("key3", 45.67);
   configStorage.Write("key4", true);
   ```

3. **Reading data**:
   ```csharp
   int intValue = configStorage.Read<int>("key1");
   string stringValue = configStorage.Read<string>("key2");
   double doubleValue = configStorage.Read<double>("key3");
   bool boolValue = configStorage.Read<bool>("key4");
   ```

4. **Reading as SecureString**:
   ```csharp
   SecureString secureStringValue = configStorage.ReadSecureString("key2");
   ```

5. **Deleting data**:
   ```csharp
   configStorage.Delete("key1");
   ```

6. **Checking if key exists**:
   ```csharp
   bool exists = configStorage.Exists("key1");
   ```

7. **Listing all keys and values**:
   ```csharp
   Dictionary<string, string> allData = configStorage.ListAll();
   ```

#### DataChanged Event
The library supports a `DataChanged` event that is triggered when data changes. You can subscribe to this event as follows:

```csharp
configStorage.DataChanged += new DataChangedEventHandler(OnDataChanged);

private void OnDataChanged(object sender, DataChangedEventArgs e)
{
    Console.WriteLine($"Key: {e.Key}, Old Value: {e.OldValue}, New Value: {e.NewValue}");
}
```