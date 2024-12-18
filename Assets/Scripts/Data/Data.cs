using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;

public static class Data 
{
    private static string _path = Application.persistentDataPath;

    public static void Save<T>(T data,string fileName) where T : ScriptableObject
    {
        string filePuth = Path.Combine(_path,fileName + ".json");

        string json = JsonUtility.ToJson(data,true);

        if(!Directory.Exists(filePuth))
        {
            Directory.CreateDirectory(filePuth);
        }
        File.WriteAllText(filePuth, json);
    }

    public static T Load<T>(string fileName) where T : ScriptableObject
    {
        string filePuth = Path.Combine(_path, fileName + ".json");

        if(File.Exists(filePuth))
        {
            string json = File.ReadAllText(filePuth);

            T data = ScriptableObject.CreateInstance<T>();

            JsonUtility.FromJsonOverwrite(json, data);

            return data;
        }
        else
        {
            return null;
        }
    }
}
