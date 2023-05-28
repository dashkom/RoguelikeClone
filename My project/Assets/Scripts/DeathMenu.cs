using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene 1");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
