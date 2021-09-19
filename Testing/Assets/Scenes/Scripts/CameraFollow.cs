using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject trackObject;
    public float horizontalAdjustment = 0;
    public float maxZoomOutScale = 2f;
    public float zoomWhen = 1f; // within 1 unit of top of frame
    public float zoomSpeed = 2f;
    public float smoothSpeed = 0.125f;
    private float originalHeight;
    private float originalY;

    private float GetZoomFactor(){
        return Camera.main.orthographicSize / originalHeight;
    }
    private bool NeedToZoomOut(){
        Vector3 worldTop =  Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        return (worldTop.y - trackObject.transform.position.y) < zoomWhen;
    }
    private bool CanZoomOut(){
        return GetZoomFactor() <= maxZoomOutScale;
    }
    private bool NeedToZoomIn(){
        Vector3 worldTop =  Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        return trackObject.transform.position.y < worldTop.y - zoomWhen;
    }
    private bool CanZoomIn(){
        return Camera.main.orthographicSize > originalHeight;
    }
    private void ZoomToFit(){
        if (NeedToZoomOut() && CanZoomOut()){
            Camera.main.orthographicSize += Time.deltaTime * zoomSpeed;
        }
        else if (NeedToZoomIn() && CanZoomIn()){
            Camera.main.orthographicSize -= Time.deltaTime * zoomSpeed;
        }
    }

    void Start(){
        originalHeight = Camera.main.orthographicSize;
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // new position fix y due to middle anchor
        Vector3 newPosition = new Vector3(transform.position.x,originalY * GetZoomFactor(),transform.position.z);
        
        // align horizontally with target object
        float toTrackObject = trackObject.transform.position.x - transform.position.x + horizontalAdjustment;
        if (toTrackObject > 0f)
            newPosition += Vector3.right * toTrackObject;
        
        // adjust zoom scale to object position
        ZoomToFit();

        Vector3 smoothedPosition = Vector3.Lerp(transform.position,newPosition,smoothSpeed);
        transform.position = newPosition;
    }
}
