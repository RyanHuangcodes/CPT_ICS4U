using System.IO;
using UnityEngine;
//gpt
public static class SaveManager
{
    // Quick‐save used by PauseManager / UnpauseManager:
    private static readonly string QuickSavePath =
        Path.Combine(Application.persistentDataPath, "player_temp.json");

    // Permanent save for “Continue Game”:
    private static readonly string PermanentSavePath =
        Path.Combine(Application.persistentDataPath, "player_save.json");

    // ----- Quick‐save API -----

    public static void SaveQuick(SaveData data)
    {
        WriteJson(QuickSavePath, data);
    }

    public static SaveData LoadQuick()
    {
        return ReadJson(QuickSavePath);
    }

    public static void DeleteQuick()
    {
        DeleteFile(QuickSavePath);
    }

    // ----- Permanent‐save API -----

    public static void SavePermanent(SaveData data)
    {
        WriteJson(PermanentSavePath, data);
    }

    public static SaveData LoadPermanent()
    {
        return ReadJson(PermanentSavePath);
    }

    public static void DeletePermanent()
    {
        DeleteFile(PermanentSavePath);
    }

    // ----- internal helpers -----

    private static void WriteJson(string fullPath, SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json);
        Debug.Log($"[SaveManager] Wrote JSON to {fullPath}");
    }

    private static SaveData ReadJson(string fullPath)
    {
        if (!File.Exists(fullPath))
            return null;
        string json = File.ReadAllText(fullPath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    private static void DeleteFile(string fullPath)
    {
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log($"[SaveManager] Deleted save at {fullPath}");
        }
    }
}
