using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace EAssets
{
    //Contains data storage and data loading API 
    namespace IO
    {
        /// <summary>
        /// This class allows IO functionality, like logging and storing data
        /// <para>Write Data: It is recommended to store single values or text with WriteData and classes or structs with
        /// WriteDataBinary. ReadData returns the string read from a file while ReadDataBinary will return deserialized data</para>
        /// </summary>
        public static class DataIO
        {
            #region Storing in Files
            /// <summary>
            /// The base directory
            /// </summary>
            private static readonly string BaseDirectory = Application.persistentDataPath;
            /// <summary>
            /// The base file extension
            /// </summary>
            private static readonly string BaseFileExtension = ".dat";

            #region Write Data
            /// <summary>
            /// Write a string to a file
            /// </summary>
            /// <param name="data">The string you want to write</param>
            /// <param name="filename">The filename you want to use</param>
            /// <param name="relativePath">The relative path you want to store your file at</param>
            /// <example>
            /// <code>
            /// //Stores a given string at the given directory (starting at the default directory)
            /// DataIO.WriteString(stringValue,filename,relativePath);
            /// </code>
            /// </example>
            public static void WriteString(string data, string filename, string relativePath)
            {
                string absolutePath = System.IO.Path.Combine(BaseDirectory, relativePath);

                if (!System.IO.Directory.Exists(absolutePath))
                {
                    System.IO.Directory.CreateDirectory(absolutePath);
                }

                string filePath = System.IO.Path.Combine(absolutePath, filename + ".json");

                using (FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate))
                {
                    StreamWriter sw = new StreamWriter(fileStream);
                    sw.WriteLine(data);

                    sw.Flush();

                    sw.Close();
                }
            }


            /// <summary>
            /// Stores data in a binary format
            /// </summary>
            /// <param name="data">The data you want to store</param>
            /// <param name="filename">The filename you want to use</param>
            /// <param name="relativePath">The filepath relative to the PersistentDataPath directory</param>
            /// <example>
            /// <code>
            /// object data = someData;
            /// //Stores given data at the given directory (starting at the default directory) using a binary format
            /// DataIO.WriteDataBinary(data,filename,relativePath);
            /// </code>
            /// </example>
            public static void WriteDataBinary(object data, string filename, string relativePath, FileMode filemode = FileMode.OpenOrCreate)
            {
                if (data == null)
                    return;

                string absolutePath = System.IO.Path.Combine(BaseDirectory, relativePath);

                if (!System.IO.Directory.Exists(absolutePath))
                {
                    System.IO.Directory.CreateDirectory(absolutePath);
                }

                string filePath = System.IO.Path.Combine(absolutePath, filename + BaseFileExtension);

                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter
                    = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                using (FileStream fileStream = File.Open(filePath, filemode))
                {
                    binaryFormatter.Serialize(fileStream, data);
                }

            }

            /// <summary>
            /// Stores data in the PersistentDataPath directory
            /// </summary>
            /// <param name="data">The data you want to store</param>
            /// <param name="filename">The name of the created file</param>
            /// <param name="relativePath">The relative path (relative to the directory of PersistentDataPath) of the stored data</param>
            /// <param name="fileExtension">The used file extension (standard is .dat)</param>
            /// <example>
            /// <code>
            /// object data = someData;
            /// //Stores given data at the given directory (starting at the default directory)
            /// //No formatter is used in this method
            /// DataIO.WriteData(data,filename,relativePath);
            /// </code>
            /// </example>
            public static void WriteData(object data, string filename, string relativePath, string fileExtension = ".dat", FileMode filemode = FileMode.OpenOrCreate)
            {
                string absolutePath = System.IO.Path.Combine(BaseDirectory, relativePath);

                if (!System.IO.Directory.Exists(absolutePath))
                {
                    System.IO.Directory.CreateDirectory(absolutePath);
                }

                string filePath = System.IO.Path.Combine(absolutePath, filename + fileExtension);

                using (FileStream fileStream = File.Open(filePath, filemode))
                {
                    StreamWriter sw = new StreamWriter(fileStream);
                    sw.WriteLine(data);

                    sw.Flush();

                    sw.Close();
                }

            }

            /// <summary>
            /// Writes the given data into a XML-File
            /// </summary>
            /// <param name="data">The data you want to write</param>
            /// <param name="filename">The name of the file to create</param>
            /// <param name="relativePath">The path you want to use</param>
            /// <example>
            /// <code>
            /// object data = someData;
            /// //Stores given data at the given directory (starting at the default directory) using a XML formatter
            /// DataIO.WritaDataXML(data,filename,relativePath);
            /// </code>
            /// </example>
            public static void WritaDataXML(object data, string filename, string relativePath)
            {
                string absolutePath = System.IO.Path.Combine(BaseDirectory, relativePath);

                if (!System.IO.Directory.Exists(absolutePath))
                {
                    System.IO.Directory.CreateDirectory(absolutePath);
                }

                string filePath = System.IO.Path.Combine(absolutePath, filename + ".xml");

                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(data.GetType());

                TextWriter writer = new StreamWriter(filePath);
                ser.Serialize(writer, data);
                writer.Close();
            }

            /// <summary>
            /// Write data to a json formated file
            /// </summary>
            /// <param name="data">The data you want to write</param>
            /// <param name="filename">The name of the file you want to write to</param>
            /// <param name="relativePath">The relative path to store the file</param>
            /// <example>
            /// <code>
            /// object data = someData;
            /// //Stores given data at the given directory (starting at the default directory) using a JSON formatter
            /// DataIO.WriteDataJSON(data,filename,relativePath);
            /// </code>
            /// </example>
            public static void WriteDataJSON(object data, string filename, string relativePath, bool append=false)
            {
                string absolutePath = System.IO.Path.Combine(BaseDirectory, relativePath);

                if (!System.IO.Directory.Exists(absolutePath))
                {
                    System.IO.Directory.CreateDirectory(absolutePath);
                }

                string filePath = System.IO.Path.Combine(absolutePath, filename + ".json");

                //Serialize the data
                System.Runtime.Serialization.Json.DataContractJsonSerializer js =
                    new System.Runtime.Serialization.Json.DataContractJsonSerializer(data.GetType());

                //The memory stream is used for serializing
                MemoryStream stream = new MemoryStream();
                //The streamwriter is used to write to a file
                StreamWriter writer = new StreamWriter(filePath,append);

                
                js.WriteObject(stream, data);
                byte[] json = stream.ToArray();
                stream.Close();

                writer.Write(System.Text.Encoding.UTF8.GetString(json, 0, json.Length));
                writer.Flush();
                writer.Close();
            }
            #endregion

            #region Read Data
            /// <summary>
            /// Reads a binary and returns it as a given type. Returns true if successfull, else false
            /// </summary>
            /// <typeparam name="T">The type you want to get back (this value is implicit)</typeparam>
            /// <param name="data">The data you get returned</param>
            /// <param name="filename">The name of the file</param>
            /// <param name="relativePath">The relative path to the file</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Reads binary data from a given file and returns true if successfull, else false
            /// if(DataIO.ReadDataBinary{typeRead}(out typeRead data,filename,relativePath))
            /// {
            ///     //Do something with the read data here...
            /// }
            /// </code>
            /// </example>
            public static bool ReadDataBinary<T>(out T data, string filename, string relativePath)
            {
                string absolutePath = System.IO.Path.Combine(BaseDirectory, relativePath);

                if (!System.IO.Directory.Exists(absolutePath))
                {
                    data = default;
                    return false;
                }

                string filePath = System.IO.Path.Combine(absolutePath, filename + BaseFileExtension);

                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter
                    = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                using (FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate))
                {
                    if (fileStream != null && fileStream.Length != 0)
                    {
                        data = (T)binaryFormatter.Deserialize(fileStream);
                        return true;
                    }
                    data = default;
                    return false;
                }
            }

            /// <summary>
            /// Reads a file. Returns true if it exists, else false. If the directory exists but the file does not, the file is created
            /// </summary>
            /// <param name="data">The data as a string you read</param>
            /// <param name="filename">The name of the file</param>
            /// <param name="relativePath">The relative path of the file</param>
            /// <param name="fileExtension">The extension of the file</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Reads plain string data from a given file and returns true if successfull, else false
            /// if(DataIO.ReadData(out string data,filename,relativePath))
            /// {
            ///     //Do something with the read data here...
            /// }
            /// </code>
            /// </example>
            public static bool ReadData(out string data, string filename, string relativePath, string fileExtension = ".dat")
            {
                string absolutePath = System.IO.Path.Combine(BaseDirectory, relativePath);

                if (!System.IO.Directory.Exists(absolutePath))
                {
                    data = String.Empty;
                    return false;
                }

                string filePath = System.IO.Path.Combine(absolutePath, filename + fileExtension);

                using (FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate))
                {
                    StreamReader sw = new StreamReader(fileStream);
                    data = sw.ReadToEnd();

                    sw.Close();

                    return true;
                }
            }

            /// <summary>
            /// Reads data from a XML-file. Returns true if successful, else false
            /// </summary>
            /// <typeparam name="T">The type of the data in the XML-File</typeparam>
            /// <param name="data">The data returned</param>
            /// <param name="filename">The name of the file</param>
            /// <param name="relativePath">The relative path to the file</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Reads xml data from a given file and returns true if successfull, else false
            /// if(DataIO.ReadDataXML{typeRead}(out typeRead data,filename,relativePath))
            /// {
            ///     //Do something with the read data here...
            /// }
            /// </code>
            /// </example>
            public static bool ReadDataXML<T>(out T data, string filename, string relativePath)
            {
                string absolutePath = System.IO.Path.Combine(BaseDirectory, relativePath);

                if (!System.IO.Directory.Exists(absolutePath))
                {
                    data = default;
                    return false;
                }

                string filePath = System.IO.Path.Combine(absolutePath, filename + ".xml");

                System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(T));

                using (Stream reader = new FileStream(filePath, FileMode.Open))
                {
                    // Call the Deserialize method to restore the object's state.
                    data = (T)serializer.Deserialize(reader);
                    return true;
                }

            }

            /// <summary>
            /// Read data from a JSON formated file
            /// </summary>
            /// <typeparam name="T">The type of the data in the file</typeparam>
            /// <param name="data">The data read from the file</param>
            /// <param name="filename">The filename you want to read</param>
            /// <param name="relativePath">The relative path of the file you want to read</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Reads json data from a given file and returns true if successfull, else false
            /// if(DataIO.ReadDataJSON{typeRead}(out typeRead data,filename,relativePath))
            /// {
            ///     //Do something with the read data here...
            /// }
            /// </code>
            /// </example>
            public static bool ReadDataJSON<T>(out T data, string filename, string relativePath)
            {
                string absolutePath = System.IO.Path.Combine(BaseDirectory, relativePath);

                if (!System.IO.Directory.Exists(absolutePath))
                {
                    data = default;
                    return false;
                }

                string filePath = System.IO.Path.Combine(absolutePath, filename + ".json");

                StreamReader json = new StreamReader(filePath);
                MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json.ReadToEnd()));
                json.Close();

                System.Runtime.Serialization.Json.DataContractJsonSerializer ser =
                    new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

                data = (T)ser.ReadObject(ms);
                ms.Close();
                return true;
            }
            #endregion

            #region Log Data
            /// <summary>
            /// Log data in a log file
            /// </summary>
            /// <param name="data">The data you want to log</param>
            public static void Log(object data)
            {
                WriteData(data, "Logfile", "CustomDebug", ".txt", FileMode.Append);
            }

            /// <summary>
            /// Writes log data into a warning-log-file
            /// </summary>
            /// <param name="data"></param>
            public static void LogWarning(object data)
            {
                WriteData(data, "WarningLogfile", "CustomDebug", ".txt", FileMode.Append);
            }

            /// <summary>
            /// Logs an error and throws an exception
            /// </summary>
            /// <param name="data">The data you want to log</param>
            /// <param name="ex">The exception you want to throw</param>
            public static void LogError(object data, Exception ex)
            {
                WriteData(data, "ErrorLogfile", "CustomDebug", ".txt", FileMode.Append);
                throw ex;
            }

            /// <summary>
            /// Logs the data into the logfile and into unity
            /// </summary>
            /// <param name="data">The data you want to log</param>
            public static void GlobalLog(object data)
            {
                Log(data);
                Debug.Log(data);
            }

            /// <summary>
            /// Logs the data into the logfile and into unity
            /// </summary>
            /// <param name="data">The data you want to log</param>
            public static void GlobalLogWarning(object data)
            {
                LogWarning(data);
                Debug.LogWarning(data);
            }

            /// <summary>
            /// Logs the data into the logfile and into unity
            /// </summary>
            /// <param name="data">The data you want to log</param>
            /// <param name="ex">The exception you want to throw</param>
            public static void GlobalLogError(object data, Exception ex)
            {
                LogError(data, ex);
                Debug.LogError(data);
            }
            #endregion

            #region Convenience Methods
            /// <summary>
            /// The default folder name to store data in
            /// </summary>
            private static readonly string Datafolder = "CustomData";

            /// <summary>
            /// Store arbitrary data permanently (session persistent)
            /// </summary>
            /// <param name="data">The data you want to store</param>
            /// <param name="filename">The name of the file you want to store the data in</param>
            /// <param name="folder">The folder you want to store it in (leave this field empty for the standard folder)</param>
            public static void SaveData(object data, string filename, string folder = "")
            {
                if (folder.Equals(""))
                    DataIO.WriteDataBinary(data, filename, Datafolder);
                else
                    DataIO.WriteDataBinary(data, filename, folder);
            }

            /// <summary>
            /// Returns the saved data if it exists, else default
            /// </summary>
            /// <typeparam name="T">The type of the data you want to load</typeparam>
            /// <param name="filename">The filename of the data</param>
            /// <param name="folder">The folder you want to store it in (leave this field empty for the standard folder)</param>
            /// <returns></returns>
            public static T LoadData<T>(string filename, string folder = "")
            {
                if (folder.Equals(""))
                {
                    if (DataIO.ReadDataBinary<T>(out T data, filename, Datafolder))
                    {
                        return data;
                    }

                    GlobalLogWarning("File or folder not found: Filename: " + filename + "; Folder: " + Datafolder);
                    return default;
                }
                else
                {
                    if (DataIO.ReadDataBinary<T>(out T data, filename, folder))
                    {
                        return data;
                    }

                    GlobalLogWarning("File or folder not found: Filename: " + filename + "; Folder: " + folder);
                    return default;
                }
            }
            #endregion

            #endregion
        }

        namespace Cached
        {
            /// <summary>
            /// Instead of saving your data your save requests will be cached. This improves performance. 
            /// It also allows to load cached data instead of reading from file
            /// <para>Dont forget to save this data before closing the game</para>
            /// <para>Supported file extension are: .dat, .json and .xml</para>
            /// </summary>
            public static class CachedDataIO
            {
                private static Dictionary<string, CacheEntry> storeCache = new Dictionary<string, CacheEntry>();

                /// <summary>
                /// Save data in the cache. You need to flush the cache to store the data on file (you can still load it before storing on file)
                /// </summary>
                /// <param name="data">The data you want to store</param>
                /// <param name="filename">The name you want to give to the file</param>
                /// <param name="relativePath">The relative path you want to store your data in</param>
                /// <param name="extension">The data extension you want to use (.dat, .json and .xml supported)</param>
                public static void SaveData(object data, string filename, string relativePath, string extension = ".dat")
                {
                    if (!(extension.ToLower().Equals(".dat")
                        || extension.ToLower().Equals(".json")
                        || extension.ToLower().Equals(".xml")))
                    {
                        //Unsupported extension found (still allow storing
                        DataIO.GlobalLogWarning("Warning: Unsupported extension at CachedDataIO. Extension requested: " + extension + "; Supported: .dat, .json and .xml");
                    }

                    string key = relativePath + filename + extension;
                    storeCache[key] = new CacheEntry(data, filename, relativePath, extension);
                }

                /// <summary>
                /// Load data from either the cache or the filesystem in this order.
                /// </summary>
                /// <typeparam name="T">The type you want to load</typeparam>
                /// <param name="filename">The name of the file you want to load</param>
                /// <param name="relativePath">The relative path of the file you want to load</param>
                /// <param name="extension">The data extension of the file</param>
                /// <returns></returns>
                public static T LoadData<T>(string filename, string relativePath, string extension = ".dat")
                {
                    string key = relativePath + filename + extension;
                    if (storeCache.TryGetValue(key, out CacheEntry ce)) //The entry exists in cache if true
                    {
                        if (ce.Data.GetType().Equals(typeof(T))) //If the type matches
                        {
                            return (T)ce.Data; //Return the data
                        }
                        else //Type mismatch, return default
                        {
                            DataIO.GlobalLogWarning("Typemismatch at CachedDataIO LoadData. Expected type: " + typeof(T) + "; found type: " + ce.Data.GetType());
                            return default;
                        }
                    }
                    else    //The entry does not exist: Look on file-system
                    {
                        T data;
                        switch (extension.ToLower())
                        {
                            case ".json":
                                if (DataIO.ReadDataJSON<T>(out data, filename, relativePath))
                                {
                                    return data;
                                }
                                else
                                {
                                    DataIO.GlobalLogWarning("Typemismatch at CachedDataIO LoadData. Expected type: " + typeof(T) + "; found type on (file-system): " + ce.Data.GetType());
                                    return default;
                                }
                            case ".xml":
                                if (DataIO.ReadDataXML<T>(out data, filename, relativePath))
                                {
                                    return data;
                                }
                                else
                                {
                                    DataIO.GlobalLogWarning("Typemismatch at CachedDataIO LoadData. Expected type: " + typeof(T) + "; found type (on file-system): " + ce.Data.GetType());
                                    return default;
                                }
                            default:    //.dat files
                                if (DataIO.ReadDataBinary<T>(out data, filename, relativePath))
                                {
                                    return data;
                                }
                                else
                                {
                                    DataIO.GlobalLogWarning("Typemismatch at CachedDataIO LoadData. Expected type: " + typeof(T) + "; found type (on file-system): " + ce.Data.GetType());
                                    return default;
                                }
                        }
                    }
                }

                /// <summary>
                /// Flushes the cache and stores all data on file
                /// </summary>
                public static void FlushCache()
                {
                    foreach (KeyValuePair<string, CacheEntry> pair in storeCache)
                    {
                        CacheEntry ce = pair.Value;
                        switch (ce.FileExtension.ToLower())
                        {
                            case ".json":
                                DataIO.WriteDataJSON(ce.Data, ce.Filename, ce.RelativePath);
                                break;
                            case ".xml":
                                DataIO.WritaDataXML(ce.Data, ce.Filename, ce.RelativePath);
                                break;
                            default:
                                DataIO.WriteDataBinary(ce.Data, ce.Filename, ce.RelativePath);
                                break;
                        }
                    }

                    storeCache.Clear(); //Empty the cache after writing its data
                }

                /// <summary>
                /// Removes an entry from the cache, preventing it from being stored when the cache is flushed
                /// </summary>
                /// <param name="filename">The filename of the entry</param>
                /// <param name="relativePath">The relative path the entry would have been stored in</param>
                /// <param name="extension">The file-extension of the file</param>
                public static void DontSaveEntry(string filename, string relativePath, string extension = ".dat")
                {
                    string key = relativePath + filename + extension;
                    if (storeCache.ContainsKey(key))
                    {
                        storeCache.Remove(key);
                    }
                }

                /// <summary>
                /// This class defines a single entry into the storeCache
                /// </summary>
                private class CacheEntry
                {
                    /// <summary>
                    /// The data of this cacheEntry
                    /// </summary>
                    public object Data { get; private set; }
                    /// <summary>
                    /// The name of the file the data will get stored in
                    /// </summary>
                    public string Filename { get; private set; }
                    /// <summary>
                    /// The relative path (beginning at Application.PersistenDataPath) to the file
                    /// </summary>
                    public string RelativePath { get; private set; }
                    /// <summary>
                    /// The extension of this entry (.dat, .json or .xml)
                    /// </summary>
                    public string FileExtension { get; private set; }

                    /// <summary>
                    /// Create a new CacheEntry
                    /// </summary>
                    /// <param name="data">The data you want to store</param>
                    /// <param name="filename">The filename you want to use</param>
                    /// <param name="relativePath">The folder directory you want to store your file in</param>
                    /// <param name="extension">The extension you want to use</param>
                    public CacheEntry(object data, string filename, string relativePath, string extension)
                    {
                        this.Data = data;
                        this.Filename = filename;
                        this.RelativePath = relativePath;
                        this.FileExtension = extension;
                    }
                }
            }
        }
    }
}
