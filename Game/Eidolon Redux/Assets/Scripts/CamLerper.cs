using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonEngine.Runtime;

public class CamLerper : MonoBehaviour
{
    Transform mainCam;

    TPCamera tpCam;

    public enum CamState
    {
        Machine1,
        Machine2,
        Machine3,
        Standard
    }

    public CamState camState = CamState.Standard;

    public Transform[] camPoints;

    public float lerpSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
        tpCam = mainCam.gameObject.GetComponent<TPCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (camState != CamState.Standard)
        {
            mainCam.position = Vector3.Lerp(mainCam.position, camPoints[(int)camState].position, lerpSpeed * Time.deltaTime);
            mainCam.rotation = Quaternion.Slerp(mainCam.rotation, camPoints[(int)camState].rotation, lerpSpeed * Time.deltaTime);
            tpCam.enabled = false;
        }
        else
        {
            tpCam.enabled = true;
        }
    }
}
