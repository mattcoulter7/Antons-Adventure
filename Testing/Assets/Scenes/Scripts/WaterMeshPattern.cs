using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMeshPattern : MonoBehaviour
{
    public BezierMeshGen bezierMeshGen;
    public float speed = 1;
    public float scale = 1;
    // Start is called before the first frame update
    void Start()
    {
        bezierMeshGen = gameObject.GetComponent<BezierMeshGen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bezierMeshGen) return;
        List<BezierMeshGen.Segment> segments = bezierMeshGen.GetSegments();
        foreach (BezierMeshGen.Segment seg in segments){
            Vector3[] verts = seg.filter.mesh.vertices;
            for (int i = 0; i < seg.bezierPts.Count; ++i){
                Vector2 pivotVertex = seg.bezierPts[i];
                
                verts[i * 2] = new Vector3(pivotVertex.x,pivotVertex.y,0);
                verts[i * 2].y += (float)(Mathf.Sin(Time.time * speed + pivotVertex.x + pivotVertex.y) * (scale*.5)
                 + Mathf.Sin(Time.time * speed + pivotVertex.y) * (scale*.5));
            }
            seg.filter.mesh.vertices = verts;
            MeshCollider2D collider = seg.filter.gameObject.GetComponent<MeshCollider2D>();
            collider.UpdateCollider();
        }
    }
}
