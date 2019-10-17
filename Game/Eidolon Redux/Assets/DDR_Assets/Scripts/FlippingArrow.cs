using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippingArrow : MonoBehaviour
{

    public float fallSpeed = 1;
    public float destroyHeight = -6f;          // at this y position, arrow is automatically destroyed (if missed)
    //public float arrowAlphaAdjustment = 50;  // change in arrow opacity after miss

    public float flipHeight = 2;               // Height at which arrow will return to original lane 
    public Transform targetSensor;             // sensor the arrow should return to
    public float flipSpeed = 1;                // speed at which the arrow returns to it's lane

    SpriteRenderer arrowSpriteRend;

    // Start is called before the first frame update
    void Start()
    {
        arrowSpriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -fallSpeed * Time.deltaTime, 0), Space.World);

        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }

        if (transform.position.y < flipHeight && (transform.position.x != targetSensor.position.x || transform.rotation != Quaternion.identity))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetSensor.position.x, transform.position.y, transform.position.z), flipSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, flipSpeed*Time.deltaTime);
        }

        //Old opacity changing code. May still include later.
        /*if (gameObject.layer == LayerMask.NameToLayer("MissedArrow") && arrowSpriteRend.color.a > 0.8f)
        {
            arrowSpriteRend.color = new Color(arrowSpriteRend.color.r, arrowSpriteRend.color.g, arrowSpriteRend.color.b, arrowSpriteRend.color.a - arrowAlphaAdjustment);
        }*/
    }

}
