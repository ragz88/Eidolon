using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAdvancer : MonoBehaviour
{

    StartDelay delay;
    public float time = 0;

    public Transform[] movePieces;
    public float speed = 2.5f;

    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        delay = GetComponent<StartDelay>();

        source.time = time - delay.delayTime;
        source.Play();
        
        for (int i = 0; i < movePieces.Length; i++)
        {
            movePieces[i].Translate(new Vector3(0, (-1 * (time - Time.deltaTime)), 0), Space.World);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
