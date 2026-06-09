using System.Net;
using UnityEngine;

public class Cube : MonoBehaviour
{
    int n = 0;

    float spinSpeed = 50.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Hello World!!");
    }

    // Update is called once per frame
    void Update()
    {
        //n++; // adds 1 to the variable n
        //Debug.Log(n); // writes the value of n in the console
         
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }
}
