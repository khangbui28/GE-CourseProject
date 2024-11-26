using System;
using System.Collections.Generic;
using UnityEngine;

public class OldEnemy : MonoBehaviour
{

    public int health = 5;
    public float speed = 10f;

    private int currentPathIndex = 0;
    private bool isAlive = true;

    private bool behaviourAdjusted = false;

    public static event Action<OldEnemy> OnEnemyDestroyed;
    public static event Action<OldEnemy> OnEnemyReachedEnd2;


    private void Update()
    {
        MoveAlongPath();

        if (!behaviourAdjusted)
        {
            
            speed = EnemyBehaviour.AdjustEnemySpeed(UIManager.Instance.playerHealth, speed);
            Debug.Log($"Adjusted Speed: {speed}");
            behaviourAdjusted = true; 
        }
       
    }

    private void SpeedChange()
    {
        speed = EnemyBehaviour.AdjustEnemySpeed(UIManager.Instance.playerHealth, speed);
    }

    private void MoveAlongPath()
    {
        if (currentPathIndex < GridManager.Instance.pathCoordinates.Count)
        {
            Vector3 targetPosition = GridManager.Instance.GetTileWorldPosition(
                GridManager.Instance.pathCoordinates[currentPathIndex].x,
                GridManager.Instance.pathCoordinates[currentPathIndex].y
            );

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

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
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

       health -= damage;

        if (health <= 0)
        {
            isAlive = false;
            OnEnemyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void ReachEnd()
    {
        OnEnemyReachedEnd2?.Invoke(this);
        Destroy(gameObject);
    }
}

