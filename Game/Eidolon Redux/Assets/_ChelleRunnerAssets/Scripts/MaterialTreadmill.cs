using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTreadmill : MonoBehaviour
{
    public Material treadMat;

    //public float xSpeed = 0;
    public float ySpeed = 0;                 // current y speed

    //public float maxXSpeed = 5;
    public float maxYSpeed = 5;              // maximum speed of Em

    //public float xRampSpeed = 1;
    public float yRampSpeed = 1;             

    public float walkThreshhold = 0.1f;      // Em begins walking after this
    public float jogThreshhold = 2;          // Em begins jogging after this
    public float runThreshhold = 4;          // Em begins running after this

    public EmMovement emMovement;

    
    // Start is called before the first frame update
    void Start()
    {
        treadMat.mainTextureOffset = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // increase speed if it's still smaller than the max speed.
        if (Mathf.Abs(ySpeed) < Mathf.Abs(maxYSpeed))
        {
            ySpeed += yRampSpeed * Time.deltaTime;
        }

        // change anims based on speed
        if (Mathf.Abs(ySpeed) >= runThreshhold)
        {
            emMovement.StartRun();
        }
        else if (Mathf.Abs(ySpeed) >= jogThreshhold)
        {
            emMovement.StartJog();
        }
        else if (Mathf.Abs(ySpeed) >= walkThreshhold)
        {
            emMovement.StartWalk();
        }


        treadMat.mainTextureOffset = new Vector2(treadMat.mainTextureOffset.x /*+ (xSpeed * Time.deltaTime)*/, treadMat.mainTextureOffset.y + (ySpeed * Time.deltaTime));
    }

    public void EmHalfCrash()
    {

    }

    public void EmFullCrash()
    {

    }
}
