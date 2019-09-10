using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySettings : MonoBehaviour
{

    public enum DifficultyLevel
    {
        Standard,
        Expert
    }
    
    public GameObject[] fallingArrows;         // stores all potential arrows in the scene
    public DifficultyLevel level;              // set externally

    string tagToDelete;                        // all objects tagged with the incorrect difficulty are removed

    // Start is called before the first frame update
    void Start()
    {
        switch (level)
        {
            case DifficultyLevel.Standard:
                tagToDelete = "Expert";
                break;

            case DifficultyLevel.Expert:
                tagToDelete = "Standard";
                break;
        }

        for (int i = 0; i < fallingArrows.Length; i++)
        {
            if (fallingArrows[i].tag == tagToDelete)
            {
                Destroy(fallingArrows[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
