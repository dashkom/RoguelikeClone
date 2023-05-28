using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordsMenu : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ClearRecords()
    {
        GameProgressData.Records.ClearRecords(Path.Combine(Application.dataPath, "data.dat"));
    }
}
