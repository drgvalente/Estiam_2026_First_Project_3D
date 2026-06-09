using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public float turnSpeed = 100.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0f, -turnSpeed * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0f, 0f, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0f, 0f, -speed * Time.deltaTime);
        }

    }
}
