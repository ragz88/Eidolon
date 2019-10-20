using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibratePattern : MonoBehaviour
{

    public long[] vibrationPattern;

    bool isVibrating = false;

    float timer = 0;
    float totalTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < vibrationPattern.Length; i++)
        {
            totalTime += vibrationPattern[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isVibrating)
        {
            Vibration.Vibrate(vibrationPattern, 1);
            isVibrating = true;
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > totalTime)
            {
                isVibrating = false;
            }
        }
        
    }

    public void ResetVibrator()
    {
        Vibration.Vibrate(vibrationPattern, 1);
        isVibrating = true;
        timer = 0;
    }
}
