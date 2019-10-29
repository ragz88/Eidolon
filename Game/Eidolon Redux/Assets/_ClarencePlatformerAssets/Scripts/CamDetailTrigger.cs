using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDetailTrigger : MonoBehaviour
{

    public enum DetailNumber
    {
        Detail1,
        Detail2
    }

    public Animator camAnim;

    public DetailNumber detNum;

    public bool deleteOnExit = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (detNum == DetailNumber.Detail1)
            {
                camAnim.SetBool("Detail1", true);
                camAnim.SetBool("Detail2", false);
                camAnim.SetBool("inEndArea", false);
            }
            else
            {
                camAnim.SetBool("Detail2", true);
                camAnim.SetBool("Detail1", false);
                camAnim.SetBool("inEndArea", false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (detNum == DetailNumber.Detail1)
            {
                camAnim.SetBool("Detail1", false);
            }
            else
            {
                camAnim.SetBool("Detail2", false);
            }

            if (deleteOnExit)
            {
                Destroy(gameObject);
            }
        }
    }
}
