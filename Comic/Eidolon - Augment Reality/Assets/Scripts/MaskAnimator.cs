using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskAnimator : MonoBehaviour
{
    public SpriteRenderer rend;
    public SpriteMask maskRend;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        maskRend.sprite = rend.sprite;
    }
}
