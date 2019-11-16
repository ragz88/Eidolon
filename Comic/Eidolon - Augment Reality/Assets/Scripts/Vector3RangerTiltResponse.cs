using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3RangerTiltResponse: MonoBehaviour
{

    [Range(0,1)]
    public float currentValue = 0;

    public float maxAccValue = 0.6f;
    public float minAccValue = -0.6f;

    public bool lockX = false;
    public bool lockY = false;

    public Vector3Ranger[] rangers;
    public Vector3RelativeRanger[] relRangers;

    float newCurrentPercent = 0;
    public float smoothingSpeed = 1;

    //public Transform pageTrans;

    //public Transform ARCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //float currentXAngle = pageTrans.eulerAngles.x;
        //float currentYAngle = pageTrans.eulerAngles.y;

        float currentXAngle = Input.acceleration.x;
        float currentYAngle = Input.acceleration.y;
        

        if (!lockX && !lockY)
        {
            currentXAngle = Mathf.Clamp(currentXAngle, minAccValue, maxAccValue);
            currentYAngle = Mathf.Clamp(currentYAngle, minAccValue, maxAccValue);
        }
        else
        {
            if (lockY)
            {
                currentXAngle = Mathf.Clamp(currentXAngle, minAccValue, maxAccValue);
                currentYAngle = 0;
            }
            else if (lockX)
            {
                currentYAngle = Mathf.Clamp(currentYAngle, minAccValue, maxAccValue);
                currentXAngle = 0;
            }

        }

        print("X: " + currentXAngle);
        print("Y: " + currentYAngle);

        if ((currentXAngle*currentXAngle) > (currentYAngle*currentYAngle))    // compares magnitude of angle without using any sqrt functions
        {
            currentValue = Mathf.Clamp( (currentXAngle - minAccValue) / (maxAccValue - minAccValue) , 0f, 1f);
        }
        else
        {
            currentValue = Mathf.Clamp((currentYAngle - minAccValue) / (maxAccValue - minAccValue), 0f, 1f);
        }

        newCurrentPercent = Mathf.Lerp(newCurrentPercent, currentValue, smoothingSpeed * Time.deltaTime);
        

        for (int i = 0; i < rangers.Length; i++)
        {
            rangers[i].currentPercent = newCurrentPercent;
        }

        for (int i = 0; i < relRangers.Length; i++)
        {
            relRangers[i].currentPercent = newCurrentPercent;
        }

        //print(Input.acceleration);
    }
}
