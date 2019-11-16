using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLerper : MonoBehaviour
{
    public Transform ARCamera;
    public float lerpSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = ARCamera.position;
        transform.rotation = ARCamera.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, ARCamera.position, lerpSpeed*Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, ARCamera.rotation, lerpSpeed*Time.deltaTime);
    }
}
