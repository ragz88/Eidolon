using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHider : MonoBehaviour
{

    //Note: script disables an object's children to prevent them from sucking up CPU power while not on screen

    Transform[] children;

    public bool childrenHidden = true;

    // Start is called before the first frame update
    void Start()
    {
        children = GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject != this.gameObject)
            {
                children[i].gameObject.SetActive(false);
            }
        }
    }

    // reactivates all the hidden children, then deletes this script component from the gameobject
    public void reactivateChildren()
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject != gameObject)
            {
                children[i].gameObject.SetActive(true);
            }
        }

        Destroy(this);
    }
}
