using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float BIGG = 9.8f;
    public GameObject Anton;
    //public Rigidbody2D phys;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CalculateGravity();
        CalculateRotation();
    }

    void CalculateGravity()
    {
        if (Input.GetKeyDown("space"))
        {
            Anton.GetComponent<Rigidbody2D>().gravityScale = BIGG * 2.0f;
        }
        else
        {
            Anton.GetComponent<Rigidbody2D>().gravityScale = BIGG * 1.0f;
        }
    }

    void CalculateRotation()
    {
        
    }
    
}