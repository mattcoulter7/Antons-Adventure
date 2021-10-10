using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateStreakMeter : MonoBehaviour
{
    public Streak streakMeter;
    public float maxScale = 1f;
    public float minScale = 0.1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float streakVal = streakMeter.area;
        float streakFactor = streakMeter.areaFactor;

        float newScale = minScale + (streakFactor * (maxScale - minScale));

        transform.localScale = new Vector3(newScale,newScale,newScale);
    }
}
