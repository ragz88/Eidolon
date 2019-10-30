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
        public float lerpSpeed;
        public bool randSpeed;
        public float randSpeedMaxChange;
        [HideInInspector] public float currentXTarget;
    }

    public RendererToMove[] rends;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rends.Length; i++)
        {
            float currentXOffset = rends[i].rend.material.GetTextureOffset("_MainTex").x;
            float currentSpeed = rends[i].speed;
            if (rends[i].randSpeed)
            {
                currentSpeed += Random.Range(-rends[i].randSpeedMaxChange, rends[i].randSpeedMaxChange);
            }
            rends[i].currentXTarget = (currentXOffset + (currentSpeed * Time.deltaTime));
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rends.Length; i++)
        {
            float currentXOffset = rends[i].rend.material.GetTextureOffset("_MainTex").x;      //the current x offset of our texture

            if (Mathf.Abs(currentXOffset - rends[i].currentXTarget) < 0.4f)  // checks if the lerp is done
            {
                float currentSpeed = rends[i].speed;
                if (rends[i].randSpeed)
                {
                    currentSpeed += Random.Range(-rends[i].randSpeedMaxChange, rends[i].randSpeedMaxChange);
                }

                rends[i].currentXTarget = (currentXOffset + (currentSpeed * Time.deltaTime));

                Vector2 target = Vector2.Lerp(new Vector2(currentXOffset, 0), new Vector2(rends[i].currentXTarget, 0), rends[i].lerpSpeed * Time.deltaTime);
                rends[i].rend.material.SetTextureOffset("_MainTex", target);
            }
            else
            {
                Vector2 target = Vector2.Lerp(new Vector2(currentXOffset, 0), new Vector2(rends[i].currentXTarget, 0), rends[i].lerpSpeed * Time.deltaTime);
                rends[i].rend.material.SetTextureOffset("_MainTex",target);
            }
        }
    }
}
