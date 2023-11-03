using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Singleton;

public class JsonManager : MonoSingleton<JsonManager>
{
    public void ConvertCSharpToJson<T>(T csharpCode, string fileName)
    {
        string directoryPath = $"{Application.persistentDataPath}/Json";
        if (Directory.Exists(directoryPath) == false)
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = $"{directoryPath}/{fileName}.json";
        string jsonData = JsonConvert.SerializeObject(csharpCode);
        File.WriteAllText(filePath, jsonData);
    }

    public T ConvertJsonToCSharp<T>(string fileName)
    {
        string directoryPath = $"{Application.persistentDataPath}/Json";
        if (Directory.Exists(directoryPath) == false)
        {
            throw new DirectoryNotFoundException();
        }

        string filePath = $"{directoryPath}/{fileName}.json";
        string jsonData = File.ReadAllText(filePath);
        T csharpCode = JsonConvert.DeserializeObject<T>(jsonData);

        return csharpCode;
    }
}

