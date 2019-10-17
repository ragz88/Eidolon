using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public SceneChange sceneChanger;
    public Animator endCamStateMachine;

    Transform clarenceTrans;

    bool clarencePresent = false;

    private void Start()
    {
        endCamStateMachine = GetComponent<Animator>();
    }

    private void Update()
    {
        if (clarencePresent)
        {
            if (Mathf.Abs(clarenceTrans.position.x - transform.position.x) < 0.1f)
            {
                clarenceTrans.GetComponent<ClarenceMovement>().enabled = false;
                sceneChanger.NextScene();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            clarencePresent = true;
            endCamStateMachine.SetBool("inEndArea", true);
            clarenceTrans = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            endCamStateMachine.SetBool("inEndArea", false);
            clarencePresent = false;
        }
    }
}
