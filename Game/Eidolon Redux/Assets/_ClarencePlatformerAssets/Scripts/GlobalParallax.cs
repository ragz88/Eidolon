using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalParallax : MonoBehaviour
{
    public  Transform[] layers;            // actual sprites to parallax
    private float[] parallaxAmounts;       // extracted from z values - refers to how much the objects will move relevant to cam movement.
    public float parallaxSpeed = 1;        // multiplier for the speed of objects overall. Higher values result in higher differences in speed between z positions.

    private Transform cam;                 // reference to main cam.
    private Vector3 previousCameraPos;     // position of camera in the previous frame

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        previousCameraPos = cam.position;

        parallaxAmounts = new float[layers.Length];
        for (int i = 0; i < parallaxAmounts.Length; i++)
        {
            parallaxAmounts[i] = layers[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            //parallax movement is opposite to that of the camera
            float parallax = (previousCameraPos.x - cam.position.x) * parallaxAmounts[i];

            float targetPositionX = layers[i].position.x + parallax;

            Vector3 layerTargetPos = new Vector3(targetPositionX, layers[i].position.y, layers[i].position.z);

            //fade from old to new position. As the lerp doesn't complete in a single frame, the lerp speed also has an effect on the total distance that the layer travels
            layers[i].position = Vector3.Lerp(layers[i].position, layerTargetPos, parallaxSpeed * Time.deltaTime);
        }

        previousCameraPos = cam.position;
    }
}
