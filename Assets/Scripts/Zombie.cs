using UnityEngine;
using UnityEngine.AI; // allow the use of the NavMesh
using System.Collections; // allow the use of arrays

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class Zombie : MonoBehaviour
{
    Transform player; // point to the player transform, making the enemy able to find it's position

    private NavMeshAgent agent; // point to the enemy navmesh agent, just like a "voodoo doll". Whatever we do o this variable will happen with the NavmehAgent component inside Unity
    private Animator anim;
    private Rigidbody rb;

    float attackCooldown = 3f;
    float lastAttackTime;

    float maxHealth = 50f;
    float currentHealth;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>(); // find the Nav Mesh Agent Component in the enemy
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true; // so the RigidBody dont "fight" the NavMesh Agent
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return; // if there is NO player, does nothing, stop the Update from running

        // 1. Calculate the distance to Player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if is in attack range
        if (distanceToPlayer <= agent.stoppingDistance)
        {
            // is close enough: stop walk and try attack
            agent.isStopped = true; // stop the enemy

            // look for the Player
            FaceTarget(player.position);

            // stop walk animation
            anim.SetBool("isWalking", false);

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
            anim.SetBool("isWalking", true);
            agent.SetDestination(player.position);

            // Ensures we are not attacking while walking (safety)
            anim.SetBool("isAttacking_1", false);
            anim.SetBool("isAttacking_2", false);
        }
    }

    void Attack()
    {
        // Sorteia um número: 0 ou 1
        int randomAttack = Random.Range(0, 2);

        if (randomAttack == 0)
        {
            // Ataque 1 (Mutant Punch)
            anim.SetBool("isAttacking_1", true);
            anim.SetBool("isAttacking_2", false);
            StartCoroutine(ResetAttackBool("isAttacking_1", 2.633f)); // Tempo estimado da animação
        }
        else
        {
            // Ataque 2 (Mutant Swiping)
            anim.SetBool("isAttacking_2", true);
            anim.SetBool("isAttacking_1", false);
            StartCoroutine(ResetAttackBool("isAttacking_2", 4.633f)); // Tempo estimado da animação
        }
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

    // Coroutine para desligar a bool de ataque depois que a animação acabar
    private IEnumerator ResetAttackBool(string boolName, float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool(boolName, false);
    }

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



}
