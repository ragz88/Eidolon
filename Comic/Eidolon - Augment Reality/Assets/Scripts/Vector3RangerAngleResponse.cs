using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3RangerAngleResponse : MonoBehaviour
{

    [Range(0,1)]
    public float currentValue = 0;

    public float maxAngle = 30;
    public float minAngle = -30;

    public bool lockX = false;
    public bool lockY = false;

    public Vector3Ranger[] rangers;

    public Transform pageTrans;

    //public Transform ARCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        float currentXAngle = pageTrans.eulerAngles.x;
        float currentYAngle = pageTrans.eulerAngles.y;

        // ensure angle is in workable range of -180 to 180
        if (currentXAngle > 180)
        {
            while(currentXAngle > 180)
            {
                currentXAngle -= 360;
            }
        }
        else if (currentXAngle < -180)
        {
            while (currentXAngle < -180)
            {
                currentXAngle += 360;
            }
        }

        if (currentYAngle > 180)
        {
            while (currentYAngle > 180)
            {
                currentYAngle -= 360;
            }
        }
        else if (currentYAngle < -180)
        {
            while (currentYAngle < -180)
            {
                currentYAngle += 360;
            }
        }

        print("Page: " + currentYAngle);

        if (!lockX && !lockY)
        {
            currentXAngle = Mathf.Clamp(currentXAngle, minAngle, maxAngle);
            currentYAngle = Mathf.Clamp(currentYAngle, minAngle, maxAngle);
        }
        else
        {
            if (lockX)
            {
                currentXAngle = Mathf.Clamp(currentXAngle, minAngle, maxAngle);
                currentYAngle = 0;
            }
            else if (lockY)
            {
                currentYAngle = Mathf.Clamp(currentYAngle, minAngle, maxAngle);
                currentXAngle = 0;
            }

        }

        print("X: " + currentXAngle);
        print("Y: " + currentYAngle);

        if ((currentXAngle*currentXAngle) > (currentYAngle*currentYAngle))    // compares magnitude of angle without using any sqrt functions
        {
            currentValue = Mathf.Clamp( (currentXAngle - minAngle) / (maxAngle - minAngle) , 0f, 1f);
        }
        else
        {
            currentValue = Mathf.Clamp((currentYAngle - minAngle) / (maxAngle - minAngle), 0f, 1f);
        }
        

        

        for (int i = 0; i < rangers.Length; i++)
        {
            rangers[i].currentPercent = currentValue;
        }
    }
}
