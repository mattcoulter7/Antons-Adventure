using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used for things such as items sitting above thew water
public class LandSpawner : Spawner
{
    public float heightOffset = 0f;
    public float[] spawnQuantityRange = new float[2]{1,1};
    public int spawnEvery = 1;
    public BezierMeshGen meshGenerator;
    
    public BezierMeshGen.Segment GetNextSegment()
    {
        // gets next segment outside of screen
        List<BezierMeshGen.Segment> segments = meshGenerator.GetSegments();
        return segments[segments.Count - 1];
    }
    public override void Spawn(){
        BezierMeshGen.Segment spawnSegment = GetNextSegment();

        // don't want max to exceed amount of points on segment
        float spawnQtyMax = Mathf.Min(spawnSegment.bezierPts.Count,spawnQuantityRange[1]);
        float spawnQty = Random.Range(spawnQuantityRange[0],spawnQtyMax);

        // spawn the objects
        for (int i = 0; i < spawnQty; i++){
            if (i % spawnEvery == 0){
                GameObject obj = objectPool.Borrow();
                if (obj){
                    Vector3 position = obj.transform.position;
                    position.x = spawnSegment.bezierPts[i].x;
                    position.y = spawnSegment.bezierPts[i].y;
                    position.y += heightOffset;
                    obj.transform.position = position;
                }
            }
        }

    }
}
