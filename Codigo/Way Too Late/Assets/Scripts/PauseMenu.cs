using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu sharedInstance;
    public Button resumeButton;
    public Text resumeText;
    public Button newGameButton;
    public Text newGameText;
    public Button returnButton;
    public Text returnText;

    public bool isActive;

    private void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {
        resumeButton.enabled = false;
        resumeButton.image.enabled = false;
        resumeText.enabled = false;

        newGameButton.image.enabled = false;
        newGameButton.enabled = false;
        newGameText.enabled = false;

        returnButton.image.enabled = false;
        returnButton.enabled = false;
        returnText.enabled = false;

        isActive = false;
    }


    void Update()
    {
        if (Player.sharedInstance.playerInput.actions.FindAction("Pause").triggered)
        {
            resume();
        }
    }

    public void resume()
    {
        resumeButton.enabled = !isActive;
        resumeButton.image.enabled = !isActive;
        resumeText.enabled = !isActive;

        newGameButton.image.enabled = !isActive;
        newGameButton.enabled = !isActive;
        newGameText.enabled = !isActive;

        returnButton.image.enabled = !isActive;
        returnButton.enabled = !isActive;
        returnText.enabled = !isActive;

        Time.timeScale = !isActive ? 0 : 1;

        isActive = !isActive;
    }

    public void newGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
}
