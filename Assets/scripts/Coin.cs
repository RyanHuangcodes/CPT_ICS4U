using UnityEngine;

public class Coin
{
    public int value;
    public int year;

    // Constructor
    public Coin(int value, int year)
    {
        this.value = value;
        this.year = year;
    }
    public float GetValue()
    {
        return value;
    }

    public int GetYear()
    {
        return year;
    }
}