using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorCamera : MonoBehaviour
{
    public Camera camera;
    public Vector3 offset = new Vector3(0,0,0);

    // Update is called once per frame
    void Update()
    {
        Vector3 topRight = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight,0));
        transform.position = topRight + offset;
    }
}
