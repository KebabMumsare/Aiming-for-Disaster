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
    public Transform firePoint; // Optional: specific point to fire from

    private float nextFireTime;

    protected override void Start()
    {
        base.Start();
        // Ensure we don't use the default destination setter logic from base if we want custom movement
        // But base.Start() sets it up. We will override the behavior in Update.
    }

    protected override void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

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
            if (distanceToPlayer <= attackRange)
            {
                // Rotate towards player to shoot
                // Simple 2D rotation
                Vector3 diff = playerTransform.position - transform.position;
                diff.Normalize();
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                // Adjust -90 if sprite is facing up, or 0 if right. Assuming Up for top-down usually requires -90 adjustment if sprite is Up.
                // Let's assume standard rotation for now, user can adjust.
                // transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90); 
                
                // Actually, let's just rely on the projectile direction for now or let A* handle rotation if enabled.
                // But for shooting we need to aim.
                
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
