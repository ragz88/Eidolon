using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpritePulse : MonoBehaviour
{

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public Direction direction;


    //public Material arrowMat;
    //public Material barMat;

    private AudioSyncValue audioSyncVal;


    // Start is called before the first frame update
    void Start()
    {
        audioSyncVal = GetComponent<AudioSyncValue>();

        switch(direction)
        {
            case Direction.Up:
                audioSyncVal = GameObject.Find("UpSensor").GetComponent<AudioSyncValue>();
                break;
            case Direction.Down:
                audioSyncVal = GameObject.Find("DownSensor").GetComponent<AudioSyncValue>();
                break;
            case Direction.Left:
                audioSyncVal = GameObject.Find("LeftSensor").GetComponent<AudioSyncValue>();
                break;
            case Direction.Right:
                audioSyncVal = GameObject.Find("RightSensor").GetComponent<AudioSyncValue>();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentBarLevel = 0;

        if (audioSyncVal.currentVal > 0.9f)
        {
            currentBarLevel = 0.9f;
        }
        else
        {
            currentBarLevel = audioSyncVal.currentVal;
        }

        transform.localScale = new Vector3(currentBarLevel, currentBarLevel, currentBarLevel);
    }
}
