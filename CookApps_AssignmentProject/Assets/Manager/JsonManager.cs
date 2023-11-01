using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using Newtonsoft.Json;

public class JsonManager : MonoSingleton<JsonManager>
{
    public void ConvertCSharpToJson<T>(T csharpCode, string fileName)
    {
        string jsonData = JsonConvert.SerializeObject(csharpCode);
        string path = Path.Combine(Application.dataPath, $"Json/{fileName}.json");
        File.WriteAllText(path, jsonData);
    }

    public T ConvertJsonToCSharp<T>(string fileName)
    {
        string path = Path.Combine(Application.dataPath, $"Json/{fileName}.json");
        string jsonData = File.ReadAllText(path);
        T csharpCode = JsonConvert.DeserializeObject<T>(jsonData);

        return csharpCode;
    }
}

