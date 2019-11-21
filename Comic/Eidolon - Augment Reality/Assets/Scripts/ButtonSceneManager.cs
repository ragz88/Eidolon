using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSceneManager : MonoBehaviour
{

    public GameObject fadeOutObj;
    public FadeOut fadeOut;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
