using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Vector2 PlayerPosition;
    public int Gold;
    public List<TowerSaveData> Towers;
    public int BasePlaced;
    public int GoldMinePlaced;

    public SaveData(Vector2 pos, int gold, List<TowerSaveData> towers, int baseCount, int mineCount)
    {
        PlayerPosition = pos;
        Gold = gold;
        Towers = towers;
        BasePlaced = baseCount;
        GoldMinePlaced = mineCount;
    }
}

[Serializable]
public class TowerSaveData
{
    public string Type;
    public Vector2 Position;
    public int Health;
    public int Level;

    public TowerSaveData(string type, Vector2 pos, int health, int level)
    {
        Type = type;
        Position = pos;
        Health = health;
        Level = level;
    }
}
