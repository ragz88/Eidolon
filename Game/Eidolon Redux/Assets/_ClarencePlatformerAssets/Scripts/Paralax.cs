using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    Transform clarenceTrans;

    public float paralaxMultiplier = 0;
    public float lerpSpeed = 1f;
    Vector3 initPos;

    public bool useMaxAndMin = false;

    public float MaxMovement = 0;
    public float MinMovement = 0;

    // Start is called before the first frame update
    void Start()
    {
        clarenceTrans = GameObject.Find("Clarence").transform;
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newX = ((clarenceTrans.position.x - initPos.x) * paralaxMultiplier) + initPos.x;

        if (useMaxAndMin)
        {
            if (newX < MaxMovement && newX > MinMovement)
            {
                //transform.position = Vector3.Lerp(transform.position, initPos + new Vector3(newX, 0, 0), lerpSpeed * Time.deltaTime);
                transform.position = initPos + new Vector3(newX, 0, 0);
            }
        }
        else
        {
            //transform.position = Vector3.Lerp(transform.position, initPos + new Vector3(newX, 0, 0), lerpSpeed * Time.deltaTime);
            transform.position = initPos + new Vector3(newX, 0, 0);
        }
    }
}
