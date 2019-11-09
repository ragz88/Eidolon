using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunnerScoreManager : MonoBehaviour
{
    public AudioSource audio1;
    public AudioSource audio2;

    float currentPitch = 0;
    float initPitch = 0;

    float timeSinceLastFly = 0;
    public float audioResetTime = 0.6f;
    public float pitchIncrements = 0.05f;

    int score = 0;

    public Text[] scoreTexts;
    bool[] scoreTextBouncing;

    public float textBounceAmount = 0.2f;
    public float textBounceSpeed = 1f;

    float initYValue;

    // Start is called before the first frame update
    void Start()
    {
        initPitch = audio1.pitch;
        currentPitch = initPitch;

        scoreTextBouncing = new bool[scoreTexts.Length];

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            scoreTexts[i].text = "0";
            scoreTextBouncing[i] = false;
        }

        initYValue = scoreTexts[0].transform.position.y;



    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastFly += Time.deltaTime;

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTextBouncing[i] == true)
            {
                // bounce this digit
                if (Mathf.Abs(scoreTexts[i].transform.position.y - (initYValue + textBounceAmount)) > 0.01f)
                {
                    scoreTexts[i].transform.position = Vector3.MoveTowards(scoreTexts[i].transform.position, 
                        scoreTexts[i].transform.position + new Vector3(0, (initYValue + textBounceAmount), 0), textBounceSpeed * Time.deltaTime);
                }
                else
                {
                    scoreTextBouncing[i] = false;
                }
            }
            else
            {
                // bring it back to it's initial pos
                if (Mathf.Abs(scoreTexts[i].transform.position.y - initYValue ) > 0.01f)
                {
                    scoreTexts[i].transform.position = Vector3.MoveTowards(scoreTexts[i].transform.position,
                        new Vector3(scoreTexts[i].transform.position.x , initYValue, scoreTexts[i].transform.position.z), textBounceSpeed * Time.deltaTime);
                }
            }
        }
    }

    public void FlyCaught()
    {
        // increase score
        score++;

        // update score text
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i].text != ( (score / (int)Mathf.Pow(10, i)) % 10 ).ToString())    // this formula separates each individual digit
            {
                scoreTexts[i].text = ((score / (int)Mathf.Pow(10, i)) % 10).ToString();
                scoreTextBouncing[i] = true;
            }
        }

        
        

        // Change sound if multiple caught in a row
        if (timeSinceLastFly < audioResetTime)
        {
            currentPitch += pitchIncrements;
        }
        else
        {
            currentPitch = initPitch;
        }

        audio1.pitch = currentPitch;
        audio2.pitch = currentPitch;


        // this prevents sounds from being cut out
        if (!audio1.isPlaying)
        {
            audio1.Play();
        }
        else
        {
            audio2.Play();
        }

        timeSinceLastFly = 0;
    }
}
