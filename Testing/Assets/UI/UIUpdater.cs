using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdater : MonoBehaviour
{
    public Distance distance;
    public TextMesh text;

    // Update is called once per frame
    void Update()
    {
        text.text = distance.GetDistance().ToString();
    }
}
