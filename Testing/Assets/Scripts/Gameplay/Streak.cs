using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    Track the area covered by summing y value every frame
    auto decharges if not much is gained
    Applies the force when area hits max
*/
public class Streak : MonoBehaviour
{
    public float constantDecreaseAmount = 1f; // area is constantly decreasing by an amount
    public float minArea = 1f; // area cannot be below this
    public float maxArea = 10000f; // area cannot exceed this
    public float increaseFactor = 1.0f; // rate in which the area charges up
    
    private float _areaCovered = 0f; // the area being summed up
    public float area => _areaCovered; // public getter
    public float areaFactor => (_areaCovered / (maxArea-minArea)); // factor for scaling


    public Vector2 streakAppliedForce = new Vector2(2000,500); // ammount of force applied when charged up
    public float tolerance = 50f; // tolerance below maximum to apply to streak
    public float cooldown = 2f; // amount of time to wait after streak used to regen
    private Rigidbody2D _rb; // rigid body reference for applying force
    private float _lastUsed = -1f;

    void Start(){
        _areaCovered = minArea;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get time since the streak ability was last used
        float timeSinceLastUse = Time.time - _lastUsed;

        // only charge up if used after cooldown duration
        if (_lastUsed == -1 || timeSinceLastUse > cooldown){
            float newArea = _areaCovered + (transform.position.y * increaseFactor) - constantDecreaseAmount;
            if (newArea > maxArea){ // truncate max
                newArea = maxArea;
            }
            if (newArea < minArea){ // truncate min
                newArea = minArea;
            }
            _areaCovered = newArea;
        }


        // apply the force when fully charged
        if (area >= maxArea - tolerance){
            _rb.AddForce(streakAppliedForce);
            _areaCovered = minArea;
            _lastUsed = Time.time;
        }
    }
}
