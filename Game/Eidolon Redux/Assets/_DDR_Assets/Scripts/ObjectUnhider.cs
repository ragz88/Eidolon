using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUnhider : MonoBehaviour
{
    
    //tells all the arrows that enter the trigger to show their children again
    private void OnTriggerEnter(Collider other)
    {
        ChildHider hider = other.GetComponent<ChildHider>();
        if (hider != null)
        {
            
            hider.reactivateChildren();
        }
    }
}
