using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void start()
    {
        SceneManager.LoadScene("Game");
    }

    public void startDemo()
    {
        SceneManager.LoadScene("GameDemo");
    }

    public void score()
    {
        SceneManager.LoadScene("ScoreMenu");
    }

    public void controls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void exit()
    {
        Application.Quit();
    }

}
