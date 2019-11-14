using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongFallingSpriteArrow : MonoBehaviour
{
    public float fallSpeed = 1;
    public float destroyHeight = -6f;                   // at this y position, arrow is automatically destroyed (if missed)

    

    public bool inSensor = false;                       // whether the arrow's in a sensor or not
    public float relativeDistance = 0;                  // distance between sensor and bottom arrow
    public bool isCurrentlyPressed = false;             // whether the arrow's button is pressed or not

    public bool arrowAligned = false;                   // true when the bottom arrow is perfectly in the sensor


    public Transform topArrow;
    public Transform bottomArrow;

    public SpriteRenderer longRend;

    bool previousFrameHit = false;

    public bool pressedBefore = false;

    public bool shattered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -fallSpeed * Time.deltaTime, 0);       //move the arrow downward;

        if (!shattered)
        {
            if (inSensor)
            {
                if (arrowAligned)
                {
                    bottomArrow.Translate(0, fallSpeed * Time.deltaTime, 0);       //keep bottom arrow stationary;
                }

                if (isCurrentlyPressed)
                {
                    //longRend.size = new Vector2(longRend.size.x, transform.position.y - bottomArrow.position.y);

                    pressedBefore = true;
                    previousFrameHit = true;
                }
                else
                {
                    if (previousFrameHit)
                    {
                        //longRend.size = new Vector2(longRend.size.x, transform.position.y - bottomArrow.position.y);

                        previousFrameHit = false;
                    }
                    else if (pressedBefore)
                    {
                        shattered = true;
                        // cause shatter
                    }
                }
            }
        }
        else
        {
            print("Shattered");
        }

        if (topArrow.transform.position.y < destroyHeight)
        {

            Destroy(gameObject);
        }

    }

    public void successfulScore()
    {
        print("Damn Son!");
    }

    public void midScore()
    {
        print("midscore");
        //Destroy(gameObject);
    }

    public void missScore()
    {
        print("no way man");
        //Destroy(gameObject);
    }
}
