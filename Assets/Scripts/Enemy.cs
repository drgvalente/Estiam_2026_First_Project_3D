using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Visual Feedback")]
    public float flashDuration = 0.15f; // how much time the enemy will flashes red

    private Rigidbody rb;
    private Renderer enemyRendererer; // Component that controls the enemy color
    private Color originalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

        // finds the renderer so we can change the enemy's color
        enemyRendererer = GetComponent<Renderer>();
        if (enemyRendererer != null)
        {
            originalColor = enemyRendererer.material.color;
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
