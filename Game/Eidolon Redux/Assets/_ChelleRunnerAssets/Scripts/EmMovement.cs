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

    // the following are used to track Em's current movement parameters
    // Note: Em can turn while jumping and jump while turning - but this doesn't apply to sliding
    // Note: Em can slide out of jumping and jump out of sliding
    public enum EmSpeedState
    {
        Running,
        Jogging,
        Walking,
        Stationary
    }

    public enum EmSpecialState
    {
        Sliding,
        Jumping,
        None
    }

    public EmSpecialState specialState = EmSpecialState.None;
    public EmSpeedState speedState = EmSpeedState.Stationary;
    public bool emTurning = false;

    public Transform[] laneMarkers;   // marks the centre for each lane

    Transform currentTarget;

    public float laneShiftSpeed = 3;
    public float jumpSpeed = 3;
    public float slideFallSpeed = 3;
    public float slideDuration = 1f;

    public Animator emAnim;
    public SpriteRenderer spriteRend;

    public BoxCollider slideBox;
    public BoxCollider standardBox;

    public float extraGravity = 1f;

    Rigidbody emRigidBody;

    //public Transform emSpriteTrans;

    public SpriteRenderer emRend;

    //public Transform jumpPoint;
    //public Transform slidePoint;
    Vector3 initPoint;

    //bool jumpRising = false;
    float slidingTimer = 0;

    public LayerMask groundRayLayers;
    public float groundRayStandardLength;
    public float groundRaySlideLength;

    float currentGroundRayLength = 0;

    bool invulnerable = false;
    public float delayBetweenColours = 0.3f;
    public Material[] invulMaterials;
    int currentInvulColour = 0;

    public float fullCrashResetSpeed = 0.3f;
    public float midCrashResetSpeed = 0.3f;

    public MaterialTreadmill treadmill;

    float invulnerableTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentTarget = laneMarkers[(int)currentLane];
        initPoint = transform.position;

        emRigidBody = GetComponent<Rigidbody>();

        emAnim.SetBool("walking", true);

        currentGroundRayLength = groundRayStandardLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (speedState != EmSpeedState.Stationary)
        {
            if (invulnerable)
            {
                if ((invulnerableTimer <= Time.time - delayBetweenColours) || (invulnerableTimer <= Time.time - (delayBetweenColours/2) && currentInvulColour < 2))
                {
                    if (currentInvulColour >= invulMaterials.Length - 1)
                    {
                        invulnerable = false;
                        currentInvulColour = 0;
                    }
                    else
                    {
                        
                        emRend.material = invulMaterials[currentInvulColour];
                        currentInvulColour++;
                    }

                    invulnerableTimer = Time.time;
                }
            }

            float movement = Input.GetAxisRaw("Horizontal");

            //if (specialState != EmSpecialState.Sliding) // check if turn is allowed
            //{
                if (movement < 0)    // left move
                {
                    if (currentLane != Lane.Left)
                    {
                        if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.05f)
                        {
                            currentLane--;
                            currentTarget = laneMarkers[(int)currentLane];
                            if (!emTurning)
                            {
                                emAnim.SetBool("switchLane", true);
                            }
                            spriteRend.flipX = false;
                            emTurning = true;
                        }
                    }
                }
                else if (movement > 0) // right move
                {
                    if (currentLane != Lane.Right)
                    {
                        if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.05f)
                        {
                            currentLane++;
                            currentTarget = laneMarkers[(int)currentLane];
                            if (!emTurning)
                            {
                                emAnim.SetBool("switchLane", true);
                            }
                            spriteRend.flipX = true;
                            emTurning = true;
                        }
                    }
                }
            //}

            // turn control
            if (emTurning)
            {
                if (Mathf.Abs(transform.position.x - currentTarget.position.x) > 0.05f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentTarget.position.x, transform.position.y, transform.position.z), laneShiftSpeed * Time.deltaTime);
                }
                else
                {
                    emAnim.SetBool("switchLane", false);
                    spriteRend.flipX = false;
                    emTurning = false;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentTarget.position.x, transform.position.y, transform.position.z), laneShiftSpeed * Time.deltaTime);
                }
            }

            // jump control    //add raycast
            if (specialState != EmSpecialState.Jumping && (Input.GetAxisRaw("Vertical") > 0.2f || Input.GetButtonDown("EmRunnerJump")))
            {
                if (specialState == EmSpecialState.Sliding)
                {
                    currentGroundRayLength = groundRaySlideLength;
                }
                else
                {
                    currentGroundRayLength = groundRayStandardLength;
                }

                RaycastHit hit;
                // Does the ray intersect with ground
                if (Physics.Raycast(transform.position, -transform.up, out hit, currentGroundRayLength, groundRayLayers))
                {
                    emAnim.SetBool("switchLane", false);
                    emAnim.SetBool("jumping", true);
                    emAnim.SetBool("sliding", false);

                    standardBox.enabled = true;
                    slideBox.enabled = false;
                    //jumpBox.enabled = true;
                    // jumpRising = true;
                    specialState = EmSpecialState.Jumping;

                    emRigidBody.velocity = new Vector3(0, jumpSpeed, 0);

                    slidingTimer = Time.time - slideDuration;   // makes sure slide is deactivated in future
                }
            }

            if (emAnim.GetBool("falling") == true)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -transform.up, out hit, currentGroundRayLength, groundRayLayers))
                {
                    emAnim.SetBool("falling", false);
                }
            }

            if (specialState == EmSpecialState.Jumping)
            {
                emRigidBody.AddForce(new Vector3(0, extraGravity, 0));       // use to improve feel

                if (emRigidBody.velocity.y <= 0)
                {
                    emAnim.SetBool("switchLane", false);
                    emAnim.SetBool("jumping", false);
                    emAnim.SetBool("falling", true);
                    
                    RaycastHit hit;
                    // Does the ray intersect with ground
                    if (Physics.Raycast(transform.position, -transform.up, out hit, currentGroundRayLength, groundRayLayers))
                    {
                        specialState = EmSpecialState.None;

                        emAnim.SetBool("falling", false);
                    }
                }
            }

            // slide control
            if (/*(!emTurning || specialState == EmSpecialState.Jumping) &&*/ specialState != EmSpecialState.Sliding && (Input.GetAxisRaw("Vertical") < -0.2f || Input.GetButtonDown("EmRunnerSlide")))
            {
                //emAnim.SetBool("switchLane", false);
                emAnim.SetBool("jumping", false);
                emAnim.SetBool("sliding", true);
                emAnim.SetBool("switchLane", false);

                emRigidBody.velocity = new Vector3(0, slideFallSpeed, 0);

                //here we change the collider to the short one
                standardBox.enabled = false;
                slideBox.enabled = true;
                //jumpBox.enabled = false;

                slidingTimer = Time.time;
                
                specialState = EmSpecialState.Sliding;
            }

            if (specialState == EmSpecialState.Sliding)
            {
                //emSpriteTrans.position = Vector3.Lerp(emSpriteTrans.position, new Vector3(emSpriteTrans.position.x, initPoint.y, emSpriteTrans.position.z), 3 * Time.deltaTime);

                RaycastHit hit;
                // Does the ray intersect with ground
                
                if (Physics.Raycast(transform.position, -transform.up, out hit, currentGroundRayLength, groundRayLayers))
                {
                    if (Time.time >= (slidingTimer + slideDuration))
                    {
                        specialState = EmSpecialState.None;

                        standardBox.enabled = true;
                        slideBox.enabled = false;

                        emAnim.SetBool("sliding", false);
                    }
                }
                else
                {
                    if (Time.time < (slidingTimer + slideDuration))
                    {
                        slidingTimer = Time.time;
                    }
                }
            }

            Debug.DrawRay(transform.position, -transform.up * groundRaySlideLength, Color.yellow);
            Debug.DrawRay(transform.position, -transform.up * groundRayStandardLength, Color.yellow);

            

        }
    }

    public void StartWalk()
    {
        emAnim.SetBool("walking", true);
        emAnim.SetBool("running", false);
        emAnim.SetBool("jogging", false);
        speedState = EmSpeedState.Walking;
    }

    public void StartJog()
    {
        emAnim.SetBool("walking", false);
        emAnim.SetBool("running", false);
        emAnim.SetBool("jogging", true);

        speedState = EmSpeedState.Jogging;
    }

    public void StartRun()
    {
        emAnim.SetBool("walking", false);
        emAnim.SetBool("running", true);
        emAnim.SetBool("jogging", false);

        speedState = EmSpeedState.Running;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!invulnerable)
        {
            if (other.tag == "HalfCrash")
            {
                /*if (Mathf.Abs(treadmill.ySpeed/2) < midCrashResetSpeed)
                {
                    treadmill.ChangeSpeed(midCrashResetSpeed);
                }
                else
                {
                    treadmill.ChangeSpeed(treadmill.ySpeed / 2);
                }*/
                

                invulnerable = true;
                invulnerableTimer = Time.time;
            }
            else if (other.tag == "FullCrash")
            {
                //treadmill.ChangeSpeed(fullCrashResetSpeed);

                invulnerable = true;
                invulnerableTimer = Time.time;
            }

            
        }
    }
}
