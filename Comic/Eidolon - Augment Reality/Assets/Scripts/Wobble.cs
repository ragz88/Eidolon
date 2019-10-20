using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    public float speed;
    public float maxDist = 1f;

    public bool lockX;
    public bool lockY;

    Vector2 finPos;

    Vector2 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.localPosition;
        finPos = initPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.localPosition.x - finPos.x) <= 0.01f && Mathf.Abs(transform.localPosition.y - finPos.y) <= 0.01f)
        {
            if (!lockX && !lockY)
            {
                finPos = Random.insideUnitCircle * maxDist;

            }
            else
            {
                if (lockX)
                {
                    finPos = new Vector2(0, Random.Range(-1,1)) * maxDist;
                }

                if (lockY)
                {
                    finPos = new Vector2(Random.Range(-1, 1), 0) * maxDist;
                }
            }

            finPos = finPos + initPos;
            
        }
        else
        {
            //print(finPos);
            //transform.localPosition = Vector2.Lerp(transform.localPosition, finPos, speed * Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(finPos.x, finPos.y, transform.localPosition.z), speed * Time.deltaTime);
        }
        
    }
}
