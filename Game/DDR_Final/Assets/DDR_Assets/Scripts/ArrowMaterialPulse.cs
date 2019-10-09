using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMaterialPulse : MonoBehaviour
{

    public Material arrowMat;
    public Material barMat;

    private AudioSyncValue audioSyncVal;

    // Start is called before the first frame update
    void Start()
    {
        audioSyncVal = GetComponent<AudioSyncValue>();
    }

    // Update is called once per frame
    void Update()
    {
        arrowMat.SetFloat("Vector1_F6BF89FD", audioSyncVal.currentVal);       //the stupid string is an auto-generated ID for our Level shader value

        float currentBarMatLevel = 0;

        if (audioSyncVal.currentVal > 0.9f)
        {
            currentBarMatLevel = 0.9f;
        }
        else
        {
            currentBarMatLevel = audioSyncVal.currentVal;
        }

        barMat.SetFloat("Vector1_F6BF89FD", currentBarMatLevel);
    }
}
