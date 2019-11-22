using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Button[] buttons;

    public SpriteRenderer[] pics;

    public float posAdjustment;

    Vector3[] butInitPos;

    public float moveSpeed = 1f;

    private void Start()
    {
        butInitPos = new Vector3[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            butInitPos[i] = buttons[i].transform.position;
        }
    }

    private void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].gameObject == EventSystem.current.currentSelectedGameObject)
            {
                buttons[i].transform.position = Vector3.Lerp(buttons[i].transform.position, 
                    butInitPos[i] + new Vector3(posAdjustment, 0, 0), moveSpeed * Time.deltaTime);
                pics[i].color = Color.Lerp(pics[i].color, new Color(pics[i].color.r, pics[i].color.g, pics[i].color.b, 1), moveSpeed * Time.deltaTime);
            }
            else
            {
                buttons[i].transform.position = Vector3.Lerp(buttons[i].transform.position,
                   butInitPos[i], moveSpeed * Time.deltaTime);
                pics[i].color = Color.Lerp(pics[i].color, new Color(pics[i].color.r, pics[i].color.g, pics[i].color.b, 0), moveSpeed * Time.deltaTime);
            }
        }
    }



    /*public RectTransform but1;
    public RectTransform but2;
    public RectTransform but3;

    public RectTransform t1;
    public RectTransform t2;
    public RectTransform t3;

    int currentPos = 0;
    bool lerping = false;

    public float moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!lerping && Input.GetAxis("Vertical") > 0)
        {
            currentPos--;

            currentPos = currentPos % 3;

            lerping = true;
        }
        else if (!lerping && Input.GetAxis("Vertical") < 0)
        {
            currentPos++;

            currentPos = currentPos % 3;

            lerping = true;
        }

        switch (currentPos)
        {
            case 0:
                if (lerping)
                {
                    but1.position = Vector3.MoveTowards(but1.position, t1.position, moveSpeed * Time.deltaTime);
                    but2.position = Vector3.MoveTowards(but2.position, t2.position, moveSpeed * Time.deltaTime);
                    but3.position = Vector3.MoveTowards(but3.position, t3.position, moveSpeed * Time.deltaTime);


                }
                break;

            case 1:

                break;

            case 2:

                break;
        }
    }*/
}
