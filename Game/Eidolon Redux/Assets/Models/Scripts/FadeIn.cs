using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    SpriteRenderer rend;

    public float fadeSpeed = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rend.color.a > 0)
        {
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a - fadeSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(this);
        }
    }
}
