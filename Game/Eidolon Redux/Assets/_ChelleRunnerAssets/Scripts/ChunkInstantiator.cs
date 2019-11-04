using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInstantiator : MonoBehaviour
{

    public GameObject[] chunkPrefabs;

    [SerializeField] GameObject[] randomisedChunks;

    public MaterialTreadmill treadmill;
    

    // Start is called before the first frame update
    void Start()
    {
        randomisedChunks = new GameObject[chunkPrefabs.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnChunk()
    {
        if (randomisedChunks[0] != null)
        {
            GameObject currentChunk = Instantiate(randomisedChunks[0], transform.position, randomisedChunks[0].transform.rotation) as GameObject;
            treadmill.AddChunk(currentChunk);

            // shift all elements left and replace end with null 
            for (int i = 0; i < randomisedChunks.Length; i++)
            {
                if (i != randomisedChunks.Length - 1)
                {
                    if (randomisedChunks[i] != null)
                    {
                        randomisedChunks[i] = randomisedChunks[i + 1];
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (randomisedChunks[i] != null)
                    {
                        randomisedChunks[i] = null;
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
        else
        {
            FillRandomisedChunkPool();
            SpawnChunk();
        }
    }

    void FillRandomisedChunkPool()
    {
        randomisedChunks = new GameObject[chunkPrefabs.Length];

        int[] unusedChunks = new int[randomisedChunks.Length];    // stores the index ints from chunkPrefabs that still haven't been ussed
        // initialisation
        for (int i = 0; i < unusedChunks.Length; i++)
        {
            unusedChunks[i] = i;
        }

        int assignedChunkCounter = 0;   // counts how many valid chunks we stored. Stops function once we have chunkPrefabs.length of them

        while (assignedChunkCounter < randomisedChunks.Length)
        {
            int randomInt = Random.Range(0, randomisedChunks.Length);    // find a random, but relevant, index
            

            if (unusedChunks[randomInt] != -1)
            {
                randomisedChunks[assignedChunkCounter] = chunkPrefabs[randomInt];
                assignedChunkCounter++;

                unusedChunks[randomInt] = -1;    // -1  means no longer usable
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ChunkEnd")
        {
            SpawnChunk();
        }
    }
}
