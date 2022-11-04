using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCanvasSize : MonoBehaviour
{
    public Vector2 size = new Vector2(1, 1);
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().size = size;
    }
}
