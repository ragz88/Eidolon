using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongFallingArrow : MonoBehaviour
{

    //note: 0 is bottom of arrow - (-arrowLength) is top (z coordinate)

    // holds a vector 3 describing point position and a float descibing the width at that point
    /*public struct BarPoint
    {
        public Vector3 position;             // specific position on line renderer
        public float width;                  // width at that position

        public BarPoint (Vector3 position, float width)
        {
            this.position = position;
            this.width = width;
        }
    }*/

    public float fallSpeed = 1;
    public float destroyHeight = -6f;        // at this y position, arrow is automatically destroyed (if missed)


    public LineRenderer topBar;              // reference to bar that should light up when arrow segment is hit.

    public Transform topArrow, bottomArrow;  // reference to the two containing arrow's transforms

    public float arrowLength = 0;            // stores the distance between the top and bottom arrows.
    public float topBarMaxWidth;             // stores maximum width for top bar (bar at this width when 'on'); 


    List<Vector3> topBarPoints = new List<Vector3>();    // the positions and relative widths of the top line renderer to be edited during runtime
    AnimationCurve widthCurve = new AnimationCurve();             // the widths along our topBar

    public bool inSensor = false;
    public bool isHit = false;
    public float relativeBottomDistance;

    Vector3 endPoint;


    // Start is called before the first frame update
    void Start()
    {
        //topBarMaxWidth = topBar.widthMultiplier;
        topBar.widthCurve = widthCurve;
        arrowLength = Mathf.Abs(topArrow.transform.position.y - bottomArrow.transform.position.y);
        endPoint = topBar.GetPosition(1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -fallSpeed * Time.deltaTime, 0);

        

        if (inSensor)
        {
            if (isHit)
            {
                topBarPoints.Add(new Vector3(0, 0, relativeBottomDistance));           // add point to topBarPoint list

                //float widthCurvePosition = Mathf.Abs(arrowLength - relativeBottomDistance) / arrowLength;

                //widthCurve.AddKey(widthCurvePosition, topBarMaxWidth);                          // add point to our width animation curve
                //widthCurve.AddKey(relativeBottomDistance, topBarMaxWidth);                          // add point to our width animation curve

                Keyframe newKey = new Keyframe((Mathf.Abs(relativeBottomDistance) /arrowLength), topBarMaxWidth, 0, 0);
                widthCurve.AddKey(newKey);                          // add point to our width animation curve

                Vector3[] newTopBarPoints = new Vector3[topBarPoints.Count + 1];   // temporary array for assigning values into our line renderer


                for (int i = 0; i < topBarPoints.Count; i++)
                {
                    newTopBarPoints[i] = topBarPoints[i];
                }

                newTopBarPoints[newTopBarPoints.Length - 1] = endPoint;    //makes sure that total length of line remains constant

                // pass new values into the line renderer object
                topBar.widthCurve = widthCurve;
                topBar.positionCount = newTopBarPoints.Length;
                topBar.SetPositions(newTopBarPoints);
            }
            else
            {
                topBarPoints.Add(new Vector3(0, 0, relativeBottomDistance));           // add point to topBarPoint list

                //float widthCurvePosition = Mathf.Abs(arrowLength - relativeBottomDistance) / arrowLength;

                //widthCurve.AddKey(widthCurvePosition, topBarMaxWidth);                          // add point to our width animation curve
                Keyframe newKey = new Keyframe((Mathf.Abs(relativeBottomDistance) / arrowLength), 0, 0, 0);
                widthCurve.AddKey(newKey);                          // add point to our width animation curve

                Vector3[] newTopBarPoints = new Vector3[topBarPoints.Count + 1];   // temporary array for assigning values into our line renderer


                for (int i = 0; i < topBarPoints.Count; i++)
                {
                    newTopBarPoints[i] = topBarPoints[i];
                }

                newTopBarPoints[newTopBarPoints.Length - 1] = endPoint;    //makes sure that total length of line remains constant

                // pass new values into the line renderer object
                topBar.widthCurve = widthCurve;
                topBar.positionCount = newTopBarPoints.Length;
                topBar.SetPositions(newTopBarPoints);
            }
        }
        else if (topArrow.transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }

        //Time.time
    }


    //inserts a new point into our topBarPoint list and into our width animation curve.
    public void AddBarPoint (Vector3 position, float width, float bottomRelativeDist)
    {
        Vector3 adjustedPosition = new Vector3(position.x, position.y, position.z + (0.5f * arrowLength));

        topBarPoints.Add(adjustedPosition);           // add point to topBarPoint list

        float widthCurvePosition = Mathf.Abs(arrowLength - bottomRelativeDist) / arrowLength;
        widthCurve.AddKey(widthCurvePosition, width);                          // add point to our width animation curve

        Vector3[] newTopBarPoints = new Vector3[topBarPoints.Count];   // temporary array for assigning values into our line renderer

        
        for (int i = 0; i < topBarPoints.Count; i++)
        {
            newTopBarPoints[i] = topBarPoints[i];
        }



        // pass new values into the line renderer object
        topBar.widthCurve = widthCurve;
        topBar.positionCount = newTopBarPoints.Length;
        topBar.SetPositions(newTopBarPoints);
        

    }
}
