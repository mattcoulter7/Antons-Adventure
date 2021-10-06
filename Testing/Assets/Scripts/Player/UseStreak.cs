using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseStreak : MonoBehaviour
{
    public Streak streak;
    public Vector2 streakAppliedForce = new Vector2(100,50);
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (streak.area == streak.maxArea){
            _rb.AddForce(streakAppliedForce);
            streak.Deplete();
        }
    }
}
