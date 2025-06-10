// using System.Collections.Generic;
// using System;
// using UnityEngine;
// using static UnityEngine.RuleTile.TilingRuleOutput;
// using UnityEngine.EventSystems;

// public class Zombie: Enemy
// {
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }
//     public override void Update()
//     {
//         Tower[] towerArray = FindObjectsByType<Tower>(FindObjectsSortMode.None);
//         List<Tower> towerList = new List<Tower>(towerArray);

//         if (towerList.Count == 0)
//         {
//             _moveDirection = Vector2.zero;
//             return;
//         }

//         Tower closestTower = FindClosestTarget(towerList);
//         if (closestTower == null)
//         {
//             _moveDirection = Vector2.zero;
//             return;
//         }

//         float distance = Vector2.Distance(transform.position, closestTower.transform.position);

//         if (distance <= AttackRange && Time.time >= _lastAttackTime + AttackCooldown)
//         {
//             Attack(closestTower.transform);
//             _lastAttackTime = Time.time;
//             _moveDirection = Vector2.zero;
//         }
//         else
//         {
//             _moveDirection = (closestTower.transform.position - transform.position).normalized;
//         }
//     }

//     public Zombie() : base()
//     {

//     }
//     public Tower FindClosestTarget(List<Tower> towers)
//     {
//         int _capacity = 1;
//         (float, Tower)[] _targets = new (float, Tower)[_capacity];
//         int _count = 0;

//         foreach (Tower tower in towers)
//         {
//             if (_count >= _targets.Length)
//             {
//                 _targets = AddArrayIndex(_targets);
//             }
//             float dist = CalculateDistance(transform.position, tower.transform.position);
//             _targets[_count] = (dist, tower);
//             _count++;
//         }

//         MergeSort(_targets, 0, _count - 1);
//         return _targets[0].Item2; // Return the closest tower

//         //_targets[0].Item2.takeDamage(GetDamage());

//         // Item2 means the Gameobject as targets are in form (float, GameObject)
//         // The list needs both float which is sorted for distance and the Gameobject to know which is which
//     }
//     private void MergeSort((float, Tower)[] targets, int start, int end)
//     {
//         if (end <= start)
//             return;

//         int _mid = start + (end - start) / 2;

//         MergeSort(targets, start, _mid);
//         MergeSort(targets, _mid + 1, end);

//         (float, Tower)[] _newArray = new (float, Tower)[end - start + 1];
//         int _cursor = 0;
//         int _left = start;
//         int _right = _mid + 1;

//         while (_left < _mid + 1 && _right <= end)
//         {
//             if (targets[_left].Item1 < targets[_right].Item1)
//             {
//                 _newArray[_cursor] = targets[_left];
//                 _left += 1;
//             }
//             else
//             {
//                 _newArray[_cursor] = targets[_right];
//                 _right += 1;
//             }
//             _cursor += 1;
//         }

//         while (_left < _mid + 1)
//         {
//             _newArray[_cursor] = targets[_left];
//             _left += 1;
//             _cursor += 1;
//         }

//         while (_right <= end)
//         {
//             _newArray[_cursor] = targets[_right];
//             _right += 1;
//             _cursor += 1;
//         }

//         for (int i = 0; i < _newArray.Length; i++)
//         {
//             targets[start + i] = _newArray[i];
//         }
//     }
//     private float CalculateDistance(Vector3 posEnemy, Vector3 posTower)
//     {
//         float _distanceX = posEnemy.x - posTower.x;
//         float _distanceY = posEnemy.y - posTower.y;
//         return Mathf.Sqrt(_distanceX * _distanceX + _distanceY * _distanceY);
//     }
//     //AddArrayIndex() is gpt code 
//     private (float, Tower)[] AddArrayIndex((float, Tower)[] original)
//     {
//         int _newSize = original.Length + 1;
//         var _newArray = new (float, Tower)[_newSize];
//         Array.Copy(original, _newArray, original.Length);
//         return _newArray;
//     }

//     // Update is called once per frame

// }
