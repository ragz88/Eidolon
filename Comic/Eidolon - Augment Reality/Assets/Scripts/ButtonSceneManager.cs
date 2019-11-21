using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneManager : MonoBehaviour
{

    public GameObject fadeOutObj;
    public FadeOut fadeOut;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 3)
        {
            LoadScene(1);
        }
    }

    public void LoadScene(int nextScene)
    {
        fadeOutObj.SetActive(true);
        fadeOut.sceneToLoad = nextScene;
        fadeOut.loadNext = false;
    }

    public void LeaveApplication()
    {
        Application.Quit();
    }
}
