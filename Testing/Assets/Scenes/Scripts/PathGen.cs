using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Runtime;

public class PathGen : MonoBehaviour
{
    public float minStepWidth = 1; // width between random points generated
    public float maxStepWidth = 1;
    public float minY = 0; // potential min point height
    public float maxY = 1; // potential max point height
    public float boundaryDistance = 1; // should be >= maxStepWidth
    
    private float lastX;
    private SpriteShapeController shapeController; // reference to shapeController component
    private Spline spline; // reference to shapeController's spline for points
    private EdgeCollider2D edgeCollider; // edge collider for getting position properies of the path
    void Awake(){
        shapeController = gameObject.GetComponent<SpriteShapeController>() as SpriteShapeController;
        edgeCollider = gameObject.GetComponent<EdgeCollider2D>() as EdgeCollider2D;
        spline = shapeController.spline;
        lastX = GetRightMostPoint().x;
    }

    private float RandomY(){
        return Random.Range(minY,maxY);
    }
    private float RandomX(){
        return Random.Range(minStepWidth,maxStepWidth);
    }
    private Vector3 AddPoint(){
        Vector3 newPoint = new Vector3(lastX + RandomX(),RandomY(),0f);
        int newIndex = spline.GetPointCount();

        spline.InsertPointAt(newIndex,newPoint);
        spline.SetTangentMode(newIndex,ShapeTangentMode.Continuous);
        
        return newPoint;
    }
    private Vector2 GetLeftMostPoint(){
        // point is from edge collider as cannot access the points in spline
        return edgeCollider.points[0];
    }
    private Vector2 GetRightMostPoint(){
        // point is from edge collider as cannot access the points in spline
        return edgeCollider.points[edgeCollider.pointCount - 1];
    }

    private List<Vector3> AddPoints(){
        // return list of new points added
        List<Vector3> newPts = new List<Vector3>();
        // endX is current end of path
        Vector3 worldRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        // continue to add points until the point is outside of the screen
        while (lastX < worldRight.x + boundaryDistance){
            Vector3 pt = AddPoint();
            lastX = pt.x;

            newPts.Add(pt);
        }
        return newPts;
    }

    private void RemovePoints(){
        float startX = GetLeftMostPoint().x;
        Vector3 worldLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        // continue to remove points that are no longer visible
        while (startX < worldLeft.x - boundaryDistance){
            spline.RemovePointAt(0);
        }
    }

    void Update(){
        // add any new points that need to be added
        List<Vector3> added = AddPoints();
        // remove any points that should be removed
        //RemovePoints();
        
        // refresh the collider for the new points added
        if (added.Count > 0)
            shapeController.BakeCollider();
    }
}
