using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen2 : MonoBehaviour
{
    class Segment{
        public float Gradient(){
            return (pt4.y - pt1.y)/(pt4.x - pt1.x);
        }
        public float Length(){
            return pt4.x - pt1.x;
        }
        public Vector2 pt1 { get; set; }
        public Vector2 pt2 { get; set; }
        public Vector2 pt3 { get; set; }
        public Vector2 pt4 { get; set; }
        public Vector2 MidPT(){
            float xDiff = (pt4.x - pt3.x) / 2;
            float yDiff = (pt4.y - pt3.y) / 2;
            return new Vector2(pt3.x + xDiff,pt3.y + yDiff);
        }
        public void Log(){        
            string points = "";
            points += "(" + pt1.x + ", " + pt1.y + "), ";
            points += "(" + pt2.x + ", " + pt2.y + "), ";
            points += "(" + pt3.x + ", " + pt3.y + "), ";
            points += "(" + pt4.x + ", " + pt4.y + "), ";
            points += "MP: (" + MidPT().x + ", " + MidPT().y + ") ";
            Debug.Log(points);
        }
        public MeshFilter filter { get; set; }
    }
    public int SegmentResolution = 32; // number of steps between 2 points
    public float minHeight = 0; // min height value
    public float maxHeight = 5;// max height value

    public float minStep = 5; // min distance between 2 points
    public float maxStep = 10; // max distance between two points
    public float boundaryWidth = 10;// Width in which the points are created/destroyed, should be at least maxStep
    
    public int MeshObjectPoolSize = 30;
    public MeshFilter SegmentPrefab;// the prefab including MeshFilter and MeshRenderer
    private List<Segment> _usedSegments = new List<Segment>(); // list of segments that are current in use
    private Vector3[] _vertexArray;// helper array to generate new segment without further allocations
    private List<MeshFilter> _freeMeshFilters = new List<MeshFilter>();// the pool of free mesh filters
    
    // returns a new random point based upon the parameters
    private Vector2 RandomPoint(float minX)
    {
        return new Vector2(
            minX + Random.Range(minStep, maxStep),
            Random.Range(minHeight, maxHeight)
        );
    }
    // returns y value of curve in a segment
    private float GetHeight(Segment seg,float x){
        // x value is RELATIVE to start of segment
        float absX = seg.pt1.x + x;
        float m = seg.Gradient(); // gradient
        float c = seg.pt1.y; // vertical translation
        float a = seg.pt1.x; // horizontal translation
        // cubic function to create an s-like shape intersecting at two points
        return m * (3*Mathf.Pow((absX-a),2) - 2*Mathf.Pow((absX-a),3)) + c;
    }

    private float cubicBezierPoint(float a0, float a1, float a2, float a3, float t){
        return Mathf.Pow(1-t, 3) * a0 + 3* Mathf.Pow(1-t, 2) * t * a1 + 3*(1-t) * Mathf.Pow(t, 2) * a2 + Mathf.Pow(t, 3) * a3;
    }
    
    private Vector2 GetBezierPoint(float t, Segment seg)
    {
        return new Vector2(
            cubicBezierPoint(seg.pt1.x,seg.pt2.x,seg.pt3.x,seg.MidPT().x,t), 
            cubicBezierPoint(seg.pt1.y,seg.pt2.y,seg.pt3.y,seg.MidPT().y,t) 
        );
    }
    private Segment FrontSegment() { return _usedSegments[0]; }
    private Segment BackSegment() { return _usedSegments[_usedSegments.Count - 1]; }
    // gets a meshfilter from the queue
    private MeshFilter BorrowMeshFilter(){
        // get from the pool
        int meshIndex = _freeMeshFilters.Count - 1;
        MeshFilter filter = _freeMeshFilters[meshIndex];
        _freeMeshFilters.RemoveAt(meshIndex);
        return filter;
    }
    // adds the filter back into the queue
    private void ReturnMeshFilter(MeshFilter filter){
        filter.gameObject.SetActive(false);
        _freeMeshFilters.Add(filter);
    }
    // creates a new segment from the end of the last segment to a new random point. Applies the graph to the mesh at the same time;
    private Segment GenerateSegment(){
        Segment seg = new Segment();
        seg.pt1 = BackSegment().MidPT(); // create a copy of the vector at the end for start of new segment
        seg.pt2 = BackSegment().pt4;
        seg.pt3 = RandomPoint(seg.pt2.x);
        seg.pt4 = RandomPoint(seg.pt3.x);
        seg.filter = BorrowMeshFilter();
        Debug.Log("Creating New Point: ");
        seg.Log();

        Mesh mesh = seg.filter.mesh;
        float step = seg.Length() / (SegmentResolution - 1);

        List<Vector2> bezierPts = new List<Vector2>();
        for (int i = 0; i < SegmentResolution; ++i)
        {
            // get the relative x position
            float x = step * i;
            float t = (x) / seg.Length();
            Vector2 bezierPt = GetBezierPoint(t,seg);
            bezierPts.Add(bezierPt);
            // top vertex
            // float yPosTop = GetHeight(seg,x); // position passed to GetHeight() must be absolute
            _vertexArray[i * 2] = new Vector3(bezierPt.x, bezierPt.y, 0);

            // bottom vertex always at y=0
            _vertexArray[i * 2 + 1] = new Vector3(bezierPt.x, 0, 0);
        }
        mesh.vertices = _vertexArray;

        // need to recalculate bounds, because mesh can disappear too early
        mesh.RecalculateBounds();

        // position
        seg.filter.transform.position = new Vector3(bezierPts[0].x - (bezierPts[bezierPts.Count - 1].x - bezierPts[0].x), 0, 0);
        Debug.Log("Bezier Start: " + bezierPts[0].x);
        Debug.Log("Bezier End: " + bezierPts[bezierPts.Count - 1].x);

        // collider
        MeshCollider2D collider = seg.filter.gameObject.GetComponent<MeshCollider2D>();
        collider.UpdateCollider();

        // make visible
        seg.filter.gameObject.SetActive(true);

        _usedSegments.Add(seg);
        return seg;
    }
    
    private void EnsureSegmentsVisible(){
        // ensures there is always segments visible by creating more to fill the screen
        Vector3 worldRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        
        // create new points as they approach
        while (BackSegment().pt4.x < (worldRight.x + boundaryWidth)){
            GenerateSegment();
        }
    }
    private void EnsureSegmentsNotVisible(){
        // removes segments that have left the screen
        Vector3 worldLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

        // remove points from the front one segment is completely out
        while (FrontSegment().pt4.x < (worldLeft.x - boundaryWidth)){
            _usedSegments.RemoveAt(0);
        }
    }

    void Awake()
    {
        // Create vertex array helper
        _vertexArray = new Vector3[SegmentResolution * 2];

        // Build triangles array. For all meshes this array always will
        // look the same, so I am generating it once 
        int iterations = _vertexArray.Length / 2 - 1;
        var triangles = new int[(_vertexArray.Length - 2) * 3];

        for (int i = 0; i < iterations; ++i)
        {
            int i2 = i * 6;
            int i3 = i * 2;

            triangles[i2] = i3 + 2;
            triangles[i2 + 1] = i3 + 1;
            triangles[i2 + 2] = i3 + 0;

            triangles[i2 + 3] = i3 + 2;
            triangles[i2 + 4] = i3 + 3;
            triangles[i2 + 5] = i3 + 1;
        }

        // Create colors array. For now make it all white.
        var colors = new Color32[_vertexArray.Length];
        for (int i = 0; i < colors.Length; ++i)
        {
            colors[i] = new Color32(255, 255, 255, 255);
        }

        // Create game objects (with MeshFilter) instances.
        // Assign vertices, triangles, deactivate and add to the pool.
        for (int i = 0; i < MeshObjectPoolSize; ++i)
        {
            MeshFilter filter = Instantiate(SegmentPrefab);

            Mesh mesh = filter.mesh;
            mesh.Clear();

            mesh.vertices = _vertexArray;
            mesh.triangles = triangles;

            filter.gameObject.SetActive(false);
            _freeMeshFilters.Add(filter);
        }
    }

    void Start(){
        Vector3 worldLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Segment seg = new Segment();
        seg.pt1 = new Vector2(worldLeft.x,worldLeft.y);
        seg.pt2 = RandomPoint(seg.pt1.x);
        seg.pt3 = RandomPoint(seg.pt2.x);
        seg.pt4 = RandomPoint(seg.pt3.x);
        _usedSegments.Add(seg);
    }
    // Update is called once per frame
    void Update()
    {
        EnsureSegmentsVisible();
        EnsureSegmentsNotVisible();
    }
}
