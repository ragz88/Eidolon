using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeSpawner : MonoBehaviour
{

    public Button unlockableButton;             // unlocks button after standard mode is complete
    public Text unlockButtonText;
    public GameObject sceneChangerPrefab;       // spawns in a SceneActivator if none is present

    SceneActivator sceneChanger;

    // Start is called before the first frame update
    void Start()
    {
        GameObject tempSceneChanger = GameObject.Find("SceneChanger(Clone)");

        if (tempSceneChanger == null)
        {
            sceneChanger = Instantiate(sceneChangerPrefab).GetComponent<SceneActivator>();
        }
        else
        {
            sceneChanger = tempSceneChanger.GetComponent<SceneActivator>();
        }

        if (sceneChanger.finishedStandard)
        {
            unlockableButton.interactable = true;       // unlocks expert mode
            unlockButtonText.color = Color.white;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartStandardGame()
    {
        sceneChanger.setDifficultyStandard();
    }

    public void StartExpertGame()
    {
        sceneChanger.setDifficultyExpert();
    }


    public void Quit()
    {
        Destroy(sceneChanger);              // prevents this baby from persisting in memory (later this will only quit back to the main game)
        //Application.Quit();
        SceneManager.LoadScene(0);
    }
}
