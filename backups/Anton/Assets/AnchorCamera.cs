using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorCamera : MonoBehaviour
{
    public Camera camera;
    public Vector2 offset = new Vector2(0,0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector2(camera.pixelWidth, camera.pixelHeight));
        transform.position = topRight + offset;
    }
}
