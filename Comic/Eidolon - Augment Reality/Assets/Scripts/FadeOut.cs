using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{

    public SpriteRenderer fadeRend;

    public float waitTime = 5;
    public float fadeSpeed;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > waitTime)
        {
            if (fadeRend.color.a < 1)
            {
                fadeRend.color = new Color(fadeRend.color.r, fadeRend.color.g, fadeRend.color.b, fadeRend.color.a - (fadeSpeed * Time.deltaTime));
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
