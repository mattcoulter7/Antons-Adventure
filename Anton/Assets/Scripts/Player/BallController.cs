using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameOver gameOverScreen;
    public bool notGameOver = true;

    public float maxForceMultiplier = 2f;
    private Rigidbody2D rb;
    public float dThrust = 50f;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(Vector2.right * maxForceMultiplier);
        rb.AddForce(transform.right * 500f);
        Debug.Log("test force");
    }

    public void GameOver() 
    {
        notGameOver = false;
        gameOverScreen.Setup();
    }

    void Update()
    {
        if (notGameOver)
        {
            //Downward force added if space or left mouse button clicked
            if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("input");
                rb.AddForce(-transform.up * dThrust);
                rb.AddForce(transform.right * 10f); //cheeky bit of sideways force to help with feel
            }
        }

        if (rb.velocity.x < 0)
        {
            GameOver();
        }
    }
}
