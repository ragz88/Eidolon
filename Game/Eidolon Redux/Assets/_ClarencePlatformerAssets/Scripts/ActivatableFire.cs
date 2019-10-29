using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableFire : MonoBehaviour
{
    public bool autoInitialiseHeightCurve = true;
    public float initialSize = 1;

    public AnimationCurve heightCurve;
    public Transform[] flames;

    public float heightAdjustment = 0;
    public float scaleThreshhold = 0.1f;  //scale at which the fire's collider is deactivated

    CircleCollider2D[] flameColliders;

    float[] initYVal;

    private void Awake()
    {
        flameColliders = new CircleCollider2D[flames.Length];

        initYVal = new float[flames.Length];


        //ensures height curve has no keys in it
        if (autoInitialiseHeightCurve)
        {
            for (int i = heightCurve.length - 1; i >= 0; i--)
            {
                heightCurve.RemoveKey(i);
            }
        }


        for (int i = 0; i < flames.Length; i++)
        {
            // Initialise the heights of each flame as 0
            if (autoInitialiseHeightCurve)
            {
                Keyframe newKey = new Keyframe((1f / flames.Length) * i, initialSize, 0, 0);
                heightCurve.AddKey(newKey);
            }

            // store reference to each flame's circle collider
            flameColliders[i] = flames[i].gameObject.GetComponent<CircleCollider2D>();

            //store each flames initial height
            initYVal[i] = flames[i].position.y;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFlameHeights();
    }

    public void UpdateFlameHeights()
    {
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].localScale = new Vector3(heightCurve[i].value, heightCurve[i].value, 0);
            flames[i].position = new Vector3(flames[i].position.x, ((heightCurve[i].value/2) + heightAdjustment) + initYVal[i], flames[i].position.z);

            if (flames[i].localScale.y <= scaleThreshhold)
            {
                flameColliders[i].enabled = false;
            }
            else
            {
                flameColliders[i].enabled = true;
            }
        }
    }
}
