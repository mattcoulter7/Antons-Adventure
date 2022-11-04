using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomConstantVelocity : MonoBehaviour
{
    public float[] dropSpeedRangeX = new float[2]{0,0};
    public float[] dropSpeedRangeY = new float[2]{0,0};
    private Vector2 _velocity;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(
            Random.Range(dropSpeedRangeX[0],dropSpeedRangeX[1]),
            Random.Range(dropSpeedRangeY[0],dropSpeedRangeY[1])
        );
    }
}
