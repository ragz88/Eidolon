using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip[] clips;

    int currentClip = 0;

    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (source.isPlaying == false)
        {
            currentClip++;
            source.clip = clips[currentClip];
            source.Play();
        }
    }
}
