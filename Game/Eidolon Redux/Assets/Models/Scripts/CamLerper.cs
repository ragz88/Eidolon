using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ThirdPersonEngine.Runtime;

public class CamLerper : MonoBehaviour
{
    Transform mainCam;

    TPCamera tpCam;

    public enum CamState
    {
        Machine1,
        Machine2,
        Machine3,
        Standard
    }

    public CamState camState = CamState.Standard;

    public Transform EmTrans;

    public Transform[] camFirstPoints;
    public Transform[] camFinalPoints;

    public float lerpSpeed = 1f;

    public static Vector3 emPos;

    public SpriteRenderer fadingRend;
    public float fadeSpeed = 1;

    bool beginSceneLoad = false;
    bool doneFisrtLerp = false;

    static bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
        tpCam = mainCam.gameObject.GetComponent<TPCamera>();

        if (firstTime)
        {
            emPos = EmTrans.position;
            firstTime = false;
        }
        else if (emPos != null)
        {
            EmTrans.position = emPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (camState != CamState.Standard)
        {
            if (!beginSceneLoad)
            {
                if ((Vector3.Distance(mainCam.position, camFirstPoints[(int)camState].position) > 0.6f ||
                    Quaternion.Angle(mainCam.rotation, camFirstPoints[(int)camState].rotation) > 3f) && !doneFisrtLerp)
                {
                    mainCam.position = Vector3.Lerp(mainCam.position, camFirstPoints[(int)camState].position, lerpSpeed * Time.deltaTime);
                    mainCam.rotation = Quaternion.Slerp(mainCam.rotation, camFirstPoints[(int)camState].rotation, lerpSpeed * Time.deltaTime);
                }
                else if (Vector3.Distance(mainCam.position, camFinalPoints[(int)camState].position) > 0.05f ||
                    Quaternion.Angle(mainCam.rotation, camFinalPoints[(int)camState].rotation) > 0.15f)
                {
                    doneFisrtLerp = true;
                    mainCam.position = Vector3.Lerp(mainCam.position, camFinalPoints[(int)camState].position, lerpSpeed * Time.deltaTime);
                    mainCam.rotation = Quaternion.Slerp(mainCam.rotation, camFinalPoints[(int)camState].rotation, lerpSpeed * Time.deltaTime);
                }
                else
                {
                    beginSceneLoad = true;
                }
            }
            else
            {
                if (fadingRend.color.a < 1)
                {
                    fadingRend.color = new Color(fadingRend.color.r, fadingRend.color.g, fadingRend.color.b, fadingRend.color.a + fadeSpeed * Time.deltaTime);
                }
                else
                {
                    CamLerper.emPos = EmTrans.position;

                    switch (camState)
                    {
                        case CamState.Machine1:
                            SceneManager.LoadScene(1);
                            break;

                        case CamState.Machine2:
                            SceneManager.LoadScene(6);
                            break;

                        case CamState.Machine3:
                            SceneManager.LoadScene(9);
                            break;

                    }
                }
            }


            tpCam.enabled = false;
        }
        else
        {
            tpCam.enabled = true;
        }
    }
}
