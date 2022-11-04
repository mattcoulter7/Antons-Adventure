using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeControl : MonoBehaviour
{
	[Range(0.0001f, 1.0f)]
	[SerializeField]
    
    private float masterVol =  1.0f;

    void Update(){
    	AudioListener.volume = masterVol;
    }
}
