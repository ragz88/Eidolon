using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDestroyer : MonoBehaviour
{

    public MaterialTreadmill treadmill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChunkEnd")
        {
            treadmill.RemoveChunk();
            Destroy(other.transform.parent.gameObject);
        }
    }
}
