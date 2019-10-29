using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTreadmill : MonoBehaviour
{
    public Material treadMat;

    public float xSpeed = 0;
    public float ySpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        treadMat.mainTextureOffset = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        treadMat.mainTextureOffset = new Vector2(treadMat.mainTextureOffset.x + (xSpeed * Time.deltaTime), treadMat.mainTextureOffset.y + (ySpeed * Time.deltaTime));
    }
}
