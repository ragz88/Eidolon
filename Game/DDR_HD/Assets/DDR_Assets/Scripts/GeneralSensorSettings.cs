using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeneralSensorSettings : MonoBehaviour
{

    // used to store values that apply to all 4 sensors - 
    // easing process of balancing and ensuring consistency

    public enum ScoreQuality
    {
        Perfect,
        Great,
        Good,
        Almost,
        Miss
    }

    public Text keyWordText;                    // text at the centre of the screen
    public bool missAfterPerfect = false;       // if true, all arrows past the perfect mark are already missed
    

    // Distances, from centre of sensor, for each relative score.
    [Header("Distances")]
    public float perfectDistance = 0.05f;
    public float greatDistance = 0.1f;
    public float goodDistance = 0.15f;
    public float almostDistance = 0.2f;
    public Color missFlashColour;

    // Words to be shown depending on timing of press
    // and their colours
    [Header("Key Words")]
    public string perfectString = "Perfect!";
    public Color perfectColour;
    public string greatString = "Great!";
    public Color greatColour;
    public string goodString = "Good";
    public Color goodColour;
    public string almostString = "Almost";
    public Color almostColour;
    public string missString = "BOO!";
    public Color missColour;

    private void Start()
    {
    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /*public void DisplayPerfect()
    {
        keyWordText.text = perfectString;
        keyWordText.color = perfectColour;
    }

    public void DisplayGreat()
    {
        keyWordText.text = greatString;
        keyWordText.color = greatColour;
    }

    public void DisplayGood()
    {
        keyWordText.text = goodString;
        keyWordText.color = goodColour;
    }

    public void DisplayAlmost()
    {
        keyWordText.text = almostString;
        keyWordText.color = almostColour;
    }

    public void DisplayMiss()
    {
        keyWordText.text = missString;
        keyWordText.color = missColour;
    } */

    public void DisplayHitQuality(ScoreQuality scoreType)
    {
        switch(scoreType)
        {
            case ScoreQuality.Perfect:
                keyWordText.text = perfectString;
                keyWordText.color = perfectColour;
                break;

            case ScoreQuality.Great:
                keyWordText.text = greatString;
                keyWordText.color = greatColour;
                break;

            case ScoreQuality.Good:
                keyWordText.text = goodString;
                keyWordText.color = goodColour;
                break;

            case ScoreQuality.Almost:
                keyWordText.text = almostString;
                keyWordText.color = almostColour;
                break;

            case ScoreQuality.Miss:
                keyWordText.text = missString;
                keyWordText.color = missColour;
                break;
        }
    }

}
