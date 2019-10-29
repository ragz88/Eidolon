﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3RangerLateralDistanceResponse : MonoBehaviour
{

    [Range(0,1)]
    public float currentValue = 0;

    public float maxDistance = 5f;
    public float minDistance = 0.2f;

    public Vector3Ranger[] rangers;

    public Transform ARCam;

    public bool invertDistance = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float currentDist = Vector3.Distance(transform.position, ARCam.position);         // not as optimised
        float currentDist = (transform.position.x - ARCam.position.x) - minDistance;

        /*if (currentDist < 0)
        {
            currentDist *= -1;
        }*/

        print(currentDist);

        currentValue = Mathf.Clamp((currentDist/ (maxDistance - minDistance)) , 0f, 1f);

        if (invertDistance)
        {
            currentValue = 1 - currentValue;
        }



        for (int i = 0; i < rangers.Length; i++)
        {
            rangers[i].currentPercent = currentValue;
        }
    }
}
