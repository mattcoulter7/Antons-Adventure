using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public float[] dropSpeedRange = new float[2]{0.0005f,0.0015f};
    private float _dropSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _dropSpeed = Random.Range(dropSpeedRange[0],dropSpeedRange[1]);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.y -= _dropSpeed;
        transform.position = position;
    }
}
