using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour
{
    public float points = 100f;
    public AudioClip honeySFX;

    void Start(){
        //honeySFX = GetComponent<AudioSource> ();
    } 

    void OnTriggerEnter2D(Collider2D col){
        if (col.isTrigger) return;
        // add points to the collided objects streak
        Streak s = col.GetComponent<Streak>();
        if (col.gameObject.tag == "Player"){
            AudioSource.PlayClipAtPoint(honeySFX, transform.position, 12f);
        }
        if (s != null){
            s.Receive(points);

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
