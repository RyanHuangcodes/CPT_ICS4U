using JetBrains.Annotations;
using UnityEngine;

public class Enemy : Entity
{
    private Weapon _weapon;
    public Enemy() : base()
    {
        _weapon = new Weapon();
    }
    
    
}

