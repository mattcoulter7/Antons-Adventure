using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameOver gameOverScreen;
    public bool gameOver = false;

    public float maxForceMultiplier = 2f;
    private Rigidbody2D rb;
    public float dThrust = 50f;
    private Vector2 tangent;
    private bool colliding = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(Vector2.right * maxForceMultiplier);
        rb.AddForce(transform.right * 500f);
    }

    public void GameOver() 
    {
        gameOver = true;
        gameOverScreen.Setup();
    }

    void Update()
    {
        if (!gameOver)
        {
            //Downward force added if space or left mouse button clicked
            if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("input");
                rb.AddForce(-transform.up * dThrust);
                rb.AddForce(transform.right * 5f); //cheeky bit of sideways force to help with feel
            }

            if (rb.velocity.x < 0)
            {
                GameOver();
            }
            colliding = false;
        }
    }

    // calculation the perpendicular vector to collision
    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.name == "Segment(Clone)"){
            colliding = true;
            Vector2 normal = col.contacts[0].normal;
            Vector2 perp = Vector2.Perpendicular(normal) * -1;

            float currMagnitude = rb.velocity.magnitude;
            perp.Normalize();

            perp *= currMagnitude;
            tangent = perp;
            foreach (var item in col.contacts)
            {
                Vector2 perpdebug = Vector2.Perpendicular(item.normal) * -1;
                //Debug.DrawRay(item.point, perpdebug * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
            }
        }
    }
}
