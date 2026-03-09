using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DBCommons : MonoBehaviour
{
    [Header(" Database Stuff")]
    public static string databaseName = "UserData.sqlite";
    public static string databaseDirectory = Application.streamingAssetsPath;
    public static string databasePath
    {
        get
        {
            return Path.Combine(databaseDirectory, databaseName);
        }
    }
}
