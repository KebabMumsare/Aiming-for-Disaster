using UnityEngine;
using Pathfinding;

public class EnemyBehaviorController : MonoBehaviour
{
    public Transform playerTarget;
    public float detectionRange = 10f;
    
    private AIDestinationSetter destinationSetter;
    private Patrol patrol;
    private IAstarAI ai;
    private bool isChasing = false;

    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        patrol = GetComponent<Patrol>();
        ai = GetComponent<IAstarAI>();
        
        // Start in patrol mode
        destinationSetter.enabled = false;
        patrol.enabled = true;
    }

    void Update()
    {
        // Check distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        
        if (distanceToPlayer <= detectionRange && !isChasing)
        {
            // Switch to chase mode
            isChasing = true;
            patrol.enabled = false;
            destinationSetter.enabled = true;
        }
        else if (distanceToPlayer > detectionRange && isChasing)
        {
            // Switch back to patrol mode
            isChasing = false;
            destinationSetter.enabled = false;
            patrol.enabled = true;
        }
    }
}
