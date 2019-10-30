using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [System.Serializable]
    public struct RendererToMove
    {
        public Renderer rend;
        public float speed;
        public bool randSpeed;
        public float randSpeedMaxChange;
    }

    public RendererToMove[] rends;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rends.Length; i++)
        {
            float currentXOffset = rends[i].rend.material.GetTextureOffset("_MainTex").x;
            float currentSpeed = rends[i].speed;
            if (rends[i].randSpeed)
            {
                currentSpeed += Random.Range(-rends[i].randSpeedMaxChange, rends[i].randSpeedMaxChange);
            }
            rends[i].rend.material.SetTextureOffset("_MainTex", new Vector2((currentXOffset + (currentSpeed * Time.deltaTime)) % 1, 0));
        }
    }
}
