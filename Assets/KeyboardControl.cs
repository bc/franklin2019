using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
    private Rigidbody rb;
    public float multiplier = 0.1f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MapForce(rb, KeyCode.UpArrow, Vector3.forward * multiplier);
        MapForce(rb, KeyCode.DownArrow, Vector3.back * multiplier);
        MapForce(rb, KeyCode.LeftArrow, Vector3.left * multiplier);
        MapForce(rb, KeyCode.RightArrow, Vector3.right * multiplier);
    }

    private void MapForce(Rigidbody rb, KeyCode arrow, Vector3 vector)
    {
        if (Input.GetKey(arrow))
        {
            rb.AddRelativeForce(vector);      
        }
    }
}
