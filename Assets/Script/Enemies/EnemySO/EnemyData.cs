using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{

    public int health;
    public float speed;
    public GameObject enemyPrefab; 
    public float spawnInterval = 1f;
    public List<Wave> waves;

  
    public int Health => health;
    public float Speed => speed;


}

