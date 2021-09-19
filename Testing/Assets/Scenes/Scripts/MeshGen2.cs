using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen2 : MonoBehaviour
{
    public float minHeight = 0; // min height value
    public float maxHeight = 5;// max height value

    public float minStep = 5; // min distance between 2 points
    public float maxStep = 10; // max distance between two points
    public float boundaryWidth = 10;// Width in which the points are created/destroyed, should be at least maxStep
    private List<Vector2> pts = new List<Vector2>();
    // returns a new random point based upon the parameters
    private Vector2 RandomPoint()
    {
        return new Vector2(
            LastPoint().x + Random.Range(minStep, maxStep),
            Random.Range(minHeight, maxHeight)
        );
    }
    private Vector2 FirstPoint() { return pts[0]; }
    private Vector2 LastPoint() { return pts[pts.Count - 1]; }

    private void EnsureVisiblePoints(){
        Vector3 worldRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        
        // create new points as they approach
        while (LastPoint().x < worldRight.x + boundaryWidth){
            Vector2 newPt = RandomPoint();
            pts.Add(newPt);
        }
    }

    private void EnsureInvisiblePoints(){
        Vector3 worldLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

        // remove points from the front
        while (FirstPoint().x < worldLeft.x + boundaryWidth){
            pts.RemoveAt(0);
        }
    }



    void Start(){
        pts.Add(new Vector2(0,0));
    }
    // Update is called once per frame
    void Update()
    {
        EnsureVisiblePoints();
        EnsureInvisiblePoints();
    }
}
