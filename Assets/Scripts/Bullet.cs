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
        if (col.transform.CompareTag("Zombie"))
        {
            col.transform.GetComponent<Zombie>().TakeDamage(damage, transform.forward, impactForce);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Verifica se acertou o Escudo primeiro
        if (other.CompareTag("Shield")) // Crie a Tag "Shield" e coloque na Sphere
        {
            BossShieldController shield = other.GetComponent<BossShieldController>();
            if (shield != null)
            {
                // Passa o PONTO EXATO mundial onde a bala encostou
                shield.RegisterHit(transform.position);
            }

            // Destroi a bala ao atingir o escudo
            Destroy(gameObject);
            return; // Sai da função para não executar o código abaixo
        }

        // 2. Se não for escudo, verifica se acertou o Inimigo (código antigo)
        if (other.CompareTag("Enemy"))
        {
            // ... seu código de dano no inimigo ...
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

}
