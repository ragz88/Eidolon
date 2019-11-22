using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSound : MonoBehaviour
{

    public Transform playerTrans;
    public Transform audioTrans;
    public Transform[] riverPoints;

    public float audioSpeed = 1f;

    int currentRiverPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < riverPoints.Length; i++)
        {
            if (Vector3.Distance(playerTrans.position, riverPoints[currentRiverPoint].position) >
                Vector3.Distance(playerTrans.position, riverPoints[i].position))
            {
                currentRiverPoint = i;
            }
        }

        audioTrans.position = Vector3.MoveTowards(audioTrans.position, riverPoints[currentRiverPoint].position, audioSpeed * Time.deltaTime);
    }
}
