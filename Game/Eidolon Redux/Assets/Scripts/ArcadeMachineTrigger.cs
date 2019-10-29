using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeMachineTrigger : MonoBehaviour
{
    CamLerper camLerper;

    public CamLerper.CamState targetCamState;

    private void Start()
    {
        camLerper = GameObject.Find("CamLerpController").GetComponent<CamLerper>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            camLerper.camState = targetCamState;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            camLerper.camState = CamLerper.CamState.Standard;
        }
        
    }
}
