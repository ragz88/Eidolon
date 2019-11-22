using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneActivator : MonoBehaviour
{
    public DifficultySettings.DifficultyLevel difficulty;

    public static bool finishedStandard = false;        // this is used to lock the expert level until later
    

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

    }

    // this is the Amilia difficulty
    public void setDifficultyStandard()
    {
        difficulty = DifficultySettings.DifficultyLevel.Standard;
        finishedStandard = true;
        SceneManager.LoadScene("DDR_InitTest");
    }

    // this is the Mother difficulty
    public void setDifficultyExpert()
    {
        difficulty = DifficultySettings.DifficultyLevel.Expert;
        SceneManager.LoadScene("DDR_InitTest");
    }
}
