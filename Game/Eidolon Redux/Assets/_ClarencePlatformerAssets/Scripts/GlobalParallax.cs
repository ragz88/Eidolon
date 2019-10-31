using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalParallax : MonoBehaviour
{
    public  Transform[] layers;            // actual sprites to parallax
    private float[] parallaxAmounts;       // extracted from z values - refers to how much the objects will move relevant to cam movement.
    public float parallaxXSpeed = 1;        // multiplier for the speed of objects overall. Higher values result in higher differences in speed between z positions.
    public float parallaxYSpeed = 0;        // multiplier for the speed of objects overall. Higher values result in higher differences in speed between z positions.

    public bool parallaxOnY = false;

    private Transform cam;                 // reference to main cam.
    private Vector3 previousCameraPos;     // position of camera in the previous frame

    public bool invertDistanceSpeedRelation = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        previousCameraPos = cam.position;

        parallaxAmounts = new float[layers.Length];
        for (int i = 0; i < parallaxAmounts.Length; i++)
        {
            if (!invertDistanceSpeedRelation)
            {
                parallaxAmounts[i] = layers[i].position.z * -1;
            }
            else
            {
                parallaxAmounts[i] = layers[i].position.z;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            //parallax movement is opposite to that of the camera
            float parallax = (previousCameraPos.x - cam.position.x) * parallaxAmounts[i];

            float parallaxY = (previousCameraPos.y - cam.position.y) * parallaxAmounts[i];

            float targetPositionX = layers[i].position.x + parallax;

            float targetPositionY = layers[i].position.y + parallaxY;

            Vector3 layerTargetPos = new Vector3 (0, 0, 0);

            if (parallaxOnY)
            {
                layerTargetPos = new Vector3(layers[i].position.x, targetPositionY, layers[i].position.z);

                //fade from old to new position. As the lerp doesn't complete in a single frame, the lerp speed also has an effect on the total distance that the layer travels
                layers[i].position = Vector3.Lerp(layers[i].position, layerTargetPos, parallaxYSpeed * Time.deltaTime);
            }
            else
            {
                layerTargetPos = new Vector3(targetPositionX, layers[i].position.y, layers[i].position.z);

                //fade from old to new position. As the lerp doesn't complete in a single frame, the lerp speed also has an effect on the total distance that the layer travels
                layers[i].position = Vector3.Lerp(layers[i].position, layerTargetPos, parallaxXSpeed * Time.deltaTime);
            }

            
        }

        previousCameraPos = cam.position;
    }
}
