using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static readonly string QuickSavePath =
        Path.Combine(Application.persistentDataPath, "player_temp.json");
    private static readonly string PermanentSavePath =
        Path.Combine(Application.persistentDataPath, "player_save.json");

    // quick‐save for pause/resume
    public static void SaveQuick(SaveData data)       => WriteJson(QuickSavePath, data);
    public static SaveData LoadQuick()                => ReadJson(QuickSavePath);
    public static void DeleteQuick()
    {
        DeleteFile(QuickSavePath);
        ResetPlacementTrackers();
    }

    // permanent‐save for Continue
    public static void SavePermanent(SaveData data)   => WriteJson(PermanentSavePath, data);
    public static SaveData LoadPermanent()            => ReadJson(PermanentSavePath);
    public static void DeletePermanent()
    {
        DeleteFile(PermanentSavePath);
        ResetPlacementTrackers();
    }

    // alias if you prefer that name
    public static void DeletePermanentSave()          => DeletePermanent();

    private static void WriteJson(string fullPath, SaveData data)
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json);
        Debug.Log($"[SaveManager] Wrote JSON to {fullPath}");
    }

    private static SaveData ReadJson(string fullPath)
    {
        if (!File.Exists(fullPath))
            return null;
        var json = File.ReadAllText(fullPath);
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

    private static void ResetPlacementTrackers()
    {
        BasePlacementTracker.Instance?.SetPlacedCount(0);
        GoldMinePlacementTracker.Instance?.SetPlacedCount(0);
        MachineGunPlacementTracker.Instance?.SetPlacedCount(0);
        DualMachineGunPlacementTracker.Instance?.SetPlacedCount(0);
        CannonPlacementTracker.Instance?.SetPlacedCount(0);
        PiercingCannonPlacementTracker.Instance?.SetPlacedCount(0);
        MissileLauncherPlacementTracker.Instance?.SetPlacedCount(0);
    }
}
