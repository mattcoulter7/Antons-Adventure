using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyLeftOfScreen : MonoBehaviour
{
    public float xOffset = 0f;
    private Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float cameraLeft = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x;

        // out of screen
        if (transform.position.x - xOffset < cameraLeft){
            // return to object pool if it exists otherwise destory it
            ObjectPoolReference objPoolRef = GetComponent<ObjectPoolReference>();
            if (objPoolRef && objPoolRef.objectPool){
                objPoolRef.objectPool.Return(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
    }
}