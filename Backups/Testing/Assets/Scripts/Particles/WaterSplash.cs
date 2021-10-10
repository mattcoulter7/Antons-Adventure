using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    public GameObject particlePrefab;
    public float minSpashVelocity = 0.1f;
    public void CreateAt(Vector2 pos){
        Instantiate(particlePrefab,pos,Quaternion.identity);
    }

    void OnCollisionEnter2D(Collision2D col){
        if (particlePrefab == null){
            return;
        }
        float impactVelocity = col.relativeVelocity.magnitude;
        // spawn new particles
        if (impactVelocity > minSpashVelocity){
            Vector2 pos = col.contacts[0].point;
            CreateAt(pos);
        }
    }
}