using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameOver : MonoBehaviour
{
    public static GameOver sharedInstance;

    public Text time2;
    public Text time3;
    public Text laptop2;
    public Text laptop3;
    public Text notes2;
    public Text notes3;
    public Text finalScore2;

    public InputField inputField;
    public Button saveScoreButton;

    public float playerScore;
    public Dictionary<string, float> scores = new Dictionary<string, float>();

    private void Awake()
    {
        float timeValue = GameTime.sharedInstance.time;
        time2.text = GameTime.sharedInstance.text.text;
        time3.text = (Mathf.FloorToInt(6000 - timeValue * 10)).ToString();

        float laptopValue = LaptopUI.sharedInstance.slider.value;
        laptop2.text = laptopValue.ToString();
        laptop3.text = (laptopValue * 100).ToString();

        float notesValue = ItemsUI.sharedInstance.notesUses;
        notes2.text = notesValue.ToString();
        notes3.text = (notesValue * 500).ToString();

        playerScore = Mathf.FloorToInt(6000 - timeValue * 10) + (laptopValue * 100) + (notesValue * 500);
        finalScore2.text = playerScore.ToString();
    }

    public void newgame()
    {
        SceneManager.LoadScene("Game");
    }

    public void menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void nameEntered()
    {
        saveScoreButton.interactable = true;
    }

    public void saveScore()
    {
        scores = loadData();
        if (scores.ContainsKey(inputField.textComponent.text))
        {
            
            if(scores[inputField.textComponent.text] > playerScore)
            {
                inputField.text = "You already have a higher score saved with that name";
            } 
            else
            {
                scores.Remove(inputField.textComponent.text);
                scores.Add(inputField.textComponent.text, playerScore);
                saveData();
                inputField.text = "Your score has been saved";
            }
        }
        else
        {
            scores.Add(inputField.textComponent.text, playerScore);
            saveData();
            inputField.text = "Your score has been saved";
        }
    }

    public void saveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, scores);
        stream.Close();
    }

    public Dictionary<string, float> loadData()
    {
        Dictionary<string, float> res = new Dictionary<string, float>();
        string path = Application.persistentDataPath + "/data.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            res = (Dictionary<string, float>) formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }

        return res;
    }
}
