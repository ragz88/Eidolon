using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;

    public Button resumeButton;

    public GameObject pauseUI;

    public AudioSource ddrSource;

    /*public Color buttonHoverColourMain;
    public Color buttonPressedColourMain;
    public Color textMain;
    public Font mainFont;

    public Color buttonHoverColourClarence;
    public Color buttonPressedColourClarence;
    public Color textClarence;
    public Font ClarenceFont;

    public Color buttonHoverColourMom;
    public Color buttonPressedColourMom;
    public Color textMom;
    public Font MomFont;

    public Color buttonHoverColourChelle;
    public Color buttonPressedColourChelle;
    public Color textChelle;
    public Font ChelleFont;*/


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("GamePause"))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        gamePaused = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1f;

        if (ddrSource != null)
        {
            ddrSource.Play();
        }
    }

    void Pause()
    {
        gamePaused = true;
        resumeButton.Select();
        resumeButton.OnSelect(null);
        pauseUI.SetActive(true);
        if (ddrSource != null)
        {
            ddrSource.Pause();
        }
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(0);
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 3  &&  SceneManager.GetActiveScene().buildIndex <= 7)
        {
            SceneManager.LoadScene(2);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 9)
        {
            SceneManager.LoadScene(8);
        }
        else
        {
            SceneManager.LoadScene(1);
        }

        
    }
}
