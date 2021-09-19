using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float downwardsForce = 500f;
    public float maxDownwardForce = 100f;
    private Rigidbody2D rb;
    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        CalculateGravity();
        CalculateRotation();
    }

    void CalculateGravity()
    {
        if (Input.GetKeyDown("space")){
            rb.AddForce(new Vector2(0,-downwardsForce));
        }
    }

    void CalculateRotation()
    {
        
    }
}