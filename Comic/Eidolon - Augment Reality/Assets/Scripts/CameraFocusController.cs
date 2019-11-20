using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraFocusController : MonoBehaviour
{
    int TapCount = 0;
    float waitTime = 0.25f;
    float timer = 0;

    bool macroFocus = false;

    public SpriteRenderer flowerOn;
    public SpriteRenderer flowerOff;
    public SpriteRenderer focusBox;

    bool flowerOnVisible = false;
    bool flowerOffVisible = false;
    bool focusVisible = false;

    float imagePauseTime = 0.25f;

    float imageWaitTimer = 0;
    float imageFadeSpeed = 1f;

    bool imageVisible = false;


    // code from  Vuforia Developer Library
    // https://library.vuforia.com/articles/Solution/Camera-Focus-Modes
    void Start()
    {
        var vuforia = VuforiaARController.Instance;
        vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        vuforia.RegisterOnPauseCallback(OnPaused);
    }

    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(
            CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
               CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        }
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                TapCount += 1;
                timer = 0;
            }

            if (TapCount == 2)
            {
                TapCount = 0;
                SwitchCamType();
            }
        }

        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            if (TapCount == 1)
            {
                FocusCam();
                print("focus");
            }

            TapCount = 0;
        }

        if (imageVisible)
        {
            if (imageWaitTimer < imagePauseTime)
            {
                imageWaitTimer += Time.deltaTime;
            }
            else
            {
                if (flowerOnVisible)
                {
                    if (flowerOn.color.a > 0)
                    {
                        flowerOn.color = new Color(flowerOn.color.r, flowerOn.color.g, flowerOn.color.b, flowerOn.color.a - (imageFadeSpeed * Time.deltaTime));
                    }
                    else
                    {
                        flowerOn.gameObject.SetActive(false);
                        imageVisible = false;
                    }
                }
                else if (flowerOffVisible)
                {
                    if (flowerOff.color.a > 0)
                    {
                        flowerOff.color = new Color(flowerOff.color.r, flowerOff.color.g, flowerOff.color.b, flowerOff.color.a - (imageFadeSpeed * Time.deltaTime));
                    }
                    else
                    {
                        flowerOff.gameObject.SetActive(false);
                        imageVisible = false;
                    }
                }
                else if (focusVisible)
                {
                    if (focusBox.color.a > 0)
                    {
                        focusBox.color = new Color(focusBox.color.r, focusBox.color.g, focusBox.color.b, focusBox.color.a - (imageFadeSpeed * Time.deltaTime));
                    }
                    else
                    {
                        focusBox.gameObject.SetActive(false);
                        imageVisible = false;
                    }
                }
            }
        }
    }

    void FocusCam()
    {
        CameraDevice.Instance.SetFocusMode(
               CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);

        if (flowerOnVisible)
        {
            flowerOnVisible = false;
            flowerOn.color = new Color(flowerOn.color.r, flowerOn.color.g, flowerOn.color.b, 0);
            flowerOn.gameObject.SetActive(false);
        }

        if (flowerOffVisible)
        {
            flowerOffVisible = false;
            flowerOff.color = new Color(flowerOff.color.r, flowerOff.color.g, flowerOff.color.b, 0);
            flowerOff.gameObject.SetActive(false);
        }

        focusVisible = true;
        focusBox.gameObject.SetActive(true);
        focusBox.color = new Color(focusBox.color.r, focusBox.color.g, focusBox.color.b, 0.85f);
        imageWaitTimer = 0;
        imageVisible = true;
    }

    void SwitchCamType()
    {
        print("switch");
        if (macroFocus)
        {
            macroFocus = false;
            CameraDevice.Instance.SetFocusMode(
               CameraDevice.FocusMode.FOCUS_MODE_MACRO);

            if (flowerOnVisible)
            {
                flowerOnVisible = false;
                flowerOn.color = new Color(flowerOn.color.r, flowerOn.color.g, flowerOn.color.b, 0);
                flowerOn.gameObject.SetActive(false);
            }

            if (focusVisible)
            {
                focusVisible = false;
                focusBox.color = new Color(focusBox.color.r, focusBox.color.g, focusBox.color.b, 0);
                focusBox.gameObject.SetActive(false);
            }

            flowerOffVisible = true;
            flowerOff.gameObject.SetActive(true);
            flowerOff.color = new Color(flowerOff.color.r, flowerOff.color.g, flowerOff.color.b, 0.85f);
            imageWaitTimer = 0;
            imageVisible = true;
        }
        else
        {
            macroFocus = true;
            CameraDevice.Instance.SetFocusMode(
               CameraDevice.FocusMode.FOCUS_MODE_NORMAL);

            if (flowerOffVisible)
            {
                flowerOffVisible = false;
                flowerOff.color = new Color(flowerOff.color.r, flowerOff.color.g, flowerOff.color.b, 0);
                flowerOff.gameObject.SetActive(false);
            }

            if (focusVisible)
            {
                focusVisible = false;
                focusBox.color = new Color(focusBox.color.r, focusBox.color.g, focusBox.color.b, 0);
                focusBox.gameObject.SetActive(false);
            }

            flowerOnVisible = true;
            flowerOn.gameObject.SetActive(true);
            flowerOn.color = new Color(flowerOn.color.r, flowerOn.color.g, flowerOn.color.b, 0.85f);
            imageWaitTimer = 0;
            imageVisible = true;
        }
        
    }
}
