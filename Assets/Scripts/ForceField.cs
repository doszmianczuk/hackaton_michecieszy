using System;
using UnityEngine;

public class ForceField : MonoBehaviour
{

    public Vector2 force;
    public float minSpeed;
    
    bool isOn = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ensure the object has a Rigidbody2D.
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null && Vector2.Dot(rb.linearVelocity, force.normalized) >= minSpeed)
        {
            isOn = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null && isOn)
        {
            rb.AddForce(force, ForceMode2D.Force);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null && isOn)
        {
            isOn = false;
        }
    }
}
