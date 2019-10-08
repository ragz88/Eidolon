using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextArea : MonoBehaviour
{
    [System.Serializable]
    public struct ConversationPoint
    {
        public string speechText;
        public Text textComponent;
        //public Color textColour;
        public float textDelay;
    }

    public ConversationPoint[] convoPoints;

    public Animator clarenceAnim;
    public ClarenceMovement clarenceMovement;
    Rigidbody2D clarenceRB;
    CapsuleCollider2D capCollider;
    //BoxCollider2D boxCollider;
    PhysicsMaterial2D initPhysicsMat;
    public PhysicsMaterial2D highFriction;


    int currentConvoPoint = 0;
    bool playerPresent = false;
    bool convoPlayed = false;

    float timeStamp = 0;                               // used to check text delay;
    int numberOfChars = 0;

    bool fullMessageDisplayed = false;
    bool fadingMessage = false;
    public float fadeSpeed = 1f;



    // Start is called before the first frame update
    void Start()
    {
        clarenceRB = clarenceMovement.gameObject.GetComponent<Rigidbody2D>();
        initPhysicsMat = clarenceRB.sharedMaterial;
        capCollider = clarenceRB.gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!convoPlayed)
        {
            if (playerPresent)
            {
                if (!fullMessageDisplayed)
                {
                    if (/*Input.GetMouseButtonDown(0)*/ Input.anyKeyDown || Input.GetButtonDown("ProgressText"))
                    {
                        convoPoints[currentConvoPoint].textComponent.text = convoPoints[currentConvoPoint].speechText;
                        convoPoints[currentConvoPoint].textComponent.text = convoPoints[currentConvoPoint].textComponent.text.Replace("*", "\n");
                        fullMessageDisplayed = true;
                        numberOfChars = 0;
                    }
                    else if (Time.time - timeStamp > convoPoints[currentConvoPoint].textDelay)
                    {
                        timeStamp = Time.time;
                        numberOfChars++;
                        if (numberOfChars <= convoPoints[currentConvoPoint].speechText.Length)
                        {
                            convoPoints[currentConvoPoint].textComponent.text = convoPoints[currentConvoPoint].speechText.Substring(0, numberOfChars);
                            convoPoints[currentConvoPoint].textComponent.text = convoPoints[currentConvoPoint].textComponent.text.Replace("*", "\n");
                        }
                        else
                        {
                            fullMessageDisplayed = true;
                            numberOfChars = 0;
                        }
                    }
                }
                else
                {
                    if (!fadingMessage && (/*Input.GetMouseButtonDown(0)*/ Input.anyKeyDown || Input.GetButtonDown("ProgressText")))
                    {
                        fadingMessage = true;
                    }

                    if (fadingMessage)
                    {
                        convoPoints[currentConvoPoint].textComponent.color = new Color(convoPoints[currentConvoPoint].textComponent.color.r,
                            convoPoints[currentConvoPoint].textComponent.color.g, convoPoints[currentConvoPoint].textComponent.color.b,
                            convoPoints[currentConvoPoint].textComponent.color.a - (fadeSpeed * Time.deltaTime));

                        if (convoPoints[currentConvoPoint].textComponent.color.a <= 0)
                        {
                            fadingMessage = false;
                            fullMessageDisplayed = false;
                            convoPoints[currentConvoPoint].textComponent.text = "";
                            convoPoints[currentConvoPoint].textComponent.color = new Color(convoPoints[currentConvoPoint].textComponent.color.r,
                                convoPoints[currentConvoPoint].textComponent.color.g, convoPoints[currentConvoPoint].textComponent.color.b, 1);

                            //move to next sentence in conversation
                            currentConvoPoint++;
                            if (currentConvoPoint >= convoPoints.Length)  // all sentences played already
                            {
                                convoPlayed = true;
                            }
                            timeStamp = Time.time;
                        }
                    }
                }
            }
        }
        else
        {
            UnfreezePlayer();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !playerPresent && !convoPlayed)
        {
            playerPresent = true;
            timeStamp = Time.time;
            FreezePlayer();
        }
    }

    void FreezePlayer()
    {
        clarenceMovement.enabled = false;
        clarenceAnim.SetBool("jumping", false);
        clarenceAnim.SetBool("falling", false);
        clarenceAnim.SetBool("walking", false);
        clarenceRB.sharedMaterial = highFriction;
        capCollider.sharedMaterial = highFriction;
    }

    void UnfreezePlayer()
    {
        clarenceMovement.enabled = true;
        clarenceAnim.SetBool("jumping", false);
        clarenceAnim.SetBool("falling", false);
        clarenceAnim.SetBool("walking", false);
        clarenceRB.sharedMaterial = initPhysicsMat;
        capCollider.sharedMaterial = initPhysicsMat;
    }


}
