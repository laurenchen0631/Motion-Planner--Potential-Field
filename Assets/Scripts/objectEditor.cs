using UnityEngine;
using System.Collections;

public class objectEditor : MonoBehaviour
{
    private bool isSelected = false;
    private bool isWithin = false;
    private float UNIT = 8.0f / 128.0f;
    private Vector2 lastPos2;
    private Vector3 lastPos3;
    private float leftBound = 0;
    private float rightBound = 8;
    private float bottomBound = 0;
    private float topBound = 8;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isWithin && Input.GetMouseButtonDown(0))
        {
            isSelected = true;
            lastPos2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //lastPos3 = Input.mousePosition;
        }
        else if (!isWithin && Input.GetMouseButtonDown(0))
            isSelected = false;

        if(isSelected)
        {
            // button values are 0 for left button, 1 for right button, 2 for the middle button.
            if(Input.GetMouseButton(0))
            {
                Vector2 nowPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 offset = nowPos - lastPos2;

                if(Mathf.Abs(offset.x)>=3)
                {
                    offset.x = (int)(offset.x / 3);
                    Vector3 targetPosition = transform.position += new Vector3(offset.x * UNIT, 0, 0);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
                    lastPos2.x += offset.x * 3.0f;
                }
                if(Mathf.Abs(offset.y)>=3)
                {
                    offset.y = (int)(offset.y / 3);
                    Vector3 targetPosition = transform.position += new Vector3(0, 0, offset.y * UNIT);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
                    lastPos2.y += offset.y * 3.0f;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                lastPos3 = Input.mousePosition;
            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 lastRelative = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(lastPos3));

                Vector3 nowRelative = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                float angle = (Mathf.Atan2(nowRelative.x, nowRelative.z) - Mathf.Atan2(lastRelative.x, lastRelative.z)) * Mathf.Rad2Deg;

                transform.Rotate(0, angle, 0);
                lastPos3 = Input.mousePosition;
            }

            
        }
    }

    void OnMouseOver()
    {

    }

    void OnMouseEnter()
    {
        isWithin = true;
    }

    void OnMouseExit()
    {
        isWithin = false;
    }
}