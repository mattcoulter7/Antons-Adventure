using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyNotVisible : MonoBehaviour
{
    private Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_renderer){
            if (!_renderer.isVisible){
                Destroy(gameObject);
            }
        }
    }
}
