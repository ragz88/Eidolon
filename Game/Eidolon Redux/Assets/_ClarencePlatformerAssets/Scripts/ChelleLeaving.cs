using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChelleLeaving : MonoBehaviour
{
    public Transform chelleTrans;
    public MichelleMovement chelleMovement;

    public Transform targetPoint;

    bool lerping = false;

    public float lerpSpeed = 2f;

    public GameObject[] objectsToDelete;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            chelleTrans.position = Vector3.Lerp(chelleTrans.position, targetPoint.position, lerpSpeed * Time.deltaTime);

            if (Mathf.Abs(chelleTrans.position.y - targetPoint.position.y) < 0.3f)
            {
                for (int i = 0; i < objectsToDelete.Length; i++)
                {
                    Destroy(objectsToDelete[i]);
                }

                Destroy(chelleTrans.gameObject);

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            lerping = true;
            chelleMovement.enabled = false;
        }
    }
}
