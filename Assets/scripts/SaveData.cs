using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Vector2 PlayerPosition;
    public int Gold;
    public int Score;
    public int PlayerHealth;
    public int PlayerMaxHealth;
    public List<TowerSaveData> Towers;
    public List<EnemySaveData> Enemies;

    public int BasePlaced;
    public int GoldMinePlaced;
    public int MachineGunPlaced;
    public int DualMachineGunPlaced;
    public int CannonPlaced;
    public int PiercingCannonPlaced;   
    public int MissileLauncherPlaced;

    public int CurrentWave;
    public int SpawnedInCurrentWave;
    public float TimeUntilNextSpawn;
    public float HealthMultiplier;
    public float DamageMultiplier;
    public int PostBossCycle;
    public int KnifeTier;

    public SaveData(
        Vector2 playerPos,
        int gold,
        int score,
        int playerHealth,
        int playerMaxHealth,
        List<TowerSaveData> towers,
        List<EnemySaveData> enemies,
        int baseCount,
        int mineCount,
        int machineGunCount,
        int dualMGCount,
        int cannonCount,
        int piercingCannonCount,      
        int missileCount,
        int currentWave,
        int spawnedInWave,
        float timeUntilNextSpawn,
        float healthMul,
        float damageMul,
        int postBossCycle,
        int knifeTier
    )
    {
        PlayerPosition = playerPos;
        Gold = gold;
        Score = score;
        PlayerHealth = playerHealth;
        PlayerMaxHealth = playerMaxHealth;
        Towers = towers;
        Enemies = enemies;
        BasePlaced = baseCount;
        GoldMinePlaced = mineCount;
        MachineGunPlaced = machineGunCount;
        DualMachineGunPlaced = dualMGCount;
        CannonPlaced = cannonCount;
        PiercingCannonPlaced = piercingCannonCount;
        MissileLauncherPlaced = missileCount;
        CurrentWave = currentWave;
        SpawnedInCurrentWave = spawnedInWave;
        TimeUntilNextSpawn = timeUntilNextSpawn;
        HealthMultiplier = healthMul;
        DamageMultiplier = damageMul;
        PostBossCycle = postBossCycle;
        KnifeTier = knifeTier;
    }
}


[Serializable]
public class TowerSaveData
{
    public string Type;
    public Vector2 Position;
    public int Health;
    public int MaxHealth;
    public int Level;

    public TowerSaveData(string type, Vector2 pos, int health, int maxHealth, int level)
    {
        Type = type;
        Position = pos;
        Health = health;
        MaxHealth = maxHealth;
        Level = level;
    }
}

[Serializable]
public class EnemySaveData
{
    public string Type;
    public Vector2 Position;
    public int Health;

    public EnemySaveData(string type, Vector2 pos, int health)
    {
        Type = type;
        Position = pos;
        Health = health;
    }
}
