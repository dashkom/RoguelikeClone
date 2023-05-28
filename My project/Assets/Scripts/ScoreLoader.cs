using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreLoader : MonoBehaviour
{
    [SerializeField] Text score;
    private void Start()
    {
        List<GameProgressData.Records> levels = GameProgressData.Records.GetRecords(Path.Combine(Application.dataPath, "data.dat"));
        string recText = "";

        for (int i = 0; i < levels.Count; i++)
        {
            recText += $"{(i + 1).ToString()}. Достигнут уровень {levels[i].Level}\n";
        }
        score.text = recText;
    }
    public void ClearRecords()
    {
        GameProgressData.Records.ClearRecords(Path.Combine(Application.dataPath, "data.dat"));
        Start();
    }

}
