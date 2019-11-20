using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    private bool camAvailable;

    private WebCamTexture backCam;


    int TapCount = 0;
    float waitTime = 0.5f;
    float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            print("No Cam");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name);
            }
        }

        if (backCam == null)
        {
            print("noBackCam");
            return;
        }


        camAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                TapCount += 1;
                timer = 0;
            }

            if (TapCount == 2)
            {
                TapCount = 0;
                FocusCam();
            }
        }

        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            TapCount = 0;
        }
    }

    void FocusCam()
    {
        //print("Oh Yeah!");
        backCam.autoFocusPoint = new Vector2(0.5f, 0.5f);
    }
}
