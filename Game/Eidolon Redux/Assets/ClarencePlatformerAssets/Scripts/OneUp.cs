using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUp : MonoBehaviour
{

    public float hoverDistance = 0.5f;    // max distance from midpoint when hovering
    public float hoverSpeed = 2;

    //public GameObject idleParts;          // Particles that play when hovering
    public GameObject pickupPartsPrefab;        // Particles that play when the player picks this up

    //bool beenPickedUp = false;

    bool rising = true;
    Vector3 initPos;

    public bool linear = false;

    float currentPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (rising)
        {
            if (linear)
            {
                if (currentPos < 1)
                {
                    transform.position = Vector2.Lerp(initPos + new Vector3(0, -hoverDistance, 0), initPos + new Vector3(0, hoverDistance, 0), currentPos);
                    currentPos += hoverSpeed * Time.deltaTime;
                }
                else
                {
                    rising = false;
                }
            }
            else
            {
                if (Mathf.Abs(transform.position.y - (initPos.y + hoverDistance)) > 0.05f)
                {
                    transform.position = Vector2.Lerp(transform.position, initPos + new Vector3(0, hoverDistance,0), hoverSpeed * Time.deltaTime);
                }
                else
                {
                    rising = false;
                }
            }
        }
        else
        {
            if (linear)
            {
                if (currentPos > 0)
                {
                    transform.position = Vector2.Lerp(initPos + new Vector3(0, -hoverDistance, 0), initPos + new Vector3(0, hoverDistance, 0), currentPos);
                    currentPos -= hoverSpeed * Time.deltaTime;
                }
                else
                {
                    rising = true;
                }
            }
            else
            {
                if (Mathf.Abs(transform.position.y - (initPos.y - hoverDistance)) > 0.05f)
                {
                    transform.position = Vector2.Lerp(transform.position, initPos + new Vector3(0, -hoverDistance, 0), hoverSpeed * Time.deltaTime);
                }
                else
                {
                    rising = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && ClarenceGameController.health < 3)
        {
            ClarenceGameController.instance.GainLife();
            //beenPickedUp = true;
            Instantiate(pickupPartsPrefab,transform.position + new Vector3(0,0,-2f), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
