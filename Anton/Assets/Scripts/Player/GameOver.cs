using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text distanceText;
    public Distance distance;

    public void Setup() 
    {
        float dist = distance.distance;
        gameObject.SetActive(true);
        distanceText.text = "Distance Travelled: " + dist.ToString();
    }

    public void RestartButton() 
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
