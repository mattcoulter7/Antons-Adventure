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
        Time.timeScale = 0;
        float dist = distance.distance;
        gameObject.SetActive(true);
        distanceText.text = "Distance Travelled: " + dist.ToString();
    }

    public void RestartButton() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Gameplay");
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
