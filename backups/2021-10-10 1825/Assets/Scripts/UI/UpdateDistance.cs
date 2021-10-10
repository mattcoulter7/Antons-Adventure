using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateDistance : MonoBehaviour
{
    public Distance distance;
    private TextMeshProUGUI _text;
    void Start(){
        _text = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        float dist = distance.distance;
        dist = Mathf.Round(dist);
        _text.SetText(dist.ToString());
    }
}
