using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 20f;
    float lifeTime = 1.5f; // time in seconds each bullet lasts/lingers/exists
    float damage = 30f; // the damage this bullet does
    float impactForce = 5f; // change to push enemy more or less further 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy this bullet after some time
    }

    void FixedUpdate()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            //Debug.Log("Take that!!!!!!");
            //Destroy(col.transform.gameObject); // destroy the gameobject this bullet hit
            col.transform.GetComponent<Enemy>().TakeDamage(damage, transform.forward, impactForce);
        }
        Destroy(gameObject);
    }

}
