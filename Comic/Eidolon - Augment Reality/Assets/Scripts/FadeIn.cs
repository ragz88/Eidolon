using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour
{

    public SpriteRenderer fadeRend;
    public float fadeSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeRend.color.a > 0)
        {
            fadeRend.color = new Color(fadeRend.color.r, fadeRend.color.g, fadeRend.color.b, fadeRend.color.a - (fadeSpeed * Time.deltaTime));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
