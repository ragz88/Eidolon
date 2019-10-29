using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapSwitch : MonoBehaviour
{
    public GameObject[] distSensor;

    Vector3RangerDistanceResponse[] dist;
    Vector3RangerLateralDistanceResponse[] angle;

    int TapCount = 0;
    float waitTime = 0.5f;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        dist = new Vector3RangerDistanceResponse[distSensor.Length];

        for (int i = 0; i < distSensor.Length; i++)
        {
            dist[i] = distSensor[i].GetComponent<Vector3RangerDistanceResponse>();
        }

        angle = new Vector3RangerLateralDistanceResponse[distSensor.Length];

        for (int i = 0; i < distSensor.Length; i++)
        {
            angle[i] = distSensor[i].GetComponent<Vector3RangerLateralDistanceResponse>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SwitchFunctionality();
        }

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
                SwitchFunctionality();
            }
        }

        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            TapCount = 0;
        }
    }

    public void SwitchFunctionality()
    {
        for (int i = 0; i < distSensor.Length; i++)
        {
            dist[i].enabled = !dist[i].enabled;
            angle[i].enabled = !angle[i].enabled;
        }
    }
}
