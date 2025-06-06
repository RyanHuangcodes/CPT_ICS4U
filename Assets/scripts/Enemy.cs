using JetBrains.Annotations;
using UnityEngine;
using System.Collections.Generic;

public class Enemy : Entity
{
    private Weapon _weapon;
    public Enemy() : base()
    {
        _weapon = new Weapon();
    }

    //public void AttackClosestTarget(List<GameObject> towers, List<GameObject> players)
    //{
    //    int capacity = 1;
    //    (float, GameObject)[] targets = new (float, GameObject)[capacity];
    //    int count = 0;

    //    foreach (GameObject tower in towers)
    //    {
    //        //if (count >= targets.Length)
    //            //targets = AddArrayIndex(targets);

    //            //float dist = CalculateDistance(transform.position, tower.transform.position);
    //            //targets[count++] = (dist, tower);
    //    }

    //}
    ////public float CalculateDistance(Vector3 a, Vector3 b)
    ////{

    ////}

}

