using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
    //This is a temporary script for the demo's purpose only
    public GameObject ender;
    public int sceneNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ender)
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }
}
