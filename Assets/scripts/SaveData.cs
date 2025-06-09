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


    public int CurrentWave;
    public int SpawnedInCurrentWave;     
    public float TimeUntilNextSpawn;     
    public float HealthMultiplier;
    public float DamageMultiplier;
    public int PostBossCycle;

    public SaveData(
        Vector2 playerPos,
        int gold,
        List<TowerSaveData> towers,
        List<EnemySaveData> enemies,
        int baseCount,
        int mineCount,
        int currentWave,
        int spawnedInWave,
        float timeUntilNextSpawn,
        float healthMul,
        float damageMul,
        int postBossCycle
    ) {
        PlayerPosition = playerPos;
        Gold = gold;
        Towers = towers;
        Enemies = enemies;
        BasePlaced = baseCount;
        GoldMinePlaced = mineCount;

        CurrentWave = currentWave;
        SpawnedInCurrentWave = spawnedInWave;
        TimeUntilNextSpawn = timeUntilNextSpawn;
        HealthMultiplier = healthMul;
        DamageMultiplier = damageMul;
        PostBossCycle = postBossCycle;
    }
}

[Serializable]
public class TowerSaveData
{
    public string Type;
    public Vector2 Position;
    public int Health;
    public int Level;

    public TowerSaveData(string type, Vector2 pos, int health, int level) {
        Type = type;
        Position = pos;
        Health = health;
        Level = level;
    }
}

[Serializable]
public class EnemySaveData
{
    public string Type;
    public Vector2 Position;
    public int Health;

    public EnemySaveData(string type, Vector2 pos, int health) {
        Type = type;
        Position = pos;
        Health = health;
    }
}
