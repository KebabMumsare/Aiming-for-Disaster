using UnityEngine;
using System.Collections;

public class Pistol : Weapon
{
    [SerializeField] public float throwSpeed = 10f;
    [SerializeField] public float returnSpeed = 12f;
    [SerializeField] public float maxThrowDistance = 6f;
    [SerializeField] public float spinSpeed = 720f;

    bool isAttackWindowOpen;
    Transform homeParent;
    Vector3 homeLocalPosition;
    Vector3 homeWorldPosition;
    Quaternion homeLocalRotation;
    Quaternion visualDefaultRotation;
    float visualSpinAngle;
    bool discardOnReturn;
    Transform playerTransform;

    public override void Awake()
    {
        weaponName = "Pistol";
        damage = GetComponent<Damage>();
        attackSpeed = attackSpeed;
        attackCooldown = 0f;
        attackCollider = GetComponent<CircleCollider2D>();
        weaponCollider.enabled = false;
        weaponVisual.enabled = false;

        isAttackWindowOpen = false;
        discardOnReturn = false;
        homeParent = transform.parent;
        homeLocalPosition = transform.localPosition;
        homeLocalRotation = transform.localRotation;
        playerTransform = FindPlayerTransform();
        UpdateHomeWorldPosition();
        if (weaponVisual != null)
        {
            visualDefaultRotation = weaponVisual.transform.localRotation;
        }
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
        visualSpinAngle = 0f;

        Vector3 throwDirection = homeParent != null ? homeParent.up : transform.up;
        UpdateHomeWorldPosition();
        transform.SetParent(null, true);

        float travelled = 0f;
        while (travelled < maxThrowDistance)
        {
            float step = throwSpeed * Time.deltaTime;
            transform.position += throwDirection * step;
            travelled += step;
            transform.up = throwDirection;
            ApplySpin();
            yield return null;
        }

        while (true)
        {
            Vector3 targetPosition = GetCurrentReturnPosition();

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
            ApplySpin();
            yield return null;
        }

        bool reattached = TryReattachToHome();

        if (!reattached)
        {
            CleanupAndDestroy();
            yield break;
        }

        weaponCollider.enabled = false;
        weaponVisual.enabled = false;
        ResetVisualRotation();
        isAttackWindowOpen = false;
        attackRoutine = null;
    }

    bool TryReattachToHome()
    {
        if (discardOnReturn || !isEquipped || homeParent == null)
        {
            return false;
        }

        for (int i = 0; i < homeParent.childCount; i++)
        {
            Transform child = homeParent.GetChild(i);
            if (child != transform)
            {
                return false;
            }
        }

        transform.SetParent(homeParent);
        transform.localPosition = homeLocalPosition;
        transform.localRotation = homeLocalRotation;
        return true;
    }

    public override void OnEquipped(Transform pivot)
    {
        base.OnEquipped(pivot);
        discardOnReturn = false;
        homeParent = pivot;
        homeLocalPosition = transform.localPosition;
        homeLocalRotation = transform.localRotation;
        playerTransform = FindPlayerTransform();
        UpdateHomeWorldPosition();
    }

    public override void OnUnequipped()
    {
        base.OnUnequipped();
        discardOnReturn = true;
        UpdateHomeWorldPosition();
        homeParent = null;
    }

    void CleanupAndDestroy()
    {
        weaponCollider.enabled = false;
        weaponVisual.enabled = false;
        ResetVisualRotation();
        isAttackWindowOpen = false;
        attackRoutine = null;
        Destroy(gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D: " + other.gameObject.name); // Unnecessary
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Health>().TakeDamage(damage.GetDamage());
        }
        if (other.gameObject.tag == "LootBox")
        {
            other.gameObject.GetComponent<Health>().TakeDamage(damage.GetDamage());
        }
    }

    void ApplySpin()
    {
        if (weaponVisual == null)
        {
            return;
        }

        visualSpinAngle += spinSpeed * Time.deltaTime;
        weaponVisual.transform.localRotation = visualDefaultRotation * Quaternion.Euler(0f, 0f, visualSpinAngle);
    }

    void ResetVisualRotation()
    {
        if (weaponVisual == null)
        {
            return;
        }

        visualSpinAngle = 0f;
        weaponVisual.transform.localRotation = visualDefaultRotation;
    }

    void UpdateHomeWorldPosition()
    {
        if (homeParent != null)
        {
            homeWorldPosition = homeParent.TransformPoint(homeLocalPosition);
        }
        else if (playerTransform != null)
        {
            homeWorldPosition = playerTransform.position;
        }
        else
        {
            homeWorldPosition = transform.position;
        }
    }

    Vector3 GetCurrentReturnPosition()
    {
        if (homeParent != null)
        {
            return homeParent.TransformPoint(homeLocalPosition);
        }

        if (playerTransform == null)
        {
            playerTransform = FindPlayerTransform();
        }

        return playerTransform != null ? playerTransform.position : homeWorldPosition;
    }

    Transform FindPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform : null;
    }
}