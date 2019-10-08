using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    GameObject scoreManager;

    public string playerName = "EM";
    public float nameTypeTime = 2;

    public Transform underline;
    public Text nameText;

    public GameObject[] enterNameObjects;
    public GameObject[] highScoreObjects;


    float timer = 0;
    float initUnderlineX;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager");
        
        initUnderlineX = underline.position.x;

        for (int i = 0; i < highScoreObjects.Length; i++)
        {
            highScoreObjects[i].SetActive(false);
        }

        SceneActivator sceneChanger = GameObject.Find("SceneChanger(Clone)").GetComponent<SceneActivator>();

        sceneChanger.finishedStandard = true;         // unlocks expert mode 

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > (0.3f * nameTypeTime) && timer <= (0.35f * nameTypeTime))   // this is an initial delay - to make things seem natural. We enter the first letter
        {
            nameText.text = playerName[0].ToString();
        }
        else if (timer > (0.35f * nameTypeTime) && timer <= (0.5f * nameTypeTime))  // we move the cursor
        {
            underline.transform.position = new Vector3(0, underline.transform.position.y, underline.transform.position.z);
        }
        else if (timer > (0.5f * nameTypeTime) && timer <= (0.75f * nameTypeTime))  // we type the next letter
        {
            nameText.text = playerName[0].ToString() + playerName[1].ToString();
        }
        else if (timer > (0.75f * nameTypeTime) && timer <= (nameTypeTime)) // move cursor
        {
            underline.transform.position = new Vector3(-initUnderlineX, underline.transform.position.y, underline.transform.position.z);
        }
        else if (timer > nameTypeTime && timer <= (1.05f * nameTypeTime)) // enter name (delete input field and title)
        {
            for (int i = 0; i < enterNameObjects.Length; i++)
            {
                Destroy(enterNameObjects[i]);
            }
        }
        else if (timer > (1.05f * nameTypeTime) && timer <= (1.1f * nameTypeTime)) // show high scores
        {
            for (int i = 0; i < highScoreObjects.Length; i++)
            {
                highScoreObjects[i].SetActive(true);
            }
        }

        timer += Time.deltaTime;
    }

    public void ReturnToMenu()
    {
        if (scoreManager!= null)
        {
            Destroy(scoreManager.gameObject);
        }
        
        SceneManager.LoadScene("Opening Scene");
    }

}
