using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeneralSensorSettings : MonoBehaviour
{

    // used to store values that apply to all 4 sensors - 
    // easing process of balancing and ensuring consistency

    public enum HitQuality
    {
        Perfect,
        Great,
        Good,
        Almost,
        Miss
    }

    public bool missAfterPerfect = false;       // if true, all arrows past the perfect mark are already missed
    

    // Distances, from centre of sensor, for each relative score.
    [Header("Distances")]
    public float perfectDistance = 0.05f;
    public float greatDistance = 0.1f;
    public float goodDistance = 0.15f;
    public float almostDistance = 0.2f;
    public Color missFlashColour;

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

}
