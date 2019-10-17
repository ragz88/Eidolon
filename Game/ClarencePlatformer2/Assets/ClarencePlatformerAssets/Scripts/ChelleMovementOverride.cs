using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChelleMovementOverride : MonoBehaviour
{

    public Transform chelle;
    MichelleMovement chelleMovement;

    public Transform finalPoint;

    //bool overridden;

    // Start is called before the first frame update
    void Start()
    {
        chelleMovement = chelle.GetComponent<MichelleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //overridden = true;
            chelleMovement.overideTrans = finalPoint;
            chelleMovement.overridden = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //overridden = false;
            chelleMovement.overridden = false;
        }
    }
}
