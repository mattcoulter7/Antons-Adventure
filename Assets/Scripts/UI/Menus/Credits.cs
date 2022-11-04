using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainMenu;
    public void OnBack(){
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
