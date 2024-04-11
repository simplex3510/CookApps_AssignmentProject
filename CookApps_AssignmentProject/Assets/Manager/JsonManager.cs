using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Singleton;

public class JsonManager : MonoSingleton<JsonManager>
{
    private string jsonDirectoryPath;
    private string JsonDirectoryPath
    {
        get
        {
            if (jsonDirectoryPath == null)
            {
                jsonDirectoryPath = $"{Application.persistentDataPath}/Json";
            }
            return jsonDirectoryPath;
        }
    }

    public void ConvertCSharpToJson<T>(T csharpCode, string jsonFileName)
    {
        if (Directory.Exists(JsonDirectoryPath) == false)
        {
            Directory.CreateDirectory(JsonDirectoryPath);
        }

        string jsonFilePath = $"{JsonDirectoryPath}/{jsonFileName}.json";
        string jsonData = JsonConvert.SerializeObject(csharpCode);
        File.WriteAllText(jsonFilePath, jsonData);
    }

    public T ConvertJsonToCSharp<T>(string jsonFileName)
    {
        if (Directory.Exists(JsonDirectoryPath) == false)
        {
            throw new DirectoryNotFoundException();
        }

        string jsonFilePath = $"{JsonDirectoryPath}/{jsonFileName}.json";
        string jsonData = File.ReadAllText(jsonFilePath);
        T csharpCode = JsonConvert.DeserializeObject<T>(jsonData);

        return csharpCode;
    }
}

