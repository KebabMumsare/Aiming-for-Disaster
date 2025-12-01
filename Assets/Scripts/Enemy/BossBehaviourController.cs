using UnityEngine;
using System.Collections;

public class BossBehaviourController : EnemyBehaviorController
{
    public Health health;

    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public float attackRange = 8f;
    public float keepDistance = 6f;
    public float fireRate = 2f;
    public float projectileSpeed = 10f;
    public Transform firePoint;
    
    [Header("Stage 2 - Melee Settings")]
    public float meleeDamage = 20f; // Damage dealt by melee attack
    public GameObject meleeAttackColliderObject; // GameObject containing the melee attack collider
    public float meleeAttackReach = 1.5f; // Radius of the circle around boss
    public float meleeRotationSpeed = 360f; // Degrees per second to rotate around boss
    public float meleeDamageCooldown = 0.5f; // Cooldown between damage hits (prevents rapid damage spam)
    
    [Header("Stage 3 - Frenzy Settings")]
    public float frenzyFireRate = 0.5f;
    public int projectilesPerBurst = 3;
    public float burstSpreadAngle = 30f;
    public float frenzyMovementSpeed = 6f;
    
    public enum BossStage
    {
        Stage1,
        Stage2,
        Stage3
    }

    [Header("Stage Settings")]
    public BossStage currentStage = BossStage.Stage1;

    private float nextFireTime;
    private Collider2D meleeCollider; // Cached reference to the collider component
    private SpriteRenderer meleeRenderer; // Cached reference to the renderer component (if visible)
    private float meleeOrbitAngle = 0f; // Current angle of melee object orbiting around boss
    private float lastMeleeDamageTime = 0f; // Last time damage was dealt (for cooldown)

    protected override void Start()
    {
        base.Start();
        // Cache the melee collider and renderer components
        if (meleeAttackColliderObject != null)
        {
            meleeCollider = meleeAttackColliderObject.GetComponent<Collider2D>();
            meleeRenderer = meleeAttackColliderObject.GetComponent<SpriteRenderer>();
            
            // Start disabled - will be enabled in Stage 2
            if (meleeCollider != null)
            {
                meleeCollider.enabled = false;
            }
            if (meleeRenderer != null)
            {
                meleeRenderer.enabled = false;
            }
            if (meleeRenderer == null)
            {
                meleeAttackColliderObject.SetActive(false);
            }
            
            // Add a script to the melee collider to handle damage
            MeleeColliderDamageHandler damageHandler = meleeAttackColliderObject.GetComponent<MeleeColliderDamageHandler>();
            if (damageHandler == null)
            {
                damageHandler = meleeAttackColliderObject.AddComponent<MeleeColliderDamageHandler>();
            }
            damageHandler.Initialize(this);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (playerTransform == null) return;

        if (health.currentHealth <= health.maxHealth * 0.5f) {
            currentStage = BossStage.Stage2;
        }

        switch (currentStage)
        {
            case BossStage.Stage1:
                DisableMeleeCollider(); // Ensure melee collider is off in Stage 1
                StageOneAttack();
                break;
            case BossStage.Stage2:
                StageTwoAttack();
                break;
            case BossStage.Stage3:
                DisableMeleeCollider(); // Ensure melee collider is off in Stage 3
                StageThreeAttack();
                break;
        }
    }

    private void StageOneAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Keep distance from player while attacking
        if (distanceToPlayer < keepDistance)
        {
            Vector3 directionAway = (transform.position - playerTransform.position).normalized;
            if (ai != null)
            {
                ai.destination = transform.position + directionAway * 2f;
            }
        }
        else if (distanceToPlayer > attackRange)
        {
            // Move closer if too far
            if (ai != null)
            {
                ai.destination = playerTransform.position;
            }
        }

        // Attack with single projectile
        if (distanceToPlayer <= attackRange && Time.time >= nextFireTime)
        {
            if (projectilePrefab != null)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                Shoot(direction);
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    private void StageTwoAttack()
    {
        // Continuously rotate melee object around boss in a circle
        // Boss movement is handled by its own movement script
        RotateMeleeObjectAroundBoss();
    }

    private void RotateMeleeObjectAroundBoss()
    {
        if (meleeAttackColliderObject == null) return;

        // Continuously increment the orbit angle
        meleeOrbitAngle += meleeRotationSpeed * Time.deltaTime;
        if (meleeOrbitAngle >= 360f)
        {
            meleeOrbitAngle -= 360f;
        }

        // Calculate position in circle around boss
        float radians = meleeOrbitAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * meleeAttackReach;
        meleeAttackColliderObject.transform.position = transform.position + offset;
        
        // Rotate melee object to point outward (tangent to the circle)
        float tangentAngle = meleeOrbitAngle + 90f; // Perpendicular to radius
        meleeAttackColliderObject.transform.rotation = Quaternion.Euler(0f, 0f, tangentAngle - 90f);
        
        // Keep melee collider enabled during Stage 2
        EnableMeleeCollider();
    }

    public void OnMeleeHitPlayer(Collider2D playerCollider)
    {
        // Called by MeleeColliderDamageHandler when melee collider hits player
        if (Time.time >= lastMeleeDamageTime + meleeDamageCooldown)
        {
            Health playerHealth = playerCollider.GetComponent<Health>();
            if (playerHealth != null)
            {
                Debug.Log("Melee attack hit player");
                playerHealth.TakeDamage(meleeDamage);
                lastMeleeDamageTime = Time.time;
            }
        }
    }

    private void EnableMeleeCollider()
    {
        if (meleeAttackColliderObject == null) return;

        if (meleeCollider != null)
        {
            meleeCollider.enabled = true;
        }
        if (meleeRenderer != null)
        {
            meleeRenderer.enabled = true;
        }
        // If no renderer, enable the GameObject
        if (meleeRenderer == null)
        {
            meleeAttackColliderObject.SetActive(true);
        }
    }

    private void DisableMeleeCollider()
    {
        if (meleeAttackColliderObject == null) return;

        if (meleeCollider != null)
        {
            meleeCollider.enabled = false;
        }
        if (meleeRenderer != null)
        {
            meleeRenderer.enabled = false;
        }
        // If no renderer, disable the GameObject
        if (meleeRenderer == null)
        {
            meleeAttackColliderObject.SetActive(false);
        }
    }

    private void StageThreeAttack()
    {
        // Implement Stage 3 logic here
        // Example: Frenzy mode or special ability
    }

    void Shoot(Vector3 direction)
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        spawnPos.z = -7; // Ensure z is set correctly for 2D
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

// Helper script attached to melee collider GameObject to handle damage
public class MeleeColliderDamageHandler : MonoBehaviour
{
    private BossBehaviourController bossController;
    
    public void Initialize(BossBehaviourController boss)
    {
        bossController = boss;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && bossController != null)
        {
            bossController.OnMeleeHitPlayer(other);
        }
    }
}
