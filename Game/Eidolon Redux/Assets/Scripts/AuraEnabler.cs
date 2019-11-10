using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aura2API;

public class AuraEnabler : MonoBehaviour
{
    public AuraLight[] auraLights;
    public AuraCamera[] auraCams;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < auraLights.Length; i++)
        {
            auraLights[i].enabled = true;
        }

        for (int i = 0; i < auraCams.Length; i++)
        {
            auraCams[i].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
