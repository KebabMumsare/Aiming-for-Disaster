using UnityEngine;
using Pathfinding;

public class RangedEnemyBehavior : EnemyBehaviorController
{
    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public float attackRange = 8f;
    public float keepDistance = 6f;
    public float fireRate = 2f;
    public float projectileSpeed = 10f;
    public LayerMask obstacleLayer;
    public Transform firePoint; // Optional: specific point to fire from

    private float nextFireTime;

    protected override void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        bool hasLineOfSight = HasLineOfSight();

        // Movement Logic
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            if (patrol != null) patrol.enabled = false;
            destinationSetter.enabled = false; // We will manually control destination

            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Vector3 targetPos;

            if (distanceToPlayer < keepDistance)
            {
                // Too close, move away
                targetPos = transform.position - directionToPlayer * (keepDistance - distanceToPlayer + 2f); // Move a bit extra away
            }
            else if (distanceToPlayer > attackRange)
            {
                // Too far, move closer
                targetPos = playerTransform.position;
            }
            else
            {
                // In sweet spot, maybe stop or strafe? For now, just stop.
                targetPos = transform.position;
            }

            if (ai != null)
            {
                ai.destination = targetPos;
            }

            // Attack Logic
            if (distanceToPlayer <= attackRange && hasLineOfSight)
            {
                // Rotate towards player to shoot
                // Simple 2D rotation
                Vector3 diff = playerTransform.position - transform.position;
                
                if (Time.time >= nextFireTime)
                {
                    Shoot(diff);
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }
        else
        {
            // Back to patrol
            if (isChasing)
            {
                isChasing = false;
                if (patrol != null) patrol.enabled = true;
                // destinationSetter.enabled = false; // Patrol script usually handles its own destination
            }
        }
    }

    private bool HasLineOfSight()
    {
        if (playerTransform == null) return false;

        Vector3 origin = firePoint != null ? firePoint.position : transform.position;
        Vector3 direction = (playerTransform.position - origin).normalized;
        float distance = Vector3.Distance(origin, playerTransform.position);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, obstacleLayer);

        // If we hit something in the obstacle layer, we don't have line of sight
        return hit.collider == null;
    }


    void Shoot(Vector3 direction)
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        
        // Rotate projectile to face direction
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90); // Assuming projectile sprite points UP

        EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
        if (ep != null)
        {
            ep.speed = projectileSpeed;
        }
    }
}
