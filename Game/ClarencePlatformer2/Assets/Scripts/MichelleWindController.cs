using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichelleWindController : MonoBehaviour
{
    // defines how the windzones in the scene are blowing
    public enum WindState
    {
        BlowingPhase1,
        Pause,
        BlowingPhase2
    }

    // defines when the system should hold it's current state
    public enum PauseType
    {
        None,
        AfterPhase1,
        AfterPhase2,
        BetweenPhases
    }

    public WindState windState = WindState.BlowingPhase1;
    public PauseType pauseType = PauseType.None;

    public float windSpeedChangeRate = 0.5f;
    public float playerAcceleration = 0.1f;

    public float pauseTime = 2f;
    public float phaseOneTime = 2f;
    public float phaseTwoTime = 2f;

    public float phaseOneWindSpeed = 0;
    public float phaseTwoWindSpeed = 0;
    public float pauseWindSpeed = 0;

    public bool twoPhaseCycle = false;

    public  float currentSpeed = 0;
    float timer = 0;
    WindState previousState;


    [SerializeField] bool pauseChelle = false;
    public bool chelleInitialPause = false;

    // body and movement controls
    public Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        //initial conditions for Michelle's animation
        if (chelleInitialPause)
        {
            animator.SetBool("Phase1", false);
            animator.SetBool("Phase2", false);
            animator.SetBool("Idle", false);
            animator.SetBool("InitIdle", true);
        }
        else
        {
            animator.SetBool("Phase1", false);
            animator.SetBool("Phase2", false);
            animator.SetBool("Idle", true);
            animator.SetBool("InitIdle", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!chelleInitialPause)
        {
            if (!pauseChelle)
            {
                if (!twoPhaseCycle)  // one speed that turns on and off
                {
                    if (pauseType == PauseType.AfterPhase1 || pauseType == PauseType.BetweenPhases)
                    {
                        if (windState == WindState.BlowingPhase1)
                        {
                            // Change speed to a specific value
                            if (Mathf.Abs(currentSpeed - phaseOneWindSpeed) > 0.05f)
                            {
                                currentSpeed = Mathf.Lerp(currentSpeed, phaseOneWindSpeed, windSpeedChangeRate * Time.deltaTime);
                            }

                            // Check how long we've been in this phase
                            if (timer >= phaseOneTime)
                            {
                                windState = WindState.Pause;

                                // define Animation parameters
                                animator.SetBool("Phase1", false);
                                animator.SetBool("Phase2", false);
                                animator.SetBool("Idle", true);


                                timer = 0;
                            }
                        }
                        else if (windState == WindState.Pause)
                        {
                            // Change speed to a specific value
                            if (Mathf.Abs(currentSpeed - pauseWindSpeed) > 0.05f)
                            {
                                currentSpeed = Mathf.Lerp(currentSpeed, pauseWindSpeed, windSpeedChangeRate * Time.deltaTime);
                            }

                            // Check how long we've been in this phase
                            if (timer >= pauseTime)
                            {
                                windState = WindState.BlowingPhase1;

                                // define Animation parameters
                                animator.SetBool("Phase1", true);
                                animator.SetBool("Phase2", false);
                                animator.SetBool("Idle", false);


                                timer = 0;
                            }
                        }

                    }
                    else // no pauses - equivalent to an unchanging speed
                    {
                        // Change speed to a phase1 value
                        if (Mathf.Abs(currentSpeed - phaseOneWindSpeed) > 0.05f)
                        {
                            currentSpeed = Mathf.Lerp(currentSpeed, phaseOneWindSpeed, windSpeedChangeRate * Time.deltaTime);
                        }

                    }
                }
                else  // two different speeds in cycle
                {
                    if (windState == WindState.BlowingPhase1)
                    {
                        // Change speed to a specific value
                        if (Mathf.Abs(currentSpeed - phaseOneWindSpeed) > 0.05f)
                        {
                            currentSpeed = Mathf.Lerp(currentSpeed, phaseOneWindSpeed, windSpeedChangeRate * Time.deltaTime);
                        }

                        // Check how long we've been in this phase
                        if (timer >= phaseOneTime)
                        {
                            if (pauseType == PauseType.AfterPhase1 || pauseType == PauseType.BetweenPhases)
                            {
                                windState = WindState.Pause;

                                // define Animation parameters
                                animator.SetBool("Phase1", false);
                                animator.SetBool("Phase2", false);
                                animator.SetBool("Idle", true);
                            }
                            else
                            {
                                windState = WindState.BlowingPhase2;

                                // define Animation parameters
                                animator.SetBool("Phase1", false);
                                animator.SetBool("Phase2", true);
                                animator.SetBool("Idle", false);
                            }

                            previousState = WindState.BlowingPhase1;
                            timer = 0;
                        }
                    }
                    else if (windState == WindState.BlowingPhase2)
                    {
                        // Change speed to a specific value
                        if (Mathf.Abs(currentSpeed - phaseTwoWindSpeed) > 0.05f)
                        {
                            currentSpeed = Mathf.Lerp(currentSpeed, phaseTwoWindSpeed, windSpeedChangeRate * Time.deltaTime);
                        }

                        // Check how long we've been in this phase
                        if (timer >= phaseTwoTime)
                        {
                            if (pauseType == PauseType.AfterPhase2 || pauseType == PauseType.BetweenPhases)
                            {
                                windState = WindState.Pause;

                                // define Animation parameters
                                animator.SetBool("Phase1", false);
                                animator.SetBool("Phase2", false);
                                animator.SetBool("Idle", true);
                            }
                            else
                            {
                                windState = WindState.BlowingPhase1;

                                // define Animation parameters
                                animator.SetBool("Phase1", true);
                                animator.SetBool("Phase2", false);
                                animator.SetBool("Idle", false);
                            }

                            previousState = WindState.BlowingPhase2;
                            timer = 0;
                        }
                    }
                    else if (windState == WindState.Pause)
                    {
                        // Change speed to a specific value
                        if (Mathf.Abs(currentSpeed - pauseWindSpeed) > 0.05f)
                        {
                            currentSpeed = Mathf.Lerp(currentSpeed, pauseWindSpeed, windSpeedChangeRate * Time.deltaTime);
                        }

                        // Check how long we've been in this phase
                        if (timer >= pauseTime)
                        {
                            if (pauseType == PauseType.AfterPhase1)  // this pause came after phase 1, now we must go into phase 2
                            {
                                windState = WindState.BlowingPhase2;

                                // define Animation parameters
                                animator.SetBool("Phase1", false);
                                animator.SetBool("Phase2", true);
                                animator.SetBool("Idle", false);
                            }
                            else if (pauseType == PauseType.AfterPhase2)  // this pause came after phase 2, now we must go into phase 1
                            {
                                windState = WindState.BlowingPhase1;

                                // define Animation parameters
                                animator.SetBool("Phase1", true);
                                animator.SetBool("Phase2", false);
                                animator.SetBool("Idle", false);
                            }
                            else if (pauseType == PauseType.BetweenPhases)
                            {
                                if (previousState == WindState.BlowingPhase1) // was phase 1 previously, thus we need to change to phase 2
                                {
                                    windState = WindState.BlowingPhase2;

                                    // define Animation parameters
                                    animator.SetBool("Phase1", false);
                                    animator.SetBool("Phase2", true);
                                    animator.SetBool("Idle", false);
                                }
                                else if (previousState == WindState.BlowingPhase2) // was phase 2 previously, thus we need to change to phase 1
                                {
                                    windState = WindState.BlowingPhase1;

                                    // define Animation parameters
                                    animator.SetBool("Phase1", true);
                                    animator.SetBool("Phase2", false);
                                    animator.SetBool("Idle", false);
                                }
                            }

                            timer = 0;
                        }
                    }

                }

                timer += Time.deltaTime;
            }
            else
            {
                windState = WindState.Pause;

                // define Animation parameters
                animator.SetBool("Phase1", false);
                animator.SetBool("Phase2", false);
                animator.SetBool("Idle", true);
            }
        }
    }

    // returns the current speed variable
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public void SetPauseChelle(bool pauseChelle)
    {
        this.pauseChelle = pauseChelle;
        if (!pauseChelle)
        {
            windState = WindState.BlowingPhase1;

            animator.SetBool("Phase1", true);
            animator.SetBool("Phase2", false);
            animator.SetBool("Idle", false);
        }
    }

    public void StartChelleMovement()
    {
        chelleInitialPause = false;
        gameObject.GetComponent<MichelleMovement>().StartChelleMovement();

        animator.SetBool("Phase1", false);
        animator.SetBool("Phase2", false);
        animator.SetBool("Idle", true);
        animator.SetBool("InitIdle", false);
    }
}
