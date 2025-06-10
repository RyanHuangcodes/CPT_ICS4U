using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string path =
        Path.Combine(Application.persistentDataPath, "player.json");

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        Debug.Log($"[SaveManager] JSON being written:\n{json}");
        File.WriteAllText(path, json);
        Debug.Log("Game saved to " + path);
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(path)) return null;
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}
