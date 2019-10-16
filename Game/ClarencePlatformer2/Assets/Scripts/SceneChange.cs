using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    public enum ShutterState
    {
        Inactive,
        Opening,
        Closing
    }

    public ShutterState shutterState = ShutterState.Opening;

    public static Vector3 spawnPoint;
    public static int currentSceneNumber = 0;

    public Transform[] topBars;
    public Transform[] botBars;

    //public Transform[] topBarsEnd;
    //public Transform[] botBarsEnd;

    public float delayBetweenBars = 0.2f; // pause between individual bars
    public float barSpeed = 2f;           // how fast bars close and open

    public float yOffset = 5f;            // how far away the open position is from the closed position

    float[] initTopYValues;               // closed y value for top bars
    float[] finalTopYValues;              // open y values for top bars

    float[] initBotYValues;               // closed y value for bottom bars
    float[] finalBotYValues;              // open y values for bottom bars


    /*float[] initEndTopYValues;               // closed y value for top bars
    float[] finalEndTopYValues;              // open y values for top bars

    float[] initEndBotYValues;               // closed y value for bottom bars
    float[] finalEndBotYValues;        */      // open y values for bottom bars

    float currentMovingBar;               // used to implement delay
    float InitialTimeStamp;               // stores time when bar movement begins

    bool barsEnabled = false;

    int sceneToLoad = 0;

    public ClarenceMovement clarence;

    public int GameOverSceneNumber = 0;

    public Animator[] heartAnims;

    bool clarenceActivated = false;

    Vector3 initSpawnPos;


    // Start is called before the first frame update
    void Start()
    {
        // opening arrays
        initTopYValues  = new float[topBars.Length];
        finalTopYValues = new float[topBars.Length];

        initBotYValues  = new float[botBars.Length];
        finalBotYValues = new float[botBars.Length];


        initSpawnPos = new Vector3(transform.position.x, transform.position.y, clarence.transform.position.z);      //here's the problem bro.

        if (currentSceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            spawnPoint = initSpawnPos;
        }

        clarence.transform.position = spawnPoint;

        //here we initialise these arrays to store the positions we'll be lerping to and from
        for (int i = 0; i < topBars.Length; i++)
        {
            initTopYValues[i]  = topBars[i].position.y;
            finalTopYValues[i] = topBars[i].position.y + yOffset;
        }

        for (int i = 0; i < botBars.Length; i++)
        {
            initBotYValues[i]  = botBars[i].position.y;
            finalBotYValues[i] = botBars[i].position.y - yOffset;
        }

        InitialTimeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (shutterState == ShutterState.Opening)
        {
            // make sure bars are visible
            if (!barsEnabled)
            {
                for (int i = 0; i < topBars.Length; i++)
                {
                    topBars[i].gameObject.SetActive(true);
                }

                for (int i = 0; i < botBars.Length; i++)
                {
                    botBars[i].gameObject.SetActive(true);
                }

                barsEnabled = true;

                //This corrects our heart animation state
                CorrectHeartAnimation();
            }

            bool topDone = false;
            bool botDone = false;

            // here we move bars into position
            for (int i = 0; i < topBars.Length; i++)
            {
                if (Time.time >= (InitialTimeStamp + (i*delayBetweenBars)))
                {
                    topBars[i].position = Vector3.Lerp(topBars[i].position, 
                        new Vector3(topBars[i].position.x, finalTopYValues[i], topBars[i].position.z), barSpeed * Time.deltaTime);
                }

                // checks if last bar is in end position
                if (i == topBars.Length - 1 && (topBars[i].position.y - finalTopYValues[i]) < 0.1f)
                {
                    topDone = true;
                }
            }

            for (int i = 0; i < botBars.Length; i++)
            {
                if (Time.time >= (InitialTimeStamp + (i * delayBetweenBars)))
                {
                    botBars[i].position = Vector3.Lerp(botBars[i].position,  
                        new Vector3(botBars[i].position.x, finalBotYValues[i], botBars[i].position.z), barSpeed * Time.deltaTime);
                }

                // checks if last bar is in end position
                if (i == botBars.Length - 1 && (botBars[i].position.y - finalBotYValues[i]) < 0.1f)
                {
                    botDone = true;
                }

                // here we allow Clarence to move again, when the bars are nearly done moving
                if (i == 0 && (botBars[i].position.y - finalBotYValues[i]) < 0.1f && !clarenceActivated)
                {
                    clarence.enabled = true;
                    clarenceActivated = true;
                }

            }

            if (topDone && botDone)
            {
                shutterState = ShutterState.Inactive;
            }


        }
        else if (shutterState == ShutterState.Closing)
        {
            // make sure bars are visible
            if (!barsEnabled)
            {
                for (int i = 0; i < topBars.Length; i++)
                {
                    topBars[i].gameObject.SetActive(true);
                }

                for (int i = 0; i < botBars.Length; i++)
                {
                    botBars[i].gameObject.SetActive(true);
                }

                barsEnabled = true;
            }

            bool topDone = false;
            bool botDone = false;

            // here we move bars into position
            for (int i = 0; i < topBars.Length; i++)
            {
                if (Time.time >= (InitialTimeStamp + (i * delayBetweenBars)))
                {
                    topBars[i].position = Vector3.Lerp(topBars[i].position, 
                        new Vector3(topBars[i].position.x, initTopYValues[i], topBars[i].position.z), barSpeed * Time.deltaTime);
                }

                // checks if last bar is in end position
                if (i == topBars.Length - 1 && (topBars[i].position.y - initTopYValues[i]) < 0.1f)
                {
                    topDone = true;
                }
            }

            for (int i = 0; i < botBars.Length; i++)
            {
                if (Time.time >= (InitialTimeStamp + (i * delayBetweenBars)))
                {
                    botBars[i].position = Vector3.Lerp(botBars[i].position, 
                        new Vector3(botBars[i].position.x, initBotYValues[i], botBars[i].position.z), barSpeed * Time.deltaTime);
                }

                // checks if last bar is in end position
                if (i == botBars.Length - 1 && (botBars[i].position.y - initBotYValues[i]) < 0.1f)
                {
                    botDone = true;
                }
            }

            if (topDone && botDone)
            {
                SceneManager.LoadScene(sceneToLoad);
            }

        }
        else if (shutterState == ShutterState.Inactive)
        {
            // make sure bars are visible
            if (barsEnabled)
            {
                for (int i = 0; i < topBars.Length; i++)
                {
                    topBars[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < botBars.Length; i++)
                {
                    botBars[i].gameObject.SetActive(false);
                }

                barsEnabled = false;
            }

            InitialTimeStamp = Time.time;
        }
    }

    public void LoadScene(int sceneToLoad)
    {
        this.sceneToLoad = sceneToLoad;
        shutterState = ShutterState.Closing;
    }

    public void RestartScene()
    {
        sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        shutterState = ShutterState.Closing;
    }

    public void NextScene()
    {
        sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        shutterState = ShutterState.Closing;
    }

    public void GameOver()
    {
        sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        SceneChange.spawnPoint = initSpawnPos;
        ClarenceGameController.health = 3;
        shutterState = ShutterState.Closing;
    }

    void CorrectHeartAnimation()
    {
        for (int i = 0; i < heartAnims.Length; i++)
        {
            if (ClarenceGameController.health <= i)
            {
                heartAnims[i].SetBool("HeartFull", false);
                heartAnims[i].Play("BrokenHeart");
            }
        }
    }
}
