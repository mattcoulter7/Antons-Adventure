using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inherit this for various spawners of obstacles
[RequireComponent(typeof(ObjectPool))]
abstract public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 100;
    public float[] spawnDistanceInterval = new float[2] { 5, 10 };

    private float _nextDistance = 0f;
    public Distance distance;
    public ObjectPool objectPool;
    private void SetRandomNextTime()
    {
        _nextDistance += Random.Range(spawnDistanceInterval[0], spawnDistanceInterval[1]);
    }

    public abstract void Spawn();
    
    void Start()
    {
        SetRandomNextTime();
        if (objectPool == null){
            objectPool = GetComponent<ObjectPool>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (distance.distance >= _nextDistance)
        {
            Spawn();
            SetRandomNextTime();
        }
    }
}
