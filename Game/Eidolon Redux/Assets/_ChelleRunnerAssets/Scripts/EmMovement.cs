using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmMovement : MonoBehaviour
{

    public enum Lane
    {
        Left,
        Middle,
        Right
    }

    public Lane currentLane = Lane.Middle;

    public Transform[] laneMarkers;   // marks the centre for each lane

    Transform currentTarget;

    public float laneShiftSpeed = 3;

    public Animator emAnim;
    public SpriteRenderer spriteRend;


    // Start is called before the first frame update
    void Start()
    {
        currentTarget = laneMarkers[(int)currentLane];
    }

    // Update is called once per frame
    void Update()
    {
        float movement = Input.GetAxisRaw("Horizontal");

        if (movement < 0)    // left move
        {
            if (currentLane != Lane.Left)
            {
                if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.05f)
                {
                    currentLane--;
                    currentTarget = laneMarkers[(int)currentLane];
                    emAnim.SetBool("switchLane", true);
                    spriteRend.flipX = false;
                }
            }
        }
        else if(movement > 0) // right move
        {
            if (currentLane != Lane.Right)
            {
                if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.05f)
                {
                    currentLane++;
                    currentTarget = laneMarkers[(int)currentLane];
                    emAnim.SetBool("switchLane", true);
                    spriteRend.flipX = true;
                }
            }
        }

        if (Mathf.Abs(transform.position.x - currentTarget.position.x) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, laneShiftSpeed * Time.deltaTime);
        }
        else
        {
            emAnim.SetBool("switchLane", false);
            spriteRend.flipX = false;
        }


    }
}
