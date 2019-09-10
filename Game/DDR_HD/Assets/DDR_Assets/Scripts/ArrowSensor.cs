﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSensor : MonoBehaviour
{
    public KeyCode sensorKey;

    public Animation sensorPulse;

    GeneralSensorSettings settings;

    GameObject collidingArrow;
    GameObject currentArrow;                  // stores the most central falling arrow in the collider

    bool arrowInTrigger = false;              // true when falling arrow present in sensor
    bool missFlash = false;                   // true when the sensor arrow should falsh to indicate a miss.
    

    // Start is called before the first frame update
    void Start()
    {
        settings = GetComponentInParent<GeneralSensorSettings>();
    }

    // Update is called once per frame
    void Update()
    {

        if (currentArrow != null)
        {
            if ((currentArrow.transform.position.y - transform.position.y) < -settings.greatDistance && settings.missAfterPerfect)  //at this point, player can no longer capture this arrow in classic mode (miss after perfect mode)
            {
                settings.keyWordText.text = settings.missString;
                settings.keyWordText.color = settings.missColour;
                //change layer/colour etc of missed arrow
                // double speed and respawn it - like super crate box

                currentArrow.gameObject.layer = LayerMask.NameToLayer("MissedArrow");
                currentArrow = null;
            }
            else if ((currentArrow.transform.position.y - transform.position.y) < -settings.almostDistance)  //at this point, player can no longer capture this arrow in std mode in the simple mode
            {
                settings.keyWordText.text = settings.missString;
                settings.keyWordText.color = settings.missColour;
                //change layer/colour etc of missed arrow
                // double speed and respawn it - like super crate box

                currentArrow.gameObject.layer = LayerMask.NameToLayer("MissedArrow");
                currentArrow = null;
            }
        }

        if (arrowInTrigger && collidingArrow != null)
        {
            float relativeDist = collidingArrow.transform.position.y - transform.position.y;    // distance between falling arrow's centre and centre of arrow sensor

            //replace current arrow if one does not exist
            if (currentArrow == null)
            {
                currentArrow = collidingArrow;
            }

            if (Input.GetKeyDown(sensorKey))  //else statement ensures that only one arrow can be destroyed at a time
            {
                pulseSensor();

                if (currentArrow == collidingArrow)
                {

                    if (settings.missAfterPerfect)
                    {
                        if (Mathf.Abs(relativeDist) <= settings.perfectDistance)
                        {
                            perfectScore();
                            destroyArrow(currentArrow);
                        }
                        else if (Mathf.Abs(relativeDist) <= settings.greatDistance)  //technically misses after great - hence the Abs here
                        {
                            greatScore();
                            destroyArrow(currentArrow);
                        }
                        else if ((relativeDist) <= settings.goodDistance && (relativeDist) >= 0)
                        {
                            goodScore();
                            destroyArrow(currentArrow);
                        }
                        else if (Mathf.Abs(relativeDist) <= settings.almostDistance && (relativeDist) >= 0)
                        {
                            almostScore();
                            destroyArrow(currentArrow);
                        }
                        else if (relativeDist < -settings.greatDistance)
                        {
                            missedScore();
                            //change layer/colour etc of missed arrow
                            // double speed and respawn it - like super crate box

                            currentArrow.gameObject.layer = LayerMask.NameToLayer("MissedArrow");
                            currentArrow = null;
                        }

                    }
                    else
                    {
                        if (Mathf.Abs(relativeDist) <= settings.perfectDistance)
                        {
                            perfectScore();
                        }
                        else if (Mathf.Abs(relativeDist) <= settings.greatDistance)
                        {
                            greatScore();
                        }
                        else if (Mathf.Abs(relativeDist) <= settings.goodDistance)
                        {
                            goodScore();
                        }
                        else if (Mathf.Abs(relativeDist) <= settings.almostDistance)
                        {
                            almostScore();
                        }

                        destroyArrow(currentArrow);
                    }

                }
            }

            
        }
        else if (Input.GetKeyDown(sensorKey))  //button pressed when no arrow present.
        {
            missedScore();
            pulseSensor();
        }
    }

   /* private void OnTriggerStay(Collider collision)
    {
        collidingArrow = collision.gameObject;
        arrowInTrigger = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        collidingArrow = collision.gameObject;
        arrowInTrigger = true;
    }

    private void OnTriggerExit(Collision collision)
    {
        //arrowInTrigger = false;
    }
    */ // removed for testing only

    // ensures the currentArrow variable is updated when the gameobject is destroyed
    void destroyArrow(GameObject arrow)
    {
        currentArrow = null;
        arrowInTrigger = false;
        Destroy(arrow);
    }

    void missedScore()
    {
        settings.keyWordText.text = settings.missString;
        settings.keyWordText.color = settings.missColour;
    }

    void emptyPressScore()
    {
        
    }

    void almostScore()
    {
        settings.keyWordText.text = settings.almostString;
        settings.keyWordText.color = settings.almostColour;
    }

    void goodScore()
    {
        settings.keyWordText.text = settings.goodString;
        settings.keyWordText.color = settings.goodColour;
    }

    void greatScore()
    {
        settings.keyWordText.text = settings.greatString;
        settings.keyWordText.color = settings.greatColour;
    }

    void perfectScore()
    {
        settings.keyWordText.text = settings.perfectString;
        settings.keyWordText.color = settings.perfectColour;
    }

    void pulseSensor()
    {
        sensorPulse.Play("LeftSensorPulse");
    }


}
