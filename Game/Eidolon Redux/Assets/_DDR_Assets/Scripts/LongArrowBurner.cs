using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongArrowBurner : MonoBehaviour
{
    public float burnSpeed = 1f;

    public float scaleToTranslateMultiplier = 1;

    public Transform mask;
    public Transform parts;

    public Transform topArrow;

    bool atMaxDistance = false;

    public SpriteRenderer[] bottomRends;

    float currentAlpha = 1;
    public float fadeSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!atMaxDistance)
        {
            if (parts.position.y < topArrow.position.y + 0.5f)
            {

                mask.localScale = new Vector3(mask.localScale.x, mask.localScale.y + (burnSpeed * Time.deltaTime), mask.localScale.z);
                parts.localPosition = new Vector3(parts.localPosition.x, parts.localPosition.y + (burnSpeed * scaleToTranslateMultiplier * Time.deltaTime), parts.localPosition.z);

                if (currentAlpha > 0.25f)
                {
                    currentAlpha -= Time.deltaTime * fadeSpeed;

                    for (int i = 0; i < bottomRends.Length; i++)
                    {
                        bottomRends[i].color = new Color(bottomRends[i].color.r, bottomRends[i].color.g, bottomRends[i].color.b, currentAlpha);
                    }
                }
            }
            else
            {
                atMaxDistance = true;
                Destroy(parts.gameObject);

            }
        }
    }
}
