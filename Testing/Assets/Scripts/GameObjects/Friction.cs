using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour
{
    public float amount = 0.5f; // amount the object is slowed down by
    
    void OnTriggerEnter2D(Collider2D col){
        // pull object into itself if it has a rigid body
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        if (rb != null && !col.isTrigger){
           rb.velocity *= amount;
        }
    }
}
