using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeMachineTrigger : MonoBehaviour
{
    CamLerper camLerper;

    public CamLerper.CamState targetCamState;

    public GameObject tutButton;

    private void Start()
    {
        camLerper = GameObject.Find("CamLerpController").GetComponent<CamLerper>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            tutButton.SetActive(true);

            if (Input.GetButtonDown("ActivateMachine"))
            {
                camLerper.camState = targetCamState;
                Destroy(tutButton.gameObject);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //camLerper.camState = CamLerper.CamState.Standard;
            tutButton.SetActive(false);
        }
        
    }
}
