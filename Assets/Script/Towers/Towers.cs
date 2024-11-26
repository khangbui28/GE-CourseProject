using UnityEngine;

public class Towers : MonoBehaviour
{
    [SerializeField] float attackRadius = 5f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] GameObject projectilePrefab;

    private float attackTimer;
    private Transform targetEnemy;
    [SerializeField] Transform shootingPoint;

    private void Update()
    {
        attackTimer += Time.deltaTime;
        FindTargetEnemy();

        if (targetEnemy != null)
        {
            LookAtEnemy();
            if (attackTimer >= attackCooldown)
            {
                ShootAtEnemy();
                attackTimer = 0f;
            }
        }
    }

    private void FindTargetEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius);
        float closestDistance = float.MaxValue;
        targetEnemy = null;

        foreach (Collider hit in hits)
        {
            NewEnemy enemy = hit.GetComponent<NewEnemy>();
            OldEnemy enemy2 = hit.GetComponent<OldEnemy>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = enemy.transform;
                }
            }
            else if (enemy2 != null)
            {
                float distance = Vector3.Distance(transform.position, enemy2.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = enemy2.transform;
                }
            }
        }
    }

    private void LookAtEnemy()
    {
        Vector3 direction = targetEnemy.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        }
    }

    private void ShootAtEnemy()
    {
        if (targetEnemy == null) return;

        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position + Vector3.up, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(targetEnemy);
    }

}
