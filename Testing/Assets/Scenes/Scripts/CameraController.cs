using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 0.01f;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed;
    }
}
