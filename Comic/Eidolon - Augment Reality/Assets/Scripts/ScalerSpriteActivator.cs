using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerSpriteActivator : MonoBehaviour
{
    public enum CompareMode
    {
        GreaterThan,
        SmallerThan
    }

    public CompareMode compMode = CompareMode.GreaterThan;

    public Vector3Ranger ranger;

    public SpriteRenderer rend;

    public float threshhold = 0.5f;

    public float fadeSpeed = 1f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (compMode == CompareMode.GreaterThan)
        {
            if (ranger.currentPercent > threshhold && rend.color.a < 1)
            {
                rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a + (fadeSpeed * Time.deltaTime));
            }
            else if (rend.color.a > 0)
            {
                rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a - (fadeSpeed*Time.deltaTime));
            }
        }
        else
        {
            if (ranger.currentPercent < threshhold && rend.color.a < 1)
            {
                rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a + (fadeSpeed * Time.deltaTime));
            }
            else if (rend.color.a > 0)
            {
                rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a - (fadeSpeed * Time.deltaTime));
            }
        }
    }
}
