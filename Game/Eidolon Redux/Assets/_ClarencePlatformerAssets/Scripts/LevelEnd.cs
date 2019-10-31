using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public SceneChange sceneChanger;
    public Animator endCamStateMachine;

    public bool finalScene = false;

    Transform clarenceTrans;

    bool clarencePresent = false;

    public int menuScene;

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

                if (finalScene)
                {
                    sceneChanger.LoadScene(menuScene);
                }
                else
                {
                    sceneChanger.NextScene();
                }
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
