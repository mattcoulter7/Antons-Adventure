using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyController : MonoBehaviour
{
    public float dropSpeed = 0.001f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.y -= dropSpeed;
        transform.position = position;
    }
}
