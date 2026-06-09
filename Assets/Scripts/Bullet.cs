using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 20f;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        
    }

}
