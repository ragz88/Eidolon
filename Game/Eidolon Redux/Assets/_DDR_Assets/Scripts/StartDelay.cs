using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDelay : MonoBehaviour
{

    //public Text text;
    public float delayTime = 3;

    public AudioSource source;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > delayTime)
        {
            source.Play();
            Destroy(gameObject);
        }
    }
}
