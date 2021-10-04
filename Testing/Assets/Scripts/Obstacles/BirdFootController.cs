using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFootController : MonoBehaviour
{
    public Sprite closedSprite;
    public float dropTime = 2f;
    public Vector2 catchForce = new Vector2(-100,100);
    private float _startTime;
    private bool _grabbed = false;
    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_grabbed && (Time.time - _startTime > dropTime)){
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0; // remove gravity as it flies away
            rb.velocity = catchForce; // apply fly away force
            GetComponent<SpriteRenderer>().sprite = closedSprite; // update sprite to be the closed one
            
            _grabbed = true;
        }
    }
}
