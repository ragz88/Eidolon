using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleParalax : MonoBehaviour
{
    public Transform ARCam;
    public Transform anchorPoint;


    public float radius;

    public float maxDist = 2; // this marks how far the camera must be in a specific direction to push the bubble to it's radius

    public bool invertDirection = false;

    public bool lockX = false;
    public bool lockY = false;

    Vector3 initPosition;
    Vector2 finPos;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistX = (anchorPoint.position.x - ARCam.position.x) / maxDist;
        float currentDistY = (anchorPoint.position.y - ARCam.position.y) / maxDist;

        Vector2 directionVector;

        if (lockX)
        {
            currentDistX = 0;
        }

        if (lockY)
        {
            currentDistY = 0;
        }

        if (invertDirection)
        {
            directionVector = new Vector2(-currentDistX, -currentDistY);
        }
        else
        {
            directionVector = new Vector2(currentDistX, currentDistY);
        }

        

        float currentGenDist = Mathf.Clamp(directionVector.magnitude, 0, radius);

        directionVector.Normalize();

        finPos = (directionVector * currentGenDist);

        transform.localPosition = finPos;

        /*Vector3 unitDirectionVector = Vector3.Normalize(transform.position - ARCam.position);

        float currentXDistPercent = Mathf.Clamp((transform.position.x - ARCam.position.x)/maxDist, 0, 1);
        float currentYDistPercent = Mathf.Clamp((transform.position.y - ARCam.position.y)/maxDist, 0, 1);

        float currentXDist = currentXDistPercent * radius;
        float currentYDist = currentYDistPercent * radius;

        finPos =  new Vector3(initPosition.x + (unitDirectionVector.x * currentXDist), initPosition.y + (unitDirectionVector.y * currentYDist), transform.localPosition.z);

        transform.localPosition = finPos;*/
    }
}
