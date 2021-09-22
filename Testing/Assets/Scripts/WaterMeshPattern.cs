using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMeshPattern : MonoBehaviour
{
    public float speed = 1;
    public float scale = 1;
    public bool updateCollider = true;
    private BezierMeshGen _bezierMeshGen;

    private List<BezierMeshGen.Segment> _segments;
    void Start()
    {
        _bezierMeshGen = gameObject.GetComponent<BezierMeshGen>();
    }

    private float GetHeight(Vector2 pt){
        return (float)(Mathf.Sin(Time.time * speed + pt.x + pt.y) * (scale*.5)
        + Mathf.Sin(Time.time * speed + pt.y) * (scale*.5));
    }

    // Update is called once per frame
    void Update()
    {
        if (!_bezierMeshGen) return;
        _segments = _bezierMeshGen.GetSegments();

        // every segment
        for (int i = 0; i < _segments.Count; ++i){
            BezierMeshGen.Segment seg = _segments[i];
            // every bezier point in segment
            for (int j = 0; j < seg.bezierPts.Count; ++j){
                Vector2 worldVertex = seg.bezierPts[j];
                // calculate sin wave value
                float newHeight = GetHeight(worldVertex);
                _bezierMeshGen.AffectVertexHeight(i,j,newHeight);
            }
        }
    }
}
