using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichelleMovement : MonoBehaviour
{

    public Transform clarenceTrans;

    public float hoverDist = 2;
    public float hoverSpeed = 2;

    public Transform[] movePoints;      // these markers compare to clarence's x position
    public Transform[] subMovePoints;   // these represent the points Chelle will move to

    int currentForwardIndex = 1;
    int currentBackwardIndex = 0;

    public bool overridden = false;
    public Transform overideTrans;
    public float overrideSpeed = 2f;

    bool hoverRising = false;
    float hoverYOffset = 0;

    SpriteRenderer spriteRend;

    public bool chelleInitialPause = false;
    public float initialFloatSpeed = 2;
    bool beginChelleMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        subMovePoints = new Transform[movePoints.Length];

        for (int i = 0; i < movePoints.Length; i++)
        {
            subMovePoints[i] = movePoints[i].Find("SubMovePoint");
        }

        spriteRend = GetComponent<SpriteRenderer>();

        beginChelleMovement = !chelleInitialPause;
    }

    // Update is called once per frame
    void Update()
    {
        // temporary solution - later we'll examine direction of flight and use all the animations.
        if (clarenceTrans.position.x < transform.position.x)
        {
            spriteRend.flipX = true;
        }
        else
        {
            spriteRend.flipX = false;
        }

        if (chelleInitialPause)
        {
            if (beginChelleMovement)
            {
                hoverYOffset = 0;

                
                for (int i = 0; i < movePoints.Length; i++)
                {
                    if (clarenceTrans.transform.position.x < movePoints[i].position.x) // we found the first position that we've yet to reach
                    {
                        currentForwardIndex = i;
                        currentBackwardIndex = i - 1;
                        break;
                    }
                }

                float currentXPercent = Mathf.Abs(clarenceTrans.transform.position.x - movePoints[currentBackwardIndex].position.x) / Mathf.Abs(movePoints[currentForwardIndex].position.x - movePoints[currentBackwardIndex].position.x);

                Vector3 newPosition = Vector3.Lerp(new Vector3(subMovePoints[currentBackwardIndex].position.x, subMovePoints[currentBackwardIndex].position.y, transform.position.z),
                    new Vector3(subMovePoints[currentForwardIndex].position.x, subMovePoints[currentForwardIndex].position.y, transform.position.z), currentXPercent);

                transform.position = Vector3.Lerp(transform.position, newPosition, initialFloatSpeed * Time.deltaTime);

                if (Mathf.Abs(transform.position.x - newPosition.x) < 0.05f &&
                    Mathf.Abs(transform.position.y - newPosition.y) < 0.05f)
                {
                    chelleInitialPause = false;
                }
            }
        }
        else
        {

            if (hoverRising)
            {
                hoverYOffset += hoverSpeed * Time.deltaTime;

                if (hoverYOffset > hoverDist)
                {
                    hoverRising = false;
                }
            }
            else
            {
                hoverYOffset -= hoverSpeed * Time.deltaTime;

                if (hoverYOffset < -hoverDist)
                {
                    hoverRising = false;
                }
            }

            //if (!overridden)
            //{
            for (int i = 0; i < movePoints.Length; i++)
            {
                if (clarenceTrans.transform.position.x < movePoints[i].position.x) // we found the first position that we've yet to reach
                {
                    currentForwardIndex = i;
                    currentBackwardIndex = i - 1;
                    break;
                }
            }

            //float backYDifference = clarenceTrans.transform.position.y - movePoints[currentBackwardIndex].position.y;
            //float frontYDifference = clarenceTrans.transform.position.y - movePoints[currentForwardIndex].position.y;

            // this is how far chelle is between the two points surrounding her - percentage wise.
            float currentXPercent = Mathf.Abs(clarenceTrans.transform.position.x - movePoints[currentBackwardIndex].position.x) / Mathf.Abs(movePoints[currentForwardIndex].position.x - movePoints[currentBackwardIndex].position.x);



            transform.position = Vector3.Lerp(new Vector3(subMovePoints[currentBackwardIndex].position.x, subMovePoints[currentBackwardIndex].position.y, transform.position.z),
                new Vector3(subMovePoints[currentForwardIndex].position.x, subMovePoints[currentForwardIndex].position.y, transform.position.z), currentXPercent);
            /*}
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
            }*/
        }
    }

    public void StartChelleMovement()
    {
        beginChelleMovement = true;
    }
}
