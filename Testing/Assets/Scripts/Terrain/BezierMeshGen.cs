using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMeshGen : MonoBehaviour
{
    public class Segment{
        public float Length(){
            return pts[3].x - pts[0].x;
        }
        public Vector2[] pts = new Vector2[4]; // array 4 points for cubic bezier
        public List<Vector2> bezierPts; // list of pts that make the bezier curve
        public MeshFilter filter { get; set; } // mesh filter for rendering. Access bezier vertices via filter.mesh.vertices
        public Vector2 MidPT(){
            // mid point in between three and four for smoothing
            float xDiff = (pts[3].x - pts[2].x) / 2;
            float yDiff = (pts[3].y - pts[2].y) / 2;
            return new Vector2(pts[2].x + xDiff,pts[2].y + yDiff);
        }
        public void Log(){
            // debug 
            string points = "";
            points += "pts[0]: (" + pts[0].x + ", " + pts[0].y + "), ";
            points += "pts[1]: (" + pts[1].x + ", " + pts[1].y + "), ";
            points += "pts[2]: (" + pts[2].x + ", " + pts[2].y + "), ";
            points += "pts[3]: (" + pts[3].x + ", " + pts[3].y + "), ";
            points += "pts[2]&pts[3] MP: (" + MidPT().x + ", " + MidPT().y + ") ";
        }
    }
    public int SegmentResolution = 32; // number of steps in segment
    public float minHeight = 0; // min new point height value
    public float maxHeight = 5;// max new point  height value

    public float minStep = 5; // min new point distance from last point
    public float maxStep = 10; // max new point distance between from last point
    public float boundaryWidth = 10; // Width in which the points are created/destroyed, should be at least maxStep
    
    public int MeshObjectPoolSize = 30; // Number of MeshFilter initialised on awake
    public MeshFilter SegmentPrefab;// the prefab including MeshFilter and MeshRenderer
    public bool renderDebugPoints = false;
    public GameObject renderPointPrefab;
    private List<Segment> _usedSegments = new List<Segment>(); // list of segments that are current in use
    private Vector3[] _vertexArray;// helper array to generate new segment without further allocations
    private List<MeshFilter> _freeMeshFilters = new List<MeshFilter>();// the pool of free mesh filters
    private Dictionary<int,Vector3[]> _segmentVertexUpdateBuff = new Dictionary<int,Vector3[]>(); // int is index of segment
    
    public List<Segment> GetSegments(){
        return _usedSegments; // for adding pattern to mesh from other scripts
    }
    private Vector2 RandomPoint(float minX)
    {
        // returns a new random point based upon the parameters
        return new Vector2(
            minX + Random.Range(minStep, maxStep),
            Random.Range(minHeight, maxHeight)
        );
    }
    private float cubicBezierPoint(float a0, float a1, float a2, float a3, float t){
        // get bezier pt on one axis
        return Mathf.Pow(1-t, 3) * a0 + 3* Mathf.Pow(1-t, 2) * t * a1 + 3*(1-t) * Mathf.Pow(t, 2) * a2 + Mathf.Pow(t, 3) * a3;
    }
    
    private Vector2 GetBezierPoint(float t, Segment seg)
    {
        // get 2d bezier pt at time t: 0 <= t <= 1; Fraction of x/length
        return new Vector2(
            cubicBezierPoint(seg.pts[0].x,seg.pts[1].x,seg.pts[2].x,seg.MidPT().x,t), 
            cubicBezierPoint(seg.pts[0].y,seg.pts[1].y,seg.pts[2].y,seg.MidPT().y,t) 
        );
    }
    private Segment FrontSegment() { 
        // Get most left segment
        return _usedSegments[0];
    }
    private Segment BackSegment() { 
        // Get the most right segment
        return _usedSegments[_usedSegments.Count - 1]; 
    }
    private MeshFilter BorrowMeshFilter(){
        // gets a meshfilter from the queue
        int meshIndex = _freeMeshFilters.Count - 1;
        MeshFilter filter = _freeMeshFilters[meshIndex];
        _freeMeshFilters.RemoveAt(meshIndex);
        return filter;
    }
    private void ReturnMeshFilter(MeshFilter filter){
        // adds the filter back into the queue
        filter.gameObject.SetActive(false);
        _freeMeshFilters.Add(filter);
    }
    private void GenerateRenderPoints(Segment seg){
        // if rendering is enabled, draw the actual points that are being used
        if (renderDebugPoints && renderPointPrefab != null){
            foreach (Vector2 pt in seg.pts){
                GameObject renderPoint = Instantiate<GameObject>(renderPointPrefab);
                renderPoint.transform.position = new Vector3(pt.x,pt.y,1);
                Text myText = renderPoint.AddComponent<Text>();
                myText.text = "(" + pt.x + "," + pt.y + ")";
            }
        }
    }
    private float GetInitialX(){
        return Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x - 20;
    }
    private Segment GenerateSegment(){
        // creates a new segment from the end of the last segment (ifdefined) to a new random point. Applies the graph to the mesh at the same time;
        Segment seg = new Segment();

        seg.pts[0] = _usedSegments.Count == 0 ? RandomPoint(GetInitialX()) : BackSegment().MidPT(); // create a copy of the vector at the end for start of new segment
        seg.pts[1] = _usedSegments.Count == 0 ? RandomPoint(seg.pts[0].x) : BackSegment().pts[3];
        seg.pts[2] = RandomPoint(seg.pts[1].x);
        seg.pts[3] = RandomPoint(seg.pts[2].x);
        seg.filter = BorrowMeshFilter();
        seg.Log();

        Mesh mesh = seg.filter.mesh;
        float step = seg.Length() / (SegmentResolution - 1);
        List<Vector2> bezierPts = new List<Vector2>();
        for (int i = 0; i < SegmentResolution; ++i)
        {
            // get the relative x position
            float x = step * i;
            float t = x / seg.Length();
            Vector2 bezierPt = GetBezierPoint(t,seg);
            bezierPts.Add(bezierPt);
            // top vertex
            _vertexArray[i * 2] = new Vector3(bezierPt.x, bezierPt.y, 0);

            // bottom vertex always at y=0
            _vertexArray[i * 2 + 1] = new Vector3(bezierPt.x, 0, 0);
        }
        seg.bezierPts = bezierPts;
        mesh.vertices = _vertexArray;

        // need to recalculate bounds, because mesh can disappear too early
        mesh.RecalculateBounds();

        // position
        seg.filter.transform.position = new Vector3(0, 0, 0);

        // collider
        MeshCollider2D[] colliders = seg.filter.gameObject.GetComponents<MeshCollider2D>(); // multiple because of istrigger
        foreach (MeshCollider2D collider in colliders){
            collider.UpdateCollider();
        }

        // make visible
        seg.filter.gameObject.SetActive(true);
        GenerateRenderPoints(seg);

        // store in used segments
        _usedSegments.Add(seg);
        return seg;
    }
    public int GetSegmentIndex(float x){
        // return the segment that global x resides within
        for (int i = 0; i < _usedSegments.Count; i++){
            BezierMeshGen.Segment seg = _usedSegments[i];
            
            Vector2 start = seg.bezierPts[0];
            Vector2 end = seg.bezierPts[seg.bezierPts.Count - 1];
            if (start.x >= x && x > end.x)
                return i;
        }
        return -1;
    }

    public int GetVertexIndex(Segment seg,float x){
        float t = x / seg.Length();
        int index = Mathf.FloorToInt(t * seg.bezierPts.Count);
        if (index < 0 || index > seg.bezierPts.Count - 1)
            return -1;
        return index;
    }

    public void AffectVertexHeight(int segmentIndex, int vertexIndex,float amount = 0){
        // external affects to the vertices call this function to affect y values
        Segment seg = _usedSegments[segmentIndex];
        if (!_segmentVertexUpdateBuff.ContainsKey(segmentIndex)){
            _segmentVertexUpdateBuff[segmentIndex] = seg.filter.mesh.vertices;
            // update to globa positions once
            for (int i = 0; i < seg.bezierPts.Count; i++){
                Vector2 bezierPt = seg.bezierPts[i];
                _segmentVertexUpdateBuff[segmentIndex][i * 2] = new Vector3(bezierPt.x,bezierPt.y,0);
            }
        }
        Vector3[] verts = _segmentVertexUpdateBuff[segmentIndex]; // todo ensure the dictionary array is updated here
        verts[vertexIndex * 2].y += amount;
    }
    private void ApplyVertexHeightUpdates(){
        // any changes made from AffectVertexHeight are applied once per update
        foreach(KeyValuePair<int, Vector3[]> entry in _segmentVertexUpdateBuff)
        {
            Segment seg = _usedSegments[entry.Key];
            seg.filter.mesh.vertices = entry.Value;
        
            MeshCollider2D collider = seg.filter.gameObject.GetComponent<MeshCollider2D>();
            collider.UpdateCollider();
        }
        // remove the buff
        _segmentVertexUpdateBuff.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyVertexHeightUpdates();
        // ensure there is always segments visible by creating more to fill the screen
        Vector3 worldRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        // create new points as they approach
        while (BackSegment().pts[3].x < (worldRight.x + boundaryWidth)){
            GenerateSegment();
        }

        // removes segments that have left the screen
        Vector3 worldLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

        // remove points from the front one segment is completely out
        while (FrontSegment().pts[3].x < (worldLeft.x - boundaryWidth)){
            ReturnMeshFilter(FrontSegment().filter);
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
        // create an initial starting segment that the rest continue to
        Vector3 worldLeft = Camera.main.ViewportToWorldPoint(new Vector3(-1, 0, 0));
        GenerateSegment();
    }
}
