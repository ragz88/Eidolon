using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardFallingArrow : MonoBehaviour
{

    public float fallSpeed = 1;
    public float destroyHeight = -6f;        // at this y position, arrow is automatically destroyed (if missed)
    //public float arrowAlphaAdjustment = 50;  // change in arrow opacity after miss

    SpriteRenderer arrowSpriteRend;

    // Start is called before the first frame update
    void Start()
    {
        arrowSpriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    /*private void FixedUpdate()
    {
        transform.Translate(0, -fallSpeed * Time.fixedDeltaTime, 0);
    }*/

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -fallSpeed * Time.deltaTime, 0);

        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }

        /*if (gameObject.layer == LayerMask.NameToLayer("MissedArrow") && arrowSpriteRend.color.a > 0.8f)
        {
            arrowSpriteRend.color = new Color(arrowSpriteRend.color.r, arrowSpriteRend.color.g, arrowSpriteRend.color.b, arrowSpriteRend.color.a - arrowAlphaAdjustment);
        }*/
    }

}
