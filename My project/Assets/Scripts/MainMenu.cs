using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene 1");
    }

    public void Records()
    {
        SceneManager.LoadScene("Scores");
    }

    public void ExitGame()
    {
        Debug.Log("Game is quit!");
        Application.Quit();
    }
}
