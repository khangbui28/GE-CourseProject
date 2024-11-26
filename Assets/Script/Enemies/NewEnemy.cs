using System;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    public EnemyData data;

  
    private int currentHealth;
    private int currentPathIndex = 0;
    private bool isAlive = true;

    public static event Action<NewEnemy> OnEnemyDestroyed;
    public static event Action<NewEnemy> OnEnemyReachedEnd;

    private void Start()
    {
        Init();
    }


    private void Init()
    {
        currentHealth = data.Health;
      

    }


    private void Update()
    {
        MoveAlongPath();
      

    }

    private void MoveAlongPath()
    {
        if (currentPathIndex < GridManager.Instance.pathCoordinates.Count)
        {
            Vector3 targetPosition = GridManager.Instance.GetTileWorldPosition(
                GridManager.Instance.pathCoordinates[currentPathIndex].x,
                GridManager.Instance.pathCoordinates[currentPathIndex].y
            );

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, data.Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }

        if (currentPathIndex == GridManager.Instance.pathCoordinates.Count)
        {
            ReachEnd();
            ReturnToPool();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isAlive = false;
            OnEnemyDestroyed?.Invoke(this);
            Destroy(gameObject);
            ReturnToPool();
        }
    }

    public void ReachEnd()
    {
        OnEnemyReachedEnd?.Invoke(this);
        Destroy(gameObject);
    }

    private void ReturnToPool()
    {
        EnemyPool.Instance.ReturnEnemy(data, gameObject);
    }
}

