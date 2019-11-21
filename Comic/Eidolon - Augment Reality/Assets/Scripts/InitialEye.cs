using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialEye : MonoBehaviour
{
    public Text title;
    public Text titleE;

    //public Text subTitle;

    public Text innerEye;
    public SpriteRenderer eyeRend;

    public Color newTextColour;

    public float fadeSpeed = 1f;
    public float waitTime = 1f;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < waitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            eyeRend.color = new Color(eyeRend.color.r, eyeRend.color.g, eyeRend.color.b, eyeRend.color.a + (fadeSpeed * Time.deltaTime));
            innerEye.color = new Color(innerEye.color.r, innerEye.color.g, innerEye.color.b, innerEye.color.a + (fadeSpeed * Time.deltaTime));
            title.color = Color.Lerp(title.color, newTextColour, fadeSpeed * Time.deltaTime);
            titleE.color = Color.Lerp(titleE.color, newTextColour, fadeSpeed * Time.deltaTime);
            //subTitle.color = Color.Lerp(subTitle.color, newTextColour, fadeSpeed * Time.deltaTime);
        }
    }
}
