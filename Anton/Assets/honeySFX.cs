using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class honeySFX : MonoBehaviour
{
     public AudioSource SFX;

    void Start(){
        SFX = GetComponent<AudioSource> ();
    } 

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Player" && gameObject != null){
            SFX.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
