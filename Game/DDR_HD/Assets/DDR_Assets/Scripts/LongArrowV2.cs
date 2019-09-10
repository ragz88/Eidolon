using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongArrowV2 : MonoBehaviour
{

    public float fallSpeed = 1;
    public float destroyHeight = -6f;                   // at this y position, arrow is automatically destroyed (if missed)

    public Transform topArrow, bottomArrow;             // stores arrow caps of line renderer
    public LineRenderer bar;                            // bar that fills as player presses successfully

    public bool[] hitArray;                             // stores true or false depending on if the player hit at that specific time

    public int numDivisions = 100;                      // number of points in bar

    public bool inSensor = false;                       // whether the arrow's in a sensor or not
    public float relativeDistance = 0;                  // distance between sensor and bottom arrow
    public bool isCurrentlyPressed = false;             // whether the arrow's button is pressed or not

    Vector3[] positions;                                // stores numDivisions equi-distant positions along our bar
    AnimationCurve widthCurve = new AnimationCurve();   // sores the widths at various points on our bar

    float animCurveMax = 1;                             // all animation curves go from 0 to 1

    float arrowLength;                                  // distance between the 2 arrow caps     
    float maxBarWidth = 1;                              // width of our bar

    
    //public int numPointsToRemove = 2;                 // number of points to delete from positions to ensure the bar doesn't stick out from bottom.

    bool pressedInPreviousFrame = false;                // records whether the correct button was pressed in the previous frame, preventing skipping due to speed.
    float previousY = 0;                                // used in conjunction with above bool for investigating skipping due to translate speed



    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[numDivisions];
        hitArray = new bool[numDivisions];

        //arrowLength = Mathf.Abs(topArrow.position.y - bottomArrow.position.y);          // finds distance between arrow caps
        arrowLength = Mathf.Abs(bar.GetPosition(0).z - bar.GetPosition(1).z);

        for (int i = 0; i < numDivisions; i++)
        {
            hitArray[i] = false;                                                        // initialise array to false
            positions[i] = new Vector3(0,0, (arrowLength/numDivisions)*i);              // set the equi-distant positions
            widthCurve.AddKey((animCurveMax/(numDivisions))*i,0);                       // initialise widths as 0 along bar's entire length
        }

        bar.positionCount = numDivisions;                                               // update size of our bar's position array
        bar.SetPositions(positions);
        bar.widthCurve = widthCurve;
    }

    /*private void FixedUpdate()
    {
        previousY = transform.position.y;
        transform.Translate(0, -fallSpeed * Time.fixedDeltaTime, 0);       //move the arrow downward;
    }*/

    // Update is called once per frame
    void Update()
    {

        previousY = transform.position.y;
        transform.Translate(0, -fallSpeed * Time.deltaTime, 0);       //move the arrow downward;

        if (inSensor && (relativeDistance >= 0  && relativeDistance <= arrowLength))
        {
            int currentIndex = 0;                             // stores the index of the division closest to the current position of the arrow

            for (int i = 0; i < numDivisions; i++)
            {      
                if (positions[i].z > relativeDistance)        // previous i value was most accurate division, hence we break
                {
                    break;                                    
                }
                else
                {
                    currentIndex = i;
                }
            }

            if (isCurrentlyPressed )
            {
                
                hitArray[currentIndex] = true;
                widthCurve.RemoveKey(numDivisions - (currentIndex + 1));
                Keyframe newKey = new Keyframe((numDivisions - (currentIndex + 1)) * (animCurveMax / numDivisions), maxBarWidth, 0, 0);
                widthCurve.AddKey(newKey);

                // this smoothes out or bar - sometimes a frame or two are missed
                if (currentIndex > 2 && hitArray[currentIndex - 2] == true && hitArray[currentIndex - 1] == false)    // a false trapped between 2 trues
                {
                    hitArray[currentIndex - 1] = true;                                                                         // repair trapped false
                    widthCurve.RemoveKey(numDivisions - (currentIndex));                                                         // and adjust that key's width
                    newKey = new Keyframe((numDivisions - (currentIndex)) * (animCurveMax / numDivisions), maxBarWidth, 0, 0);
                    widthCurve.AddKey(newKey);
                }

                // this further smoothes the curve by examine the button press in the previous frame - preventing skipping due to speed.
                // in this case, the correct button was pressed in the previous frame, but some positions were still left false
                // this is due to the arrow moving too quickly for the framerate to keep up with
                /*if (pressedInPreviousFrame && currentIndex != 0 && hitArray[currentIndex - 1] == false)
                {
                    int previousExaminedIndex = -1;       // stores the index of the last position that was properly caught by the script 
                    
                    // here we figure out which position was the last unskipped one
                    for (int i = 0; i < positions.Length; i++)
                    {
                        if (positions[i].z > previousY)
                        {
                            previousExaminedIndex = i;
                            break;
                        }
                    }

                    // here we set all the positions that were missed to true
                    for (int i = previousExaminedIndex; i < currentIndex; i++)
                    {
                        hitArray[i] = true;
                        widthCurve.RemoveKey(numDivisions - (i + 1));
                        newKey = new Keyframe((numDivisions - (i + 1)) * (animCurveMax / numDivisions), maxBarWidth, 0, 0);
                        widthCurve.AddKey(newKey);
                    }                  
                }*/


                pressedInPreviousFrame = true;        // next frame we'll consider this bool
            }
            else
            {
                pressedInPreviousFrame = false;       // next frame we'll consider this bool
            }

            bar.widthCurve = widthCurve;                      // update the width curve
        }
        else if (topArrow.transform.position.y < destroyHeight)
        {
            
            Destroy(gameObject);
        }
    }

    // used by ArrowSensorV2 to prevent the last few points from being missed due to framerate issues
    public void setFinalPoints (int numPoints)
    {
        for (int i = 0; i < numPoints; i++)
        {
            print("Total points: " + numDivisions);
            print("hit array num: " + ((hitArray.Length - 1) - i));
            print("curve num: " + (numDivisions - (widthCurve.length) + i));

            hitArray[(hitArray.Length - 1) - i] = true;
            widthCurve.RemoveKey(i);
            Keyframe newKey = new Keyframe(i * (animCurveMax / numDivisions), maxBarWidth, 0, 0);
            print("New Key time: " + newKey.time + " and it's width: " + newKey.value);
            widthCurve.AddKey(newKey);
        }
    }
}
