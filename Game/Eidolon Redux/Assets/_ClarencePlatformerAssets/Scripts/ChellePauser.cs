using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChellePauser : MonoBehaviour
{

    public MichelleWindController windControl;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            windControl.SetPauseChelle(true);
        }
    }
}
