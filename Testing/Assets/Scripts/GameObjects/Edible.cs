using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour
{
    public float points = 100f;

    void OnTriggerEnter2D(Collider2D col){
        // add points to the collided objects streak
        Streak s = col.GetComponent<Streak>();
        if (s != null){
            s.Receive(points);
        }

        // delet itself as it has been eaten
        Destroy(gameObject);
    }
}
