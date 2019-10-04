﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            other.gameObject.GetComponent<ClarenceMovement>().LoseLife();


            WalkingFireActivator walkingActivator = gameObject.GetComponentInParent<WalkingFireActivator>();

            if (walkingActivator != null)
            {
                walkingActivator.RemoveActiveFlames();
            }
        }
    }
}
