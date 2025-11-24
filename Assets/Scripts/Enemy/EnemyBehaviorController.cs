using UnityEngine;
using Pathfinding;

public class EnemyBehaviorController : MonoBehaviour
{
    public GameObject playerTarget;
    public float detectionRange = 10f;
    public float xpReward = 10f;
    protected AIDestinationSetter destinationSetter;
    protected Patrol patrol;
    protected IAstarAI ai;
    protected bool isChasing = false;
    protected Transform playerTransform;

    protected virtual void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        patrol = GetComponent<Patrol>();
        ai = GetComponent<IAstarAI>();

        // Check if required components exist
        if (destinationSetter == null)
        {
            Debug.LogError("AIDestinationSetter component not found on " + gameObject.name);
            return;
        }

        if (patrol == null)
        {
            Debug.LogWarning("Patrol component not found on " + gameObject.name);
        }

        // Find player if not assigned
        if (playerTarget == null)
        {
            Debug.Log("Player target not set, using player tag");
            playerTarget = GameObject.FindGameObjectWithTag("Player");
        }

        if (playerTarget == null)
        {
            Debug.LogError("Player target not found");
            return;
        }

        playerTransform = playerTarget.transform;

        destinationSetter.target = playerTransform;
        
        // Start in patrol mode
        destinationSetter.enabled = false;
        if (patrol != null)
        {
            patrol.enabled = true;
        }
    }

    protected virtual void Update()
    {
        // Check if playerTransform is valid before using it
        if (playerTransform == null)
        {
            return;
        }

        // Check distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= detectionRange && !isChasing)
        {
            // Switch to chase mode
            isChasing = true;
            if (patrol != null)
            {
                patrol.enabled = false;
            }
            destinationSetter.enabled = true;
        }
        else if (distanceToPlayer > detectionRange && isChasing)
        {
            // Switch back to patrol mode
            isChasing = false;
            destinationSetter.enabled = false;
            if (patrol != null)
            {
                patrol.enabled = true;
            }
        }
    }
}
