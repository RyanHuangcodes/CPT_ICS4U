using System;                        
using System.Collections.Generic;   
using UnityEngine;                   

[Serializable]
public class SaveData
{
    public Vector2 PlayerPosition;
    public int Gold;
    public List<TowerSaveData> Towers;
    public List<EnemySaveData> Enemies;

    public int BasePlaced;
    public int GoldMinePlaced;
    public int CannonPlaced;      

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
        List<TowerSaveData> towers,
        List<EnemySaveData> enemies,
        int baseCount,
        int mineCount,
        int cannonCount,         
        int currentWave,
        int spawnedInWave,
        float timeUntilNextSpawn,
        float healthMul,
        float damageMul,
        int postBossCycle,
        int knifeTier
    ) {
        PlayerPosition       = playerPos;
        Gold                 = gold;
        Towers               = towers;
        Enemies              = enemies;
        BasePlaced           = baseCount;
        GoldMinePlaced       = mineCount;
        CannonPlaced         = cannonCount; 
        CurrentWave          = currentWave;
        SpawnedInCurrentWave = spawnedInWave;
        TimeUntilNextSpawn   = timeUntilNextSpawn;
        HealthMultiplier     = healthMul;
        DamageMultiplier     = damageMul;
        PostBossCycle        = postBossCycle;
        KnifeTier            = knifeTier;
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
        Type      = type;
        Position  = pos;
        Health    = health;
        MaxHealth = maxHealth;
        Level     = level;
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
        Type     = type;
        Position = pos;
        Health   = health;
    }
}
