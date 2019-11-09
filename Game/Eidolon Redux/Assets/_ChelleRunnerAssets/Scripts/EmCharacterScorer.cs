using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmCharacterScorer : MonoBehaviour
{
    public RunnerScoreManager scoreMan;


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
        if (other.tag == "Firefly")
        {
            scoreMan.FlyCaught();
            other.GetComponentInChildren<FireflyControl>().Disappear();
        }
    }
}
