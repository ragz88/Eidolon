using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3RelativeRanger : MonoBehaviour
{
    public enum RangerType
    {
        Scale,
        Position
    }

    public enum RangerAxis
    {
        xAxis,
        yAxis,
        zAxis
    }

    public RangerAxis axis;

    public RangerType rangeType = RangerType.Scale;

    [Range(0,1)]
    public float currentPercent = 0;

    public float initVal;                    // This ranger works differently - it adds a specific value to a defined axis (x or y)
    public float finVal;

    Vector3 initPos;
    Vector3 initScale;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        initScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        switch (axis)
        {
            case RangerAxis.xAxis:
                if (rangeType == RangerType.Scale)
                {
                    transform.localScale = Vector3.Lerp(initScale + new Vector3(initVal, 0, 0), initScale + new Vector3(finVal, 0, 0), currentPercent);
                }
                else if (rangeType == RangerType.Position)
                {
                    transform.localPosition = Vector3.Lerp(initPos + new Vector3(initVal, 0, 0), initPos + new Vector3(finVal, 0, 0), currentPercent);
                }
                break;

            case RangerAxis.yAxis:
                if (rangeType == RangerType.Scale)
                {
                    transform.localScale = Vector3.Lerp(initScale + new Vector3(0, initVal, 0), initScale + new Vector3(0, finVal, 0), currentPercent);
                }
                else if (rangeType == RangerType.Position)
                {
                    transform.localPosition = Vector3.Lerp(initPos + new Vector3(0, initVal, 0), initPos + new Vector3(0, finVal, 0), currentPercent);
                }
                break;

            case RangerAxis.zAxis:
                if (rangeType == RangerType.Scale)
                {
                    transform.localScale = Vector3.Lerp(initScale + new Vector3(0, 0, initVal), initScale + new Vector3(0, 0, finVal), currentPercent);
                }
                else if (rangeType == RangerType.Position)
                {
                    transform.localPosition = Vector3.Lerp(initPos + new Vector3(0, 0, initVal), initPos + new Vector3(0, 0, finVal), currentPercent);
                }
                break;
        }




        
    }
}
