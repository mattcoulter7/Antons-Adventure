using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    public float fadeDuration = 1.0f; // fade for 1 second
    public float fadeAfter = 1.0f; // fade after 1 second
    private Color _alphaColor;
    private Color _ogColor;
    private float _startTime;
    private SpriteRenderer _sr;
    // Start is called before the first frame update
    void OnEnable()
    {
        _startTime = Time.time;
        _sr.material.color = _ogColor; // reset the color back to normal
    }
    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();

        _ogColor = _sr.material.color;
        _alphaColor = _sr.material.color;
        _alphaColor.a = 0;
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float currentDuration = Time.time - _startTime;
        if (currentDuration > fadeAfter)
        {
            _sr.material.color = Color.Lerp(_sr.material.color, _alphaColor, fadeDuration * Time.deltaTime);

            // return to pool once it is no longer visible
            if (_sr.material.color.a == 0){
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
}
