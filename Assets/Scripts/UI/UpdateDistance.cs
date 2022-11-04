using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateDistance : MonoBehaviour
{
    public Distance distance;
    private Text _text;
    void Start(){
        _text = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        float dist = distance.distance;
        dist = Mathf.Round(dist);
        _text.text = (dist.ToString());
    }
}
