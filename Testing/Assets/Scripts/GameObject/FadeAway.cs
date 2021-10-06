using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    public float fadeDuration=1.0f; // fade for 1 second
    public float fadeAfter=1.0f; // fade after 1 second
    private Color _alphaColor;
    private float _startTime;
    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
        _alphaColor = GetComponent<MeshRenderer>().material.color;
        _alphaColor.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float currentDuration = Time.time - _startTime;
        if (currentDuration > fadeAfter){
            GetComponent<SpriteRenderer>().material.color = Color.Lerp(GetComponent<SpriteRenderer>().material.color, _alphaColor, fadeDuration * Time.deltaTime);
        }
    }
}
