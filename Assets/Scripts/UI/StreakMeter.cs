using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreakMeter : MonoBehaviour
{
    public Streak streak;
    public float maxScale = 1f;
    public float minScale = 0.1f;

    // Update is called once per frame
    void Update()
    {
        float streakVal = streak.area;
        float streakFactor = streak.areaFactor;

        float newScale = minScale + (streakFactor * (maxScale - minScale));

        //transform.localScale = new Vector3(newScale,newScale,newScale);
        transform.Rotate(0, 0, newScale * Time.deltaTime);
    }
}
