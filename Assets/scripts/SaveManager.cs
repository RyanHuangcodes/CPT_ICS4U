//gpt
using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string path = Path.Combine(Application.persistentDataPath, "player.json");

    public static void SavePlayerPosition(Vector2 pos)
    {
        SaveData data = new SaveData(pos.x, pos.y);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public static Vector2? LoadPlayerPosition()
    {
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return new Vector2(data.PlayerX, data.PlayerY);
    }
}
