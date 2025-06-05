//gpt
using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string path = Path.Combine(Application.persistentDataPath, "player.json");

    public static void SavePlayerPosition(Vector2 pos)
    {
        PlayerData data = new PlayerData(pos.x, pos.y);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public static Vector2? LoadPlayerPosition()
    {
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        return new Vector2(data.x, data.y);
    }
}
