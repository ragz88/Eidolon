using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClarenceGameController : MonoBehaviour
{
    //this static script allows us to store Clarence's health in an easily accessible place.
    public static ClarenceGameController instance = null;

    public static bool[] textTutorialsPlayed;

    public static int health = 3;

    public Animator[] heartAnimators;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;

            
        }
        else if (instance != this)   // implies we already have a manager - and we don't want a duplicate.
        {
            Destroy(gameObject);   
        }

        if (textTutorialsPlayed == null)
        {
            textTutorialsPlayed = new bool[10];

            for (int i = 0; i < textTutorialsPlayed.Length; i++)
            {
                textTutorialsPlayed[i] = false;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckLife()
    {
        switch (health)
        {
            case 3:
                heartAnimators[0].SetBool("HeartFull", true);
                heartAnimators[1].SetBool("HeartFull", true);
                heartAnimators[2].SetBool("HeartFull", true);
                break;

            case 2:
                heartAnimators[0].SetBool("HeartFull", true);
                heartAnimators[1].SetBool("HeartFull", true);
                heartAnimators[2].SetBool("HeartFull", false);
                break;

            case 1:
                heartAnimators[0].SetBool("HeartFull", true);
                heartAnimators[1].SetBool("HeartFull", false);
                heartAnimators[2].SetBool("HeartFull", false);
                break;

            case 0:
                heartAnimators[0].SetBool("HeartFull", false);
                heartAnimators[1].SetBool("HeartFull", false);
                heartAnimators[2].SetBool("HeartFull", false);
                break;
        }
    }

    public void LoseLife()
    {
        health--;

        CheckLife();
    }

    public void GainLife()
    {
        if (health < 3)
        {
            health++;
        }

        CheckLife();
    }
}
