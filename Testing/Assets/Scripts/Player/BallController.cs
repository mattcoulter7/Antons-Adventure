using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float maxForceMultiplier = 2f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(Vector2.right * maxForceMultiplier);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rb.AddForce(Vector2.right * h * maxForceMultiplier);
        rb.AddForce(Vector2.up * v * maxForceMultiplier);
    }
}
