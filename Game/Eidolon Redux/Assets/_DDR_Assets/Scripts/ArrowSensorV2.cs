using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSensorV2 : MonoBehaviour
{
    // This stores a list of possible arrow types that could be within the sensor
    public enum ArrowType
    {
        Standard,
        Long
    }

    //Specialised object for falling arrows that enter this sensor - they'll be stored in an array and minipulated.
    //--------------------------------------------------------------------------------------------------------------
    struct EnteredArrow
    {
        //Variable declaration
        public float entryTime;                  // stores the time that the arrow enters the sensor
        public int objectID;                     // unique integer connected to specific arrow's gameobject
        public GameObject arrowObject;           // falling arrow gameobject
        public ArrowType arrowType;              // Investigates what type of arrow is present

        
        //Constructor (not necessary, but helpful)
        //Used like this: EnteredArrow arrow = new EnteredArrow(0.15f, arrow1);
        public EnteredArrow(float entryTime, GameObject arrowObject, ArrowType arrowType)
        {
            this.entryTime = entryTime;
            this.objectID = arrowObject.GetInstanceID();
            this.arrowObject = arrowObject;
            this.arrowType = arrowType;
        }
    }
    //--------------------------------------------------------------------------------------------------------------


    //Public Variables ---------------------------------------------------------------------------------------------

    public string sensorButtonName;              // Stores the button linked to this arrow (name from Unity Input Manager)
    public string sensorAxisName;                // Stores axis linked to sensor (for joysticks)
    public int axisValue;                        // -1 or 1 dependant on direction (up and right are +)
    bool axisPressedPreviousFrame = false;       // used to prevent holding button in with controller

    public GameObject standardPartsPrefab;       // Particles to play when a std arrow is destroyed

    public int endLongArrowLiniency = 2;

    public ParticleSystem longArrowParticles;    // Particles that show when a long arrow is being hit
    public DDRScoreManager scoreManager;


    //End Public Variables -----------------------------------------------------------------------------------------



    //Private Variables --------------------------------------------------------------------------------------------

    bool missArrowAfterPerfect = false;          // Defines when a falling arrow should count as missed - set by general settings object

    EnteredArrow[] currentEnteredArrows;

    int maxEnteredArrows = 4;                   // Maximum number of arrows expected in the sensor ant any given time.

    GeneralSensorSettings sensorSettings;       // Stores tweakable settings that apply to all sensors

    LongArrowV2 currentLongFallingArrow;        // Stores component of long falling arrow should one enter the sensor
    int currentLongArrowID = 0;                 // Stores ID of long falling arrow that's currently in sensor.

    //End Private Variables ----------------------------------------------------------------------------------------



    // Start is called before the first frame update
    void Start()
    {
        currentEnteredArrows = new EnteredArrow[maxEnteredArrows];            // initialising size of array
        for (int i = 0; i < currentEnteredArrows.Length; i++)
        {
            currentEnteredArrows[i].arrowObject = null;
            currentEnteredArrows[i].objectID = 0;
            currentEnteredArrows[i].entryTime = 0;
        }

        sensorSettings = GetComponentInParent<GeneralSensorSettings>();       // finds our settings from the scene

        missArrowAfterPerfect = sensorSettings.missAfterPerfect;

        StopLongArrowParticles();                                             // hides the particles for long arrow feedbacks
    }

    // Update is called once per frame
    void Update()
    {
        // NOTE: we always keep the lowest arrow in the sensor in the 1st position in the array.
        // this makes it easy to check its position and make decisions based on that.

        if (currentEnteredArrows[0].arrowObject != null)                     // checks if the array is empty
        {
            
            // Standard Arrow Handler =====================================================================================================
            if (currentEnteredArrows[0].arrowType == ArrowType.Standard)
            {
                float relativeDistance = (currentEnteredArrows[0].arrowObject.transform.position.y - transform.position.y);   // distance between the center of lowest arrow and center of the sensor (with sign)
                float absRelativeDistance = Mathf.Abs(relativeDistance);                                                      // distance between the center of lowest arrow and center of the sensor (magnitude only)

                float missDistance = 0;                          // This will either store perfect distance or almost distance, depending on the MissArrowAfterPerfect
                                                                 // setting. It defines how far an arrow can pass the center of the sensor before it's considered a miss.


                if (missArrowAfterPerfect)
                {
                    missDistance = sensorSettings.perfectDistance;     //miss after perfect sweet spot.
                }
                else
                {
                    missDistance = sensorSettings.almostDistance;      //miss once arrow passes sensor completely
                }

                // Check hit type using missDistance ======================================================================================

                if (Input.GetButtonDown(sensorButtonName) || (Input.GetAxisRaw(sensorAxisName) == axisValue && !axisPressedPreviousFrame))                   // checks if the sensor's allocated button is being pressed
                {
                    axisPressedPreviousFrame = true;

                    if (absRelativeDistance <= sensorSettings.perfectDistance)     //Perfect Hit!
                    {
                        //scoreManager.DisplayHitQuality(DDRScoreManager.HitQuality.Perfect);
                        scoreManager.processStandardArrowHit(DDRScoreManager.HitQuality.Perfect);
                        DeleteFirstArrow();
                        Instantiate(standardPartsPrefab);
                    }
                    else if ((absRelativeDistance <= sensorSettings.greatDistance) && (relativeDistance >= -missDistance))  //Great Hit.
                    {
                        //scoreManager.DisplayHitQuality(DDRScoreManager.HitQuality.Great);
                        scoreManager.processStandardArrowHit(DDRScoreManager.HitQuality.Great);
                        DeleteFirstArrow();
                        Instantiate(standardPartsPrefab);
                    }
                    else if ((absRelativeDistance <= sensorSettings.goodDistance) && (relativeDistance >= -missDistance))  //Good Hit.
                    {
                        //scoreManager.DisplayHitQuality(DDRScoreManager.HitQuality.Good);
                        scoreManager.processStandardArrowHit(DDRScoreManager.HitQuality.Good);
                        DeleteFirstArrow();
                        Instantiate(standardPartsPrefab);
                    }
                    else if ((absRelativeDistance <= sensorSettings.almostDistance) && (relativeDistance >= -missDistance))  //Almost Hit.
                    {
                        //scoreManager.DisplayHitQuality(DDRScoreManager.HitQuality.Almost);
                        scoreManager.processStandardArrowHit(DDRScoreManager.HitQuality.Almost);
                        DeleteFirstArrow();
                        Instantiate(standardPartsPrefab);
                    }
                    else if (relativeDistance < -missDistance)  //Miss
                    {
                        //scoreManager.DisplayHitQuality(DDRScoreManager.HitQuality.Miss);
                        scoreManager.processStandardArrowHit(DDRScoreManager.HitQuality.Miss);
                        DeleteFirstArrow();
                    }
                }
                else
                {
                    if (Input.GetAxisRaw(sensorAxisName) != axisValue)
                    {
                        axisPressedPreviousFrame = false;
                    }
                    

                    if (relativeDistance < -missDistance)  //Miss                                              
                    {
                        scoreManager.processStandardArrowHit(DDRScoreManager.HitQuality.Miss);
                        DeleteFirstArrow();
                    }

                }

                // End Check Hit Type =====================================================================================================
            }
            // End Standard Arrow Handler =================================================================================================

            // Long Arrow Handler =========================================================================================================
            else if (currentEnteredArrows[0].arrowType == ArrowType.Long)
            {
                
                if (currentEnteredArrows[0].objectID != currentLongArrowID)     // check if the current long arrow has changed from the previous frame
                {
                    currentLongFallingArrow = currentEnteredArrows[0].arrowObject.GetComponent<LongArrowV2>();
                    currentLongArrowID = currentEnteredArrows[0].objectID;
                }

                if (currentLongFallingArrow.topArrow.position.y < transform.position.y)         // this marks the point when the long arrow can no longer score
                {
                    StopLongArrowParticles();
                    
                    // this is a final check to ensure the last point in the bar isn't missed due to framerate issues
                    if (Input.GetButton(sensorButtonName) || Input.GetButtonDown(sensorButtonName) || Input.GetAxisRaw(sensorAxisName) == axisValue)
                    {
                       currentLongFallingArrow.setFinalPoints(endLongArrowLiniency);      // sets the last few points in the bar to true if the button is still depresseds
                    }

                    // analyse hit quality by how much of arrow was hit
                    float longArrowScore = ExtractLongArrowScore();

                    // magic numbers to be repaired
                    if (longArrowScore >= 0.9f)     //Perfect Hit!
                    {
                        scoreManager.processLongArrowHit(DDRScoreManager.HitQuality.Perfect);
                    }
                    else if (longArrowScore >= 0.75f && longArrowScore < 0.9f)  //Great Hit.
                    {
                        scoreManager.processLongArrowHit(DDRScoreManager.HitQuality.Great);
                    }
                    else if (longArrowScore >= 0.5f && longArrowScore < 0.75f)  //Good Hit.
                    {
                        scoreManager.processLongArrowHit(DDRScoreManager.HitQuality.Good);
                    }
                    else if (longArrowScore >= 0.15f && longArrowScore < 0.5f)  //Almost Hit.
                    {
                        scoreManager.processLongArrowHit(DDRScoreManager.HitQuality.Almost);
                    }
                    else if (longArrowScore < 0.15f)  //Miss
                    {
                        scoreManager.processLongArrowHit(DDRScoreManager.HitQuality.Miss);
                    }


                    // reset arrow's sensor information and remove our reference to it
                    currentLongFallingArrow.inSensor = false;
                    currentLongFallingArrow.isCurrentlyPressed = false;
                    currentLongFallingArrow = null;
                    DeleteFirstLongArrow();

                }
                else                                                                            // Arrow still in sensor
                {
                    currentLongFallingArrow.inSensor = true;
                    currentLongFallingArrow.relativeDistance = transform.position.y - currentLongFallingArrow.bottomArrow.position.y;

                    if (Input.GetButton(sensorButtonName) || Input.GetButtonDown(sensorButtonName) || Input.GetAxisRaw(sensorAxisName) == axisValue)
                    {
                        currentLongFallingArrow.isCurrentlyPressed = true;
                        //PlayLongArrowParticles();                                          still requires tweaking
                    }
                    else
                    {
                        currentLongFallingArrow.isCurrentlyPressed = false;
                    }
                }     

            }

            // End Long Arrow Handler =====================================================================================================
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        int  currentID = other.gameObject.GetInstanceID();           // will be used to check this gameobject against the other falling arrow objects already in the array.
        bool arrowAlreadyPresent = false;                            // set to true if arrow already in the array
        int firstNullPosition = -1;                                  // stores the first open position in array, allowing the adding of a new arrow at said position

        for (int i = 0; i < currentEnteredArrows.Length; i++)        // loops through all arrows currently in sensor
        {

            if (currentEnteredArrows[i].arrowObject != null)         // prevents object reference errors
            {
                if (currentEnteredArrows[i].objectID == currentID)
                {
                    arrowAlreadyPresent = true;
                }
            }
            else if (firstNullPosition == -1)                        // checks if first null has been set yet
            {
                firstNullPosition = i;
                break;                                               // should never be nulls in the center - a null represents the space after the last array element
            }

            if (arrowAlreadyPresent)
            {
                break;                                               // arrow already found, so we exit our loop
            }
        }

       

        // if the arrow is not in the array AND there is an empty space AND the array isn't full
        if (!arrowAlreadyPresent  &&  (firstNullPosition > -1)  &&  (firstNullPosition <= (maxEnteredArrows - 1) )  )
        {
            if (other.gameObject.GetComponent<StandardFallingArrow>() || other.gameObject.GetComponent<FlippingArrow>())       //Note: after flipping arrows flip midway, thay behave just like standard arrows
            {
                currentEnteredArrows[firstNullPosition] = new EnteredArrow(Time.time, other.gameObject, ArrowType.Standard);
            }
            else if (other.gameObject.GetComponent<LongFallingArrow>())
            {
                currentEnteredArrows[firstNullPosition] = new EnteredArrow(Time.time, other.gameObject, ArrowType.Long);
            }

        }

    }

    // This removes the first arrow in the array and shifts all the remaining arrows over - leaving no empty space
    void DeleteFirstArrow()
    {
        // Here we delete the fallingarrow gameobject
        Destroy(currentEnteredArrows[0].arrowObject);

        // Shifts every object in array 1 space left.
        // Note: this automatically removes obscelete values in the first element of array.
        for (int i = 0; i < currentEnteredArrows.Length - 1; i++)           
        {
            currentEnteredArrows[i] = currentEnteredArrows[i + 1];            
        }

        ReinitialiseEnteredArrow(currentEnteredArrows[currentEnteredArrows.Length - 1]);

        
    }


    //Examines arrow to see percentage of it that was hit.
    float ExtractLongArrowScore()
    {
        float percentageHit = 0;

        for (int i = 0; i < currentLongFallingArrow.hitArray.Length; i++)
        {
            //print(currentLongFallingArrow.hitArray[i]);
            if (currentLongFallingArrow.hitArray[i] == true)
            {
                percentageHit += (1f / currentLongFallingArrow.hitArray.Length);
            }
            /*print("Lenght: " + currentLongFallingArrow.hitArray.Length);
            print("Value: " + (1f / currentLongFallingArrow.hitArray.Length));
            print(1/24);*/
        }

        return percentageHit;
    }
    
    // This removes the first arrow in the array if it's a long arrow and shifts all the remaining arrows over - leaving no empty space
    void DeleteFirstLongArrow()
    {   
        // Shifts every object in array 1 space left.
        // Note: this automatically removes obscelete values in the first element of array.
        for (int i = 0; i < currentEnteredArrows.Length - 1; i++)
        {
            currentEnteredArrows[i] = currentEnteredArrows[i + 1];
        }

        ReinitialiseEnteredArrow(currentEnteredArrows[currentEnteredArrows.Length - 1]);
    }

    // resets an enteredArrow to it's initial set of values
    void ReinitialiseEnteredArrow(EnteredArrow arrow)
    {
        arrow.arrowObject = null;
        arrow.entryTime = 0;
        arrow.objectID = 0;
    }

    // to be implemented in next iteration
    void PlayLongArrowParticles()
    {
        if (!longArrowParticles.gameObject.activeInHierarchy)
        {
            longArrowParticles.gameObject.SetActive(true);
            longArrowParticles.Play();
        }
        
    }

    void StopLongArrowParticles()
    {
        longArrowParticles.Pause();
        longArrowParticles.gameObject.SetActive(false);
    }

    void SpawnScoreText()
    {

    }

}
