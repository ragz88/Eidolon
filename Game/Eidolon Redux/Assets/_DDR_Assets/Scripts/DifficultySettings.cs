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
    public DDRScoreManager scoreManager;       // for adjusting scores, multipliers and diction based on difficulty

    string tagToDelete;                        // all objects tagged with the incorrect difficulty are removed

    public float scoreAdjustment = -0.02f;
    public float comboAdjustment = 1.1f;


    // Words to be shown depending on timing of press
    // and their colours
    // these get sent to the ScoreManager
    [Header("Standard Key Words")]
    [SerializeField] string perfectStringStd = "Perfect!";
    [SerializeField] Color perfectColourStd;
    [SerializeField] string greatStringStd = "Great!";
    [SerializeField] Color greatColourStd;
    [SerializeField] string goodStringStd = "Good";
    [SerializeField] Color goodColourStd;
    [SerializeField] string almostStringStd = "Almost";
    [SerializeField] Color almostColourStd;
    [SerializeField] string missStringStd = "Oops...";
    [SerializeField] Color missColourStd;

    [Header("Expert Key Words")]
    [SerializeField] string perfectStringExpert = "Acceptable.";
    [SerializeField] Color perfectColourExpert;
    [SerializeField] string greatStringExpert = "You're off.";
    [SerializeField] Color greatColourExpert;
    [SerializeField] string goodStringExpert = "Try Harder.";
    [SerializeField] Color goodColourExpert;
    [SerializeField] string almostStringExpert = "Wrong.";
    [SerializeField] Color almostColourExpert;
    [SerializeField] string missStringExpert = "Pathetic.";
    [SerializeField] Color missColourExpert;

    // Start is called before the first frame update
    void Start()
    {
        // the following checks if a sceneActivator gameobject is present, implying that the difficulty was already chosen in a different scene (if not, we use a default).
        GameObject sceneActObj = GameObject.Find("SceneChanger(Clone)");                     // temp variable to prevent null reference errors
        if (sceneActObj != null)
        {
            SceneActivator sceneActivator = sceneActObj.GetComponent<SceneActivator>();
            level = sceneActivator.difficulty;
            SceneActivator.finishedStandard = true;
            
        }

        //here we define which arrow types should be deleted and set our keywords and colours tht should appear based on difficulty level
        switch (level)
        {
            case DifficultyLevel.Standard:
                tagToDelete = "Expert";
                scoreManager.SetKeyWords(missStringStd, almostStringStd, goodStringStd, greatStringStd, perfectStringStd);
                scoreManager.SetKeyWordColours(missColourStd, almostColourStd, goodColourStd, greatColourStd, perfectColourStd);
                scoreManager.bonusColour = perfectColourStd;
                break;

            case DifficultyLevel.Expert:
                tagToDelete = "Standard";
                scoreManager.SetKeyWords(missStringExpert, almostStringExpert, goodStringExpert, greatStringExpert, perfectStringExpert);
                scoreManager.SetKeyWordColours(missColourExpert, almostColourExpert, goodColourExpert, greatColourExpert, perfectColourExpert);
                scoreManager.bonusColour = perfectColourExpert;
                scoreManager.fullBonusMultiplier = comboAdjustment;
                scoreManager.missEffect += scoreAdjustment;
                scoreManager.almostEffect += scoreAdjustment;
                scoreManager.greatEffect += scoreAdjustment;
                scoreManager.perfectEffect += scoreAdjustment;
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
