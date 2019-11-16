using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public bool beginFade = false;
    public float fadeSpeed = 0.5f;

    Renderer[] rends;
    Renderer rend;

    MaterialTreadmill treadmill;

    // Start is called before the first frame update
    void Start()
    {
        // we need to remove this gameobject from the return of the function below
        rends = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i] != GetComponent<Renderer>())
            {
                rend = rends[i];
                break;
            }
        }

        treadmill = GameObject.Find("Ground").GetComponent<MaterialTreadmill>();
    }

    // Update is called once per frame
    void Update()
    {
        if (beginFade)
        {
            rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 
                rend.material.color.a - fadeSpeed * Time.deltaTime * Mathf.Abs(treadmill.ySpeed));
            if (rend.material.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
