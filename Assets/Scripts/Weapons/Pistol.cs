using UnityEngine;
using System.Collections;

public class Pistol : Weapon
{
    [SerializeField] float throwSpeed = 10f;
    [SerializeField] float returnSpeed = 12f;
    [SerializeField] float maxThrowDistance = 6f;

    bool isAttackWindowOpen;
    Transform homeParent;
    Vector3 homeLocalPosition;
    Quaternion homeLocalRotation;

    public override void Awake()
    {
        weaponName = "Pistol";
        damage = 10;
        attackSpeed = 0.3f;
        attackCooldown = 0f;
        attackCollider = GetComponent<CircleCollider2D>();
        weaponCollider.enabled = false;
        weaponVisual.enabled = false;

        isAttackWindowOpen = false;
        homeParent = transform.parent;
        homeLocalPosition = transform.localPosition;
        homeLocalRotation = transform.localRotation;
    }

    public override void Update()
    {
        if (attackCooldown <= 0f)
            return;

        attackCooldown -= Time.deltaTime;
        if (attackCooldown < 0f)
            attackCooldown = 0f;
    }

    public override void Attack()
    {
        if (attackCooldown > 0f || isAttackWindowOpen)
            return;

        attackCooldown = attackSpeed;
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(ThrowAndReturn());
    }

    IEnumerator ThrowAndReturn()
    {
        isAttackWindowOpen = true;
        weaponCollider.enabled = true;
        weaponVisual.enabled = true;

        Vector3 throwDirection = homeParent != null ? homeParent.up : transform.up;
        transform.SetParent(null, true);

        float travelled = 0f;
        while (travelled < maxThrowDistance)
        {
            float step = throwSpeed * Time.deltaTime;
            transform.position += throwDirection * step;
            travelled += step;
            transform.up = throwDirection;
            yield return null;
        }

        while (true)
        {
            Vector3 targetPosition = homeParent != null
                ? homeParent.TransformPoint(homeLocalPosition)
                : homeLocalPosition;

            Vector3 toHome = targetPosition - transform.position;
            float distanceThisFrame = returnSpeed * Time.deltaTime;

            if (toHome.sqrMagnitude <= distanceThisFrame * distanceThisFrame)
            {
                transform.position = targetPosition;
                break;
            }

            Vector3 move = toHome.normalized * distanceThisFrame;
            transform.position += move;
            transform.up = move.normalized;
            yield return null;
        }

        if (homeParent != null)
            transform.SetParent(homeParent);

        transform.localPosition = homeLocalPosition;
        transform.localRotation = homeLocalRotation;

        weaponCollider.enabled = false;
        weaponVisual.enabled = false;
        isAttackWindowOpen = false;
        attackRoutine = null;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D: " + other.gameObject.name);
        if (other.gameObject.layer == enemyLayer)
        {
            Debug.Log("Hit enemy " + other.gameObject.name + " with " + damage + " damage");
            other.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}