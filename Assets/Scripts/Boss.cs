using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class Boss : MonoBehaviour
{
    
    Transform player;

    private NavMeshAgent agent;
    private Animator anim;
    private Rigidbody rb;

    float attackCooldown = 2f;
    float lastAttackTime;

    float moveSpeed = 5f;
    float turnSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<BetterPlayerController>().transform;
        agent = GetComponent<NavMeshAgent>(); // find the Nav Mesh Agent Component in the Boss
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true; // so the RigidBody dont "fight" the NavMesh Agent
    }

    // Update is called once per frame
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

            // stop animation
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
        }
        
    }

    void Attack()
    {

    }

    void FaceTarget(Vector3 targetPosition)
    {

    }

    void FixedUpdate()
    {
        // the BAD way to make Boss follow player:
        // because Boss will ignore walls and obstacles
        //Vector3 playerPos = new Vector3(player.position.x, 0f, player.position.z);
        //transform.LookAt(playerPos);
        //transform.Translate(0f, 0f, moveSpeed * Time.deltaTime);
    }
}
