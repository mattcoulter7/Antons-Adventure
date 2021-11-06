using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour
{
    public float points = 100f;
    public AudioClip honeySFX;
    // public AudioSource honeySFX;
    // public AudioManager audioManager;

    void Start(){
        // honeySFX = GetComponent<AudioSource> ();
    } 

    void OnTriggerEnter2D(Collider2D col){
        if (col.isTrigger) return;
        // add points to the collided objects streak
        Streak s = col.GetComponent<Streak>();
        if (col.gameObject.tag == "Player"){
            AudioSource.PlayClipAtPoint(honeySFX, transform.position, 12f);
        //     //honeySFX.Play();
            // audioManager.Play("Honey");
            Debug.Log("Player Collide");
        }
        if (s != null){
            s.Receive(points);

            // return to object pool if it exists otherwise destory it
            ObjectPoolReference objPoolRef = GetComponent<ObjectPoolReference>();
            if (objPoolRef && objPoolRef.objectPool){
                objPoolRef.objectPool.Return(gameObject);
            } else {
                Destroy(gameObject);//Invoke("Kill", 1/2);
            }
        }

        
    }

    // void Kill(){
    //     
    // }
}
