using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinFireLight : MonoBehaviour
{

    Light objLight;

    public float maxRange = 3;
    public float lightSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        objLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objLight.range < maxRange)
        {
            objLight.range += (lightSpeed * Time.deltaTime);
        }
    }
}
