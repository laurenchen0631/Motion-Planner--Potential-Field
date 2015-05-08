using UnityEngine;
using System.Collections;

public class objectEditor : MonoBehaviour
{
    public bool     isSelected      = false;
    private bool    isWithin        = false;
    private Vector3 rightClickPos;
    private Vector3 leftClickPos;

    // Update is called once per frame
    void Update()
    {
        if (isWithin && Input.GetMouseButtonDown(0))
        {
            isSelected      = true;
            rightClickPos   = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (!isWithin && Input.GetMouseButtonDown(0))
            isSelected      = false;

        if(isSelected)
        {
            // button values are 0 for left button, 1 for right button, 2 for the middle button.
            if(Input.GetMouseButton(0))
            {
                Vector3 nowPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                rightClickPos = Camera.main.ScreenToWorldPoint(rightClickPos);
                Vector3 offset = nowPos - rightClickPos;
                //Vector3 targetPosition = transform.position += new Vector3(offset.x, 0, offset.z);
                transform.Translate(offset.x, 0, offset.z, Space.World);
                //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime*5);
                rightClickPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                leftClickPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 lastRelative = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(leftClickPos));

                Vector3 nowRelative = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                float angle = (Mathf.Atan2(nowRelative.x, nowRelative.z) - Mathf.Atan2(lastRelative.x, lastRelative.z)) * Mathf.Rad2Deg;

                transform.Rotate(0, angle, 0);
                leftClickPos = Input.mousePosition;
            }
        }
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