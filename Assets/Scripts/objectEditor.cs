using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class objectEditor : MonoBehaviour
{
    public bool isSelected = false;
    private bool isWithin = false;
    private Vector3 rightClickPos;
    private Vector3 leftClickPos;
    private Text text;

    void Awake()
    {
        text = GameObject.FindWithTag("Configuration").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWithin && Input.GetMouseButtonDown(0))
            isSelected = false;

        if (isSelected && !(GameObject.Find("Robot Manager").GetComponent<robotManagerScript>().isStarting) )
        {
            text.text = "(" + transform.position.x * 16f + ", " + transform.position.z * 16f + ", " + transform.rotation.eulerAngles.y + ")";
            // button values are 0 for left button, 1 for right button, 2 for the middle button.
            if (Input.GetMouseButton(0))
            {
                Vector3 nowPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                leftClickPos = Camera.main.ScreenToWorldPoint(leftClickPos);
                Vector3 offset = nowPos - leftClickPos;
                //Vector3 trans = gameObject.transform.position;
                //trans.y = 0;

                //Vector3 direction = offset.normalized * 5;
                //float sqrMag = offset.sqrMagnitude;

                //gameObject.GetComponent<Rigidbody>().MovePosition(trans + offset);
                //if(sqrMag > 0 )

                Vector3 targetPosition = transform.position += new Vector3(offset.x, 0, offset.z);
                //gameObject.transform.Translate(offset.x, 0, offset.z, Space.World);

                //transform
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime*5);
                leftClickPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                rightClickPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 lastRelative = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(rightClickPos));

                Vector3 nowRelative = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                float angle = (Mathf.Atan2(nowRelative.x, nowRelative.z) - Mathf.Atan2(lastRelative.x, lastRelative.z)) * Mathf.Rad2Deg;

                
                gameObject.transform.Rotate(0, angle, 0);
                //gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.AngleAxis(transform.rotation.eulerAngles.y + angle, Vector3.up));
                rightClickPos = Input.mousePosition;
            }
        }
    }

    void OnMouseDown()
    {
        isWithin = true;
        if (Input.GetMouseButtonDown(0))
        {
            isSelected = true;
            leftClickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    void OnMouseExit()
    {
        isWithin = false;
    }
}