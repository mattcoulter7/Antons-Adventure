using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealOutOfScreen : MonoBehaviour
{
    public GameObject trackObject;
    public GameObject iconPrefab;
    public float iconPadding = 1f; // 1 from top of screen
    public float trackPadding = 1f; // show icon when anton is 1 unit above the screen
    private Camera _camera;
    private GameObject _icon;

    void Start(){
        _camera = GetComponent<Camera>();
        _icon = Instantiate(iconPrefab);
        _icon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float cameraTop = _camera.ViewportToWorldPoint(new Vector3(0,1,0)).y;
        if (trackObject.transform.position.y > cameraTop + trackPadding){
            // update the icon position
            Vector3 newPosition = _icon.transform.position;
            newPosition.y = cameraTop - iconPadding;
            newPosition.x = trackObject.transform.position.x;
            // show the icon representing he is above the screen
            _icon.transform.position = newPosition;
            _icon.SetActive(true);
        } else {
            _icon.SetActive(false);
            // hide the icon
        }
    }
}
