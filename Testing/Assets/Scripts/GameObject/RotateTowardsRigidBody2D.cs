using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsRigidBody2D : MonoBehaviour
{
    public float rotateSpeed = 0.05f;
    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = _rb.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q,rotateSpeed);
    }
}
