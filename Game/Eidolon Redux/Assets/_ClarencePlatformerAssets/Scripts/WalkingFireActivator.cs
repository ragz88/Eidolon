using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingFireActivator : MonoBehaviour
{

    ActivatableFire actFire;

    public float maxFireSize = 0.25f;
    public float fireIncreaseSpeed = 10f;
    
    public GameObject sensorAreaPrefab;

    WalkSensor[] sensors;
    
    // Start is called before the first frame update
    void Start()
    {
        actFire = GetComponent<ActivatableFire>();

        InitialiseSensors();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < sensors.Length; i++)
        {
            if (!sensors[i].fullyActive && sensors[i].beenEntered)
            {
                float newVal = Mathf.Lerp(actFire.heightCurve[i].value, maxFireSize, fireIncreaseSpeed * Time.deltaTime);

                Keyframe newKey = new Keyframe(actFire.heightCurve[i].time, newVal);
                actFire.heightCurve.RemoveKey(i);
                actFire.heightCurve.AddKey(newKey);
            }
        }
    }

    public void RemoveActiveFlames()
    {
        for (int i = 0; i < sensors.Length; i++)
        {
            sensors[i].beenEntered = false;
            sensors[i].fullyActive = false;

            Keyframe newKey = new Keyframe(actFire.heightCurve[i].time, 0);
            actFire.heightCurve.RemoveKey(i);
            actFire.heightCurve.AddKey(newKey);
        }

        InitialiseSensors();
    }

    public void InitialiseSensors()
    {
        sensors = new WalkSensor[actFire.flames.Length];

        for (int i = 0; i < sensors.Length; i++)
        {
            GameObject tempObj = Instantiate(sensorAreaPrefab, actFire.flames[i].position, Quaternion.identity, transform) as GameObject;

            sensors[i] = tempObj.GetComponent<WalkSensor>();
            sensors[i].GetComponent<BoxCollider2D>().isTrigger = true;
            sensors[i].GetComponent<BoxCollider2D>().size = new Vector2(maxFireSize, maxFireSize);
        }
    }
}
