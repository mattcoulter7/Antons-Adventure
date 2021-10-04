using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyTimer : MonoBehaviour
{
    public float destroyTime = 5f;
    private float _startTime;
    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _startTime > destroyTime){
            Destroy(gameObject);
        }
    }
}
