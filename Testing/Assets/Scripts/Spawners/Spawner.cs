using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inherit this for various spawners of obstacles
abstract public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float[] spawnDistanceInterval = new float[2] { 5, 10 };

    private float _nextDistance = 0f;
    private Distance _distance;
    public Distance distance => _distance;
    
    private void SetRandomNextTime()
    {
        _nextDistance += Random.Range(spawnDistanceInterval[0], spawnDistanceInterval[1]);
    }

    public abstract void Spawn();
    
    void Start()
    {
        _distance = Camera.main.GetComponent<Distance>();
        SetRandomNextTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (_distance.GetDistance() >= _nextDistance)
        {
            Spawn();
            SetRandomNextTime();
        }
    }
}
