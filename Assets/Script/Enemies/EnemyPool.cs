using UnityEngine;
using System.Collections.Generic;



public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance; // Singleton for global access

    [System.Serializable]
    public class EnemyPoolItem
    {
        public EnemyData enemyData; // Reference to the Flyweight ScriptableObject
        public int initialPoolSize; // Initial size of the pool
    }

    public List<EnemyPoolItem> enemyPoolItems;
    private Dictionary<EnemyData, Queue<GameObject>> poolDictionary = new Dictionary<EnemyData, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializePool();
    }

    private void InitializePool()
    {
        foreach (var item in enemyPoolItems)
        {
            if (!poolDictionary.ContainsKey(item.enemyData))
            {
                poolDictionary[item.enemyData] = new Queue<GameObject>();

                for (int i = 0; i < item.initialPoolSize; i++)
                {
                    GameObject enemy = CreateNewEnemy(item.enemyData);
                    enemy.SetActive(false);
                    poolDictionary[item.enemyData].Enqueue(enemy);
                }
            }
        }
    }

    private GameObject CreateNewEnemy(EnemyData enemyData)
    {
        GameObject enemy = Instantiate(enemyData.enemyPrefab);
        enemy.GetComponent<NewEnemy>().data = enemyData; // Assign Flyweight data
        return enemy;
    }

    public GameObject GetEnemy(EnemyData enemyData, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(enemyData))
        {
            Debug.LogWarning($"No pool found for enemy type: {enemyData.name}");
            return null;
        }

        if (poolDictionary[enemyData].Count > 0)
        {
            GameObject enemy = poolDictionary[enemyData].Dequeue();
            enemy.transform.position = position;
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            // Expand pool if no inactive enemies are available
            GameObject enemy = CreateNewEnemy(enemyData);
            enemy.transform.position = position;
            return enemy;
        }
    }

    public void ReturnEnemy(EnemyData enemyData, GameObject enemy)
    {
        enemy.SetActive(false);

        if (!poolDictionary.ContainsKey(enemyData))
        {
            Debug.LogWarning($"No pool found for enemy type: {enemyData.name}");
            Destroy(enemy);
        }
        else
        {
            poolDictionary[enemyData].Enqueue(enemy);
        }
    }
}
