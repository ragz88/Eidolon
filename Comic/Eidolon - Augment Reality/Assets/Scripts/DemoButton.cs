using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoButton : MonoBehaviour
{
    public GameObject[] distSensor;

    Vector3RangerDistanceResponse[] dist;
    Vector3RangerAngleResponse[] angle;

    // Start is called before the first frame update
    void Start()
    {
        dist = new Vector3RangerDistanceResponse[distSensor.Length];

        for (int i = 0; i < distSensor.Length; i++)
        {
            dist[i] = distSensor[i].GetComponent<Vector3RangerDistanceResponse>();
        }

        angle = new Vector3RangerAngleResponse[distSensor.Length];

        for (int i = 0; i < distSensor.Length; i++)
        {
            angle[i] = distSensor[i].GetComponent<Vector3RangerAngleResponse>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
