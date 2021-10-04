using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{
    public GameObject determinedBy;
    private float _initialPos;
    public float GetDistance(){
        return determinedBy.transform.position.x - _initialPos;
    }
    // Start is called before the first frame update
    void Start()
    {
        _initialPos = determinedBy.transform.position.x;
    }
}
