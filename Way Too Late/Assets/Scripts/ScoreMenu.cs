using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ScoreMenu : MonoBehaviour
{
    public Text bestScoreText1;
    public Text bestScoreText2;
    public Text searchPlayer;
    public Text searchScore;

    public Dictionary<string, float> allScores;

    void Start()
    {
        bestScoreText1.text = "";
        bestScoreText2.text = "";

        loadData();
        List<KeyValuePair<string, float>> sortedScores =  (from entry in allScores orderby entry.Value descending select entry).ToList();
        showBestScores(sortedScores);
    }

    public void loadData()
    {
        string path = Application.persistentDataPath + "/data.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            allScores = (Dictionary<string, float>)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }

    }

    void showBestScores(List<KeyValuePair<string, float>> sortedScores)
    {
        int max = sortedScores.Count < 10 ? sortedScores.Count : 10;
        for (int i = 0; i < max; i++)
        {
            bestScoreText1.text += sortedScores.ElementAt(i).Key + ":\r\n\r\n";
            bestScoreText2.text += sortedScores.ElementAt(i).Value + "\r\n\r\n";
        }
    }

    public void searchPlayerScore()
    {
        string name = searchPlayer.text;
        searchScore.text = allScores.ContainsKey(name) ? allScores[name].ToString() : "There is no player registered with that name";
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
