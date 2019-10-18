using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSensor : MonoBehaviour
{
    public bool beenEntered = false;     // true after player leaves its collider
    public bool fullyActive = false;     // true when flame lerped to full size

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            beenEntered = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

    }
}
