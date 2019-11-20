using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleParallaxTilt : MonoBehaviour
{

    [Range(-1,1)]
    public float currentXValue = 0;

    [Range(-1, 1)]
    public float currentYValue = 0;

    public float maxAccValue = 0.1f;
    public float minAccValue = -0.1f;

    public float maxYAccValue = -0.8f;
    public float minYAccValue = -1f;

    public bool lockX = false;
    public bool lockY = false;
    public bool invertX = false;
    public bool invertY = true;

    public float xRadius;
    public float yRadius;

    float newCurrentXPercent = 0;
    float newCurrentYPercent = 0;

    public float smoothingSpeed = 1;

    Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //float currentXAngle = pageTrans.eulerAngles.x;
        //float currentYAngle = pageTrans.eulerAngles.y;

        float currentXAngle = Input.acceleration.x;
        float currentYAngle = Input.acceleration.y;

        //print(currentYAngle);

        if (!lockX && !lockY)
        {
            currentXAngle = Mathf.Clamp(currentXAngle, minAccValue, maxAccValue);
            currentYAngle = Mathf.Clamp(currentYAngle, minYAccValue, maxYAccValue);
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
                currentYAngle = Mathf.Clamp(currentYAngle, minYAccValue, maxYAccValue);
                currentXAngle = 0;
            }

        }

        

        //print("X: " + currentXAngle);
        //print("Y: " + currentYAngle);


        currentXValue = Mathf.Clamp( (((currentXAngle - minAccValue) / (maxAccValue - minAccValue)) * 2) - 1, -1f, 1f);
        
        currentYValue = Mathf.Clamp( (((currentYAngle - minYAccValue) / (maxYAccValue - minYAccValue)) * 2) - 1, -1f, 1f);

        if (invertX)
        {
            currentXAngle *= -1;
            
        }
        if (invertY)
        {
            currentYAngle += -1;
        }

        Vector3 newPos = initPos + new Vector3(currentXValue * xRadius, 0 , currentYValue * yRadius);

        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, smoothingSpeed * Time.deltaTime);
        
        
    }
}
