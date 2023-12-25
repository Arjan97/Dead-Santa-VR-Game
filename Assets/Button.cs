using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Start Game");
        SceneManager.LoadScene("DemoScene");
    }

    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
