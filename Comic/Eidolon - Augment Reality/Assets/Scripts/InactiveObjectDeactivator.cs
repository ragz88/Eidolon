using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveObjectDeactivator : MonoBehaviour
{
    // this class examines a specific renderer that the vuforia engine will deactivate
    // thus it turns off specific gameObjects to save processing power when they wouldn't be visible

    public SpriteRenderer rend;             // this is enabled and disabled by vuforia

    public GameObject[] objectsToActivate;
    bool rendActive = false;

    public VibratePattern[] vibrators;

    // Start is called before the first frame update
    void Start()
    {
        rendActive = false;
        Vibration.Cancel();

        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            objectsToActivate[i].SetActive(false);
        }

        Vibration.Cancel();
    }

    // Update is called once per frame
    void Update()
    {
        if (rend.enabled && !rendActive)
        {
            rendActive = true;

            for (int i = 0; i < vibrators.Length; i++)
            {
                vibrators[i].ResetVibrator();
            }

            for (int i = 0; i < objectsToActivate.Length; i++)
            {
                objectsToActivate[i].SetActive(true);
            }
        }
        else if (!rend.enabled && rendActive)
        {
            rendActive = false;
            Vibration.Cancel();

            for (int i = 0; i < objectsToActivate.Length; i++)
            {
                objectsToActivate[i].SetActive(false);
            }
        }
    }
}
