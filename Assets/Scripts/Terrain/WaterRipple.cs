using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRipple : MonoBehaviour
{
    struct Ripple {
        public Vector2 origin;
        public float currentScale;
    }
    public float scale = 100; // scale of ripple
    public float speed = 1; // speed in which the ripple vanishes
    private List<Ripple> _ripples = new List<Ripple>();
    private BezierMeshGen _bezierMeshGen;

    public void CreateRipple(float x,float impact = 1f){
        // get the segment at position x
        int segIndex = _bezierMeshGen.GetSegmentIndex(x);
        if (segIndex == -1) return;
        BezierMeshGen.Segment seg = _bezierMeshGen.GetSegments()[segIndex];

        // get the vertex within the segment at position x
        int vertexIndex = _bezierMeshGen.GetVertexIndex(seg,x);
        if (vertexIndex == -1) return;
        Vector2 origin = seg.bezierPts[vertexIndex];

        // create ripple object and add it to current collection
        Ripple ripple = new Ripple();
        ripple.currentScale = scale * impact;
        ripple.origin = origin;

        _ripples.Add(ripple);
    }

    private float GetUpdateScale(float scale){
        // function for affect on ripple scale over time
        return scale - Time.deltaTime * speed;
    }

    private void UpdateRipple(Ripple rip){
        // affect curve
        // update scale
        rip.currentScale = GetUpdateScale(rip.currentScale);
    }

    private void UpdateActiveRipples(){
        for (int i = 0; i < _ripples.Count;){
            Ripple rip = _ripples[i];
            UpdateRipple(rip);
            // remove if scale reaches 0
            if (rip.currentScale <= 0)
                _ripples.RemoveAt(i);
            else
                ++i;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _bezierMeshGen = gameObject.GetComponent<BezierMeshGen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_bezierMeshGen) return;
    }
}
