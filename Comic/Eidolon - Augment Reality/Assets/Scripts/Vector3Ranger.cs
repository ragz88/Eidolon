using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Ranger : MonoBehaviour
{
    public enum RangerType
    {
        Scale,
        Position,
        EulerAngles
    }

    public RangerType rangeType = RangerType.Scale;

    [Range(0,1)]
    public float currentPercent = 0;

    public Vector3 initVector;
    public Vector3 finVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rangeType == RangerType.Scale)
        {
            gameObject.transform.localScale = Vector3.Lerp(initVector, finVector, currentPercent);
        }
        else if (rangeType == RangerType.Position)
        {
            gameObject.transform.localPosition = Vector3.Lerp(initVector, finVector, currentPercent);
        }
        else if(rangeType == RangerType.EulerAngles)
        {
            gameObject.transform.localEulerAngles = Vector3.Lerp(initVector, finVector, currentPercent);
        }
    }
}
