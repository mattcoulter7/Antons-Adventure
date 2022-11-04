using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{
    public float distance => _currentDistance;
    private float _initialPos;
    private float _currentDistance;
    // Start is called before the first frame update
    void Start()
    {
        _initialPos = transform.position.x;
    }
    void Update(){
        _currentDistance = transform.position.x - _initialPos;
    }
}
