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

    public SpriteRenderer[] spriteRends;
    public Renderer[] rends;

    float alphaVal = 0;

    public float fadeSpeed = 1f;

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

        // reset fade of all renderers
        alphaVal = 0;

        for (int i = 0; i < spriteRends.Length; i++)
        {
            spriteRends[i].color = new Color(spriteRends[i].color.r, spriteRends[i].color.g, spriteRends[i].color.b, 0);
        }

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = new Color(rends[i].material.color.r, rends[i].material.color.g, rends[i].material.color.b, 0);
        }
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

            // set the renderers to transparrent
            alphaVal = 0;

            for (int i = 0; i < spriteRends.Length; i++)
            {
                spriteRends[i].color = new Color(spriteRends[i].color.r, spriteRends[i].color.g, spriteRends[i].color.b, 0);
            }

            for (int i = 0; i < rends.Length; i++)
            {
                rends[i].material.color = new Color(rends[i].material.color.r, rends[i].material.color.g, rends[i].material.color.b, 0);
            }
        }

        // fade in sprites
        if (alphaVal < 1)
        {
            if (rend.enabled)
            {
                alphaVal = Mathf.Lerp(alphaVal, 1.15f, fadeSpeed * Time.deltaTime);

                for (int i = 0; i < spriteRends.Length; i++)
                {
                    spriteRends[i].color = new Color(spriteRends[i].color.r, spriteRends[i].color.g, spriteRends[i].color.b, alphaVal);
                }

                for (int i = 0; i < rends.Length; i++)
                {
                    rends[i].material.color = new Color(rends[i].material.color.r, rends[i].material.color.g, rends[i].material.color.b, alphaVal);
                }
            }
        }
    }
}
