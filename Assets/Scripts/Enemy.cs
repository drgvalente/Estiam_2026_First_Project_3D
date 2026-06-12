using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    float attackCooldown = 2f;
    float lastAttackTime;

    [Header("Visual Feedback")]
    public float flashDuration = 0.15f; // how much time the enemy will flashes red

    private Rigidbody rb;
    private Renderer enemyRendererer; // Component that controls the enemy color
    private Color originalColor;

    private Transform player;

    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        player = FindAnyObjectByType<BetterPlayerController>().transform;
        agent = GetComponent<NavMeshAgent>(); // find the Nav Mesh Agent Component in the Boss
        
        rb.isKinematic = true;

        // finds the renderer so we can change the enemy's color
        enemyRendererer = GetComponent<Renderer>();
        if (enemyRendererer != null)
        {
            originalColor = enemyRendererer.material.color;
        }
    }

    void Update()
    {
        if (player == null) return;

        // 1. Calculate the distance to Player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 2. Check if is in attack range
        if (distanceToPlayer <= agent.stoppingDistance)
        {
            // is close enough: stop walk and try attack
            agent.isStopped = true;

            // look for the Player
            FaceTarget(player.position);


            // try to attack (consider cooldown)
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time; // get the current system clock time in seconds
            }
        }
        else
        {

            // is not in attack range: pursue the Player
            agent.isStopped = false;
            agent.SetDestination(player.position);

        }

    }

    void Attack()
    {

    }

    void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Ignora o eixo Y para não inclinar o Boss
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // public method to be called by the Bullet
    public void TakeDamage(float dmg, Vector3 hitDirection, float impactForce)
    {
        // 1. Reduces the HP
        currentHealth -= dmg;
        //Debug.Log("Enemy took damage and have " + currentHealth + " remaining HP.");

        // 2. Apply the force of the impact
        // let's use the hitDirection (Bullet direction) multiplied by the force
        if (rb != null)
        {
            rb.AddForce(hitDirection * impactForce, ForceMode.Impulse);
        }

        // 3. Starts the visual feedback:
        if (enemyRendererer != null)
        {
            StartCoroutine(FlashRed());
        }

        // 4. Tests if it Died:
        if (currentHealth <= 0f)
        {
            Die();
        }

    }

    void Die()
    {
        // Seek the spawner and tell this enemy died
        LevelManager spawner = FindFirstObjectByType<LevelManager>();
        if (spawner != null)
        {
            spawner.EnemyDied();
        }
        Destroy(gameObject);
    }

    // create the Coroutine to make the enemy flashes red:
    private IEnumerator FlashRed()
    {
        // changes color to red:
        enemyRendererer.material.color = Color.red;

        // wait the flashDuration time
        yield return new WaitForSeconds(flashDuration);

        // changes the color to original color:
        enemyRendererer.material.color = originalColor;
    }


}
