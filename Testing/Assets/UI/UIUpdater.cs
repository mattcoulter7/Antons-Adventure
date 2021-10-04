using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdater : MonoBehaviour
{
    public Distance distance;
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        float dist = distance.GetDistance();
        dist = Mathf.Round(dist);
        text.SetText(dist.ToString());
    }
}
