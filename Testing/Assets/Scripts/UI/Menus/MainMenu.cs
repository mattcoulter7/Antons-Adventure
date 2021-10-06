using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject options;
    public void OnPlay(){
        SceneManager.LoadScene("Gameplay");
    }

    public void OnOptions(){
        options.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnExit(){
        Application.Quit();
    }
}
