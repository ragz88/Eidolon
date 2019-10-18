using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDRScoreManager : MonoBehaviour
{
    public enum HitQuality
    {
       Miss,
       Almost,
       Good,
       Great,
       Perfect
    }

    public Image comboBar;
    public Text scoreText;
    public AudioSyncScale scaleBeatDetector;

    public Text keyWordText;                    // text at the centre of the screen



    // these define how much each arrow type is worth
    [Header("Arrow Values")]
    public int standardArrowScore = 10;
    public int longArrowScore = 50;                     // note, this value is multiplied by the percentage of the arrow that is hit
    public int flippingArrowScore = 20;


    

    // these values define how each type of hit effects the comboPercent
    [Header("Combo Effect of Hit Qualities")]
    public float missEffect = -0.15f;
    public float almostEffect = -0.07f;
    public float greatEffect = 0.02f;
    public float perfectEffect = 0.07f;

   
    // Words to be shown depending on timing of press                          probs use getters and setters
    // and their colours
    [HideInInspector] public string perfectString = "Perfect!";
    [HideInInspector] public Color perfectColour;
    [HideInInspector] public string greatString = "Great!";
    [HideInInspector] public Color greatColour;
    [HideInInspector] public string goodString = "Good";
    [HideInInspector] public Color goodColour;
    [HideInInspector] public string almostString = "Almost";
    [HideInInspector] public Color almostColour;
    [HideInInspector] public string missString = "BOO!";
    [HideInInspector] public Color missColour;

    // colours for the combo bar depending on whether it's full or not
    public Color bonusColour;
    Color standardColour;


    public float comboPercent = 0.5f;                   // this value relates to the combo-meter - when it's 1 the player gets significant bonuses on score
    public float fullBonusMultiplier = 1.5f;            // bonus player gets when bonus bar is full

    Vector3 initBarScale;                               // used to return bar after beat detection augments its shapes
    bool lerpBarBack = false;                           // when true, the comboBar lerps to it's initial scale;
    float returnSpeed = 2f;


    int score = 0;                   // stores the score of single DDR game

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);     // makes this persist, allowing us to access the score on an end game scene

        standardColour = comboBar.color;
        initBarScale = comboBar.transform.localScale;
    }

    private void Update()
    {
        if (lerpBarBack)
        {
            comboBar.transform.localScale = Vector3.Lerp(comboBar.transform.localScale, initBarScale, returnSpeed*Time.deltaTime);
            if (comboBar.transform.localScale == initBarScale)
            {
                lerpBarBack = false;
            }
        }
    }


    // change combo meter based on quality of a hit
    void UpdateComboPercent(HitQuality hitQuality)
    {
        // update combo percent using publicly editable floats
        switch (hitQuality)
        {
            case HitQuality.Miss:
                comboPercent += missEffect;
                break;

            case HitQuality.Almost:
                comboPercent += almostEffect;
                break;

            case HitQuality.Great:
                comboPercent += greatEffect;
                break;

            case HitQuality.Perfect:
                comboPercent += perfectEffect;
                break;
        }

        // ensure that the value remains between 1 and 0
        // additionally, this changes our colour and activates beat detection when the combo meter is full and reverts these properties otherwise
        if (comboPercent < 0)
        {
            comboPercent = 0;
            if (comboBar.color == bonusColour)
            {
                comboBar.color = standardColour;
                scaleBeatDetector.enabled = false;
                lerpBarBack = true;
            }
        }
        else if (comboPercent >= 1)
        {
            comboPercent = 1;
            comboBar.color = bonusColour;
            scaleBeatDetector.enabled = true;
            lerpBarBack = false;
        }
        else
        {
            if (comboBar.color == bonusColour)
            {
                comboBar.color = standardColour;
                scaleBeatDetector.enabled = false;
                lerpBarBack = true;
            }
        }

        comboBar.fillAmount = comboPercent;              // update the bar's fill level based on combo percent

        DisplayHitQuality(hitQuality);                   // Shows the quality of our hit on our on - screen Keyword

    }

    // adds score based on arrow type and hit quality
    public void processStandardArrowHit(HitQuality hitQuality)
    {
        float scoreIncrement = standardArrowScore;

        UpdateComboPercent(hitQuality);                  // make relevant changes to our combo percent and bar

        if (comboPercent == 1)
        {
            scoreIncrement *= fullBonusMultiplier;
        }

        if (hitQuality == HitQuality.Miss)
        {
            scoreIncrement = 0;
        }

        ChangeScore(Mathf.RoundToInt(scoreIncrement));
    }


    // adds score based on arrow's length and hit quality
    public void processLongArrowHit(HitQuality hitQuality)
    {
        float scoreIncrement = longArrowScore;

        // adds score based on how much of the long arrow was hit
        switch (hitQuality)
        {
            case HitQuality.Miss:
                scoreIncrement = 0;
                break;

            case HitQuality.Almost:
                scoreIncrement = longArrowScore * 0.2f;
                break;

            case HitQuality.Good:
                scoreIncrement = longArrowScore * 0.5f;
                break;

            case HitQuality.Great:
                scoreIncrement = longArrowScore * 0.75f;
                break;

            case HitQuality.Perfect:
                scoreIncrement = longArrowScore;
                break;
        }

        UpdateComboPercent(hitQuality);                  // make relevant changes to our combo percent and bar

        if (comboPercent == 1)
        {
            scoreIncrement *= fullBonusMultiplier;
        }

        if (hitQuality == HitQuality.Miss)
        {
            scoreIncrement = 0;
        }

        ChangeScore(Mathf.RoundToInt(scoreIncrement));
    }


    // adds score based on arrow type and hit qualitys
    public void processFlipArrowHit(HitQuality hitQuality)
    {
        float scoreIncrement = flippingArrowScore;

        UpdateComboPercent(hitQuality);                  // make relevant changes to our combo percent and bar

        if (comboPercent == 1)
        {
            scoreIncrement *= fullBonusMultiplier;
        }

        if (hitQuality == HitQuality.Miss)
        {
            scoreIncrement = 0;
        }

        ChangeScore(Mathf.RoundToInt(scoreIncrement));
    }


    public void ChangeScore(int increment)
    {
        score += increment;
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public void DisplayHitQuality(HitQuality scoreType)
    {
        switch (scoreType)
        {
            case HitQuality.Perfect:
                keyWordText.text = perfectString;
                keyWordText.color = perfectColour;
                break;

            case HitQuality.Great:
                keyWordText.text = greatString;
                keyWordText.color = greatColour;
                break;

            case HitQuality.Good:
                keyWordText.text = goodString;
                keyWordText.color = goodColour;
                break;

            case HitQuality.Almost:
                keyWordText.text = almostString;
                keyWordText.color = almostColour;
                break;

            case HitQuality.Miss:
                keyWordText.text = missString;
                keyWordText.color = missColour;
                break;
        }
    }

    public void SetKeyWords(string miss, string almost, string good, string great, string perfect)
    {
        missString = miss;
        almostString = almost;
        goodString = good;
        greatString = great;
        perfectString = perfect;
    }

    public void SetKeyWordColours(Color miss, Color almost, Color good, Color great, Color perfect)
    {
        missColour = miss;
        almostColour = almost;
        goodColour = good;
        greatColour = great;
        perfectColour = perfect;
    }

    public void StoreHighScore()
    {

    }

}
