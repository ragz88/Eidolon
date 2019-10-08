using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichelleMovement : MonoBehaviour
{

    //public Transform chelleSprite;

    //public float hoverDist = 2;
    //public float hoverSpeed = 2;

    public Transform[] movePoints;  // these markers allow Chelle to move in a way that prevents collision with tiles

    int currentForwardIndex = 1;
    int currentBackwardIndex = 0;

    public bool overridden = false;
    public Transform overideTrans;
    public float overrideSpeed = 2f;

    //bool hoverRising = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!overridden)
        {
            for (int i = 0; i < movePoints.Length; i++)
            {
                if (transform.position.x < movePoints[i].position.x) // we found the first position that we've yet to reach
                {
                    currentForwardIndex = i;
                    currentBackwardIndex = i - 1;
                    break;
                }
            }

            float backYDifference = transform.position.y - movePoints[currentBackwardIndex].position.y;
            float frontYDifference = transform.position.y - movePoints[currentForwardIndex].position.y;

            // this is how far chelle is between the two points surrounding her - percentage wise.
            float currentXPercent = Mathf.Abs(transform.position.x - movePoints[currentBackwardIndex].position.x) / Mathf.Abs(movePoints[currentForwardIndex].position.x - movePoints[currentBackwardIndex].position.x);



            transform.position = Vector3.Lerp(new Vector3(transform.position.x, movePoints[currentBackwardIndex].position.y, transform.position.z),
                new Vector3(transform.position.x, movePoints[currentForwardIndex].position.y, transform.position.z), currentXPercent);
        }
        else
        {
            float backYDifference = transform.position.y - movePoints[currentBackwardIndex].position.y;
            float frontYDifference = transform.position.y - overideTrans.position.y;

            // this is how far chelle is between the two points surrounding her - percentage wise.
            //float currentXPercent = Mathf.Abs(transform.position.x - movePoints[currentBackwardIndex].position.x) / Mathf.Abs(overideTrans.position.x - movePoints[currentBackwardIndex].position.x);



            transform.position = Vector3.Lerp(transform.position,
                new Vector3(overideTrans.position.x, overideTrans.position.y, transform.position.z), overrideSpeed * Time.deltaTime);

            //transform.position = Vector3.Lerp(new Vector3(transform.position.x, movePoints[currentBackwardIndex].position.y, transform.position.z),
            //    new Vector3(transform.position.x, overideTrans.position.y, transform.position.z), overrideSpeed * Time.deltaTime);
        }
    }
}
