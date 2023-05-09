using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrone : MonoBehaviour
{
    public float health = 1f;
    public float projectileSpeed = 10f;
    public float projectileDelay = 1f;
    public float selfDestructRange = 2f;
    public float forceFieldRange = 1f;
    public float forceFieldDuration = 2f;
    public float forceFeildCooldown = 2f;
    public float selfDestructDelay = 1f;
    public GameObject projectilePrefab;
    public GameObject explosionPrefab;
    public GameObject forceFieldPrefab;
    private bool isShooting = false;

    private Transform player;
    private bool isActive = true;
    private bool isForceFieldActive = false;
    private float forceFieldTimer = 0f;
    private float forceFieldCoolDownTimer = 0f;
    private float selfDestructTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isActive)
        {
            // Check if the player is within range for self-destruct
            if (Vector2.Distance(transform.position, player.position) < selfDestructRange)
            {
                Debug.Log(selfDestructTimer);
                selfDestructTimer += Time.deltaTime;
                if (selfDestructTimer >= selfDestructDelay)
                {
                    SelfDestruct();
                }
            }
            else
            {
                selfDestructTimer = 0f;
            }

            // Check if the player is within range for force field & if duration is up & cooldown was done
            if (Vector2.Distance(transform.position, player.position) < forceFieldRange && forceFieldTimer < forceFieldDuration)
            {
                if (!isForceFieldActive && forceFieldCoolDownTimer < 0)
                {
                    ActivateForceField();
                    forceFieldTimer = 0;
                }
                else
                {
                    forceFieldCoolDownTimer-=Time.deltaTime;
                }

                forceFieldTimer += Time.deltaTime;
                forceFieldCoolDownTimer -= Time.deltaTime;
            }
            else
            {
                DeactivateForceField();
                forceFieldCoolDownTimer = forceFeildCooldown;
            }

            // Shoot projectiles at the player
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootProjectile());
                
            }
        }
    }

    IEnumerator ShootProjectile()
    {
        yield return new WaitForSeconds(projectileDelay);
        if (isActive && !isForceFieldActive)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = (player.position - transform.position).normalized * projectileSpeed;
        }
        isShooting = false;
    }

    void ActivateForceField()
    {
        isForceFieldActive = true;
        forceFieldTimer = forceFieldDuration;
        GameObject forceField = Instantiate(forceFieldPrefab, transform.position, Quaternion.identity);
        forceField.transform.parent = transform;
    }

    void DeactivateForceField()
    {
        isForceFieldActive = false;
        forceFieldTimer = 0f;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("ForceField"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    void SelfDestruct()
    {
        isActive = false;
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && !isForceFieldActive && other.CompareTag("PlayerProjectile"))
        {
            health -= 1f;
            if (health <= 0f)
            {
                SelfDestruct();
            }
        }
    }
}

