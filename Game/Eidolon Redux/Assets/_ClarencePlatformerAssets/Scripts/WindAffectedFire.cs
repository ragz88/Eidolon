using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAffectedFire : MonoBehaviour
{

    ActivatableFire actFire;

    public float finalFireSize = 0f;
    public float fireSizeChangeSpeed = 10f;

    //public AirCurrent[] airCurrents;
    public AirCurrentChelleResponse[] airCurrents;

    public bool taperEdges = true;
    public float taperFireSizeTier1 = 0.5f;
    public float taperFireSizeTier2 = 0.25f; // hard coded for now

    public float threshholdPercent = 0.6f;  // once wind speed is above this percent of the max, the fire will react.

    public float disappearThreshhold = 0.1f;

    public bool windBlowing = false;
    bool lerping = false;

    float[] initSizes;

    // Start is called before the first frame update
    void Start()
    {
        actFire = GetComponent<ActivatableFire>();

        initSizes = new float[actFire.flames.Length];

        for (int i = 0; i < initSizes.Length; i++)
        {

            initSizes[i] = actFire.heightCurve[i].value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool windCurrentlyPresent = false;      // set to true if a single windzone is blowing on the fire

        

        for (int i = 0; i < airCurrents.Length; i++)
        {
            //float currentWindPercent = (airCurrents[i].currentStrength - airCurrents[i].minStrength) / (airCurrents[i].maxStrength - airCurrents[i].minStrength);

            /*if (currentWindPercent > threshholdPercent)   
            {
                windCurrentlyPresent = true;
            }*/

            if (airCurrents[i].chelleController.windState == MichelleWindController.WindState.BlowingPhase1)
            {
                windCurrentlyPresent = true;
            }
        }

        // detects a change in wind behaviour
        if (windBlowing != windCurrentlyPresent)
        {
            lerping = true;
        }

        windBlowing = windCurrentlyPresent;     // update our persistent bool variable

        if (lerping)
        {
            if (windBlowing)
            {
                for (int i = 0; i < actFire.flames.Length; i++)
                {
                    if (taperEdges)  //hardcoded for now
                    {
                        if (i == 1 || i == actFire.flames.Length - 2)    // second and second last flames
                        {
                            ChangeFlameSize(i, taperFireSizeTier1);
                        }
                        else if (i == 2 || i == actFire.flames.Length - 3)
                        {
                            ChangeFlameSize(i, taperFireSizeTier2);
                        }
                        else
                        {
                            ChangeFlameSize(i, finalFireSize);
                        }
                    }
                    else
                    {
                        ChangeFlameSize(i, finalFireSize);
                    }

                    if (actFire.heightCurve[i].value >= disappearThreshhold)
                    {
                        actFire.flames[i].GetComponent<SpriteRenderer>().enabled = true;
                    }

                    if (Mathf.Abs(actFire.heightCurve[i].value - finalFireSize) <= 0.05f)
                    {
                        lerping = false;
                        if (actFire.heightCurve[i].value < disappearThreshhold)
                        {
                            actFire.flames[i].GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < actFire.flames.Length; i++)
                {
                    ChangeFlameSize(i, initSizes[i]);

                    if (actFire.heightCurve[i].value >= disappearThreshhold)
                    {
                        actFire.flames[i].GetComponent<SpriteRenderer>().enabled = true;
                    }

                    if (Mathf.Abs(actFire.heightCurve[i].value - initSizes[i]) <= 0.05f)
                    {
                        lerping = false;
                        if (actFire.heightCurve[i].value < disappearThreshhold)
                        {
                            actFire.flames[i].GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                }
            }
        }
        
    }

    void ChangeFlameSize(int i, float newSize)
    {
        float newVal = Mathf.Lerp(actFire.heightCurve[i].value, newSize, fireSizeChangeSpeed * Time.deltaTime);

        Keyframe newKey = new Keyframe(actFire.heightCurve[i].time, newVal);
        actFire.heightCurve.RemoveKey(i);
        actFire.heightCurve.AddKey(newKey);
    }
}
