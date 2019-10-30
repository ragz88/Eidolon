using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public Transform spawnPoint;

    //public bool sceneChangerHeightMod;
    //public SceneChange sceneChanger;
    //public float yAdjustment = 0;
    //public float xAdjustment = 0;

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
            /*if (sceneChangerHeightMod)
            {
                sceneChanger.yAdjustment = yAdjustment;
                sceneChangerHeightMod = false;
            }
            */
            SceneChange.spawnPoint = new Vector3(spawnPoint.position.x, spawnPoint.position.y, other.transform.position.z);
            other.gameObject.GetComponent<ClarenceMovement>().LoseLife();
            
        }
    }
}
