using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject trackObject;
    public float maxZoomOutScale = 2f; // times bigger than original height
    public Vector2 padding = new Vector2(1f,1f);
    public float smoothing = 0.125f;
    
    private float _ogOrthographicSize; // the original zoom amount
    private float _ogHeight; // the original camera height
    private float _ogY; // the original camera y position

    private Camera _camera;

    void Start(){
        _camera = GetComponent<Camera>();

        Vector3 bl = _camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 tr = _camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        _ogHeight = tr.y - bl.y;
        _ogOrthographicSize = _camera.orthographicSize;
        _ogY = _camera.transform.position.y;
    }
    private float GetCameraWidth(){
        // width of camera for if it is resized
        Vector3 bl = _camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 tr = _camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        return tr.x - bl.x;
    }

    private float GetZoomFactor(){
        // returns proportion of how much has been zoomed in for adjusting the bottom
        return _camera.orthographicSize / _ogOrthographicSize;
    }
    private void UpdateZoom(){
        // calculate the height required to "fit" the target in the screen
        float cameraBottom = _camera.ViewportToWorldPoint(new Vector3(0,0, 0)).y;
        float minFitHeight = trackObject.transform.position.y - cameraBottom;
        float targetHeight = padding.y + minFitHeight;
        
        // don't zoom in any further than origin height/ Don't zoom out any further than max height
        if ((targetHeight <= _ogHeight) || (targetHeight > (maxZoomOutScale * _ogHeight))) return;
        
        // calculate the orthographic size for this new height
        float targetOrthographicSize = _ogOrthographicSize * (targetHeight / _ogHeight);

        // apply smoothing for the zoom
        float toTarget = Mathf.Lerp(_camera.orthographicSize,targetOrthographicSize,smoothing);

        // update the zoom
        _camera.orthographicSize = toTarget;
    }

    private float GetDesiredY(){
        // zoomin out at center, ensure y position is still at original y position
        return _ogY * GetZoomFactor();
    }
    private float GetDesiredX(){
        // keep camera with target
        return trackObject.transform.position.x + (GetCameraWidth() / 2) - (padding.x);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (trackObject == null) return;
        // apply the zoom first
        UpdateZoom();
        
        // new position fix y due to middle anchor
        Vector3 targetPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        targetPosition.x = GetDesiredX();

        // apply the transformation
        Vector3 toTargetPosition = Vector3.Lerp(transform.position,targetPosition,0.125f);
        toTargetPosition.y = GetDesiredY();

        transform.position = toTargetPosition;
    }
}
