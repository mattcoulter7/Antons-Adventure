using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splashSFX : MonoBehaviour
{

	public AudioSource splash;
	public float minSpashVelocity = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        splash = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col){
    	 float impactVelocity = col.relativeVelocity.magnitude;
        if (col.gameObject.tag =="Player")
        {
        	if (impactVelocity > minSpashVelocity)
        	{
        		Vector2 pos = col.contacts[0].point;
            	splash.Play();
        	}
        	
        }

        // spawn new particles
        
    }
}
