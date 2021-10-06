using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streak : MonoBehaviour
{
    // track the area covered by summing y value every frame
    public float constantDecreaseAmount = 1f; // area is constantly decreasing by an amount
    public float minArea = 1f; // area cannot be below this
    public float maxArea = 1000000f; // area cannot exceed this
    private float _areaCovered = 0f;
    public float area => _areaCovered;
    public float areaFactor => (_areaCovered / (maxArea-minArea));
    void Start(){
        _areaCovered = minArea;
    }

    public void Deplete(){
        _areaCovered = minArea;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newArea = _areaCovered + transform.position.y - constantDecreaseAmount;
        if (newArea > maxArea){
            newArea = maxArea;
        }
        if (newArea < minArea){
            newArea = minArea;
        }
        _areaCovered = newArea;
    }
}
