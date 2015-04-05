using UnityEngine;
using System.Collections;

public class objectEditor : MonoBehaviour
{
    private bool isSelected = false;
    private bool isWithin = false;
    // Use this for initialization
    void Start()
    {
        Debug.Log("editor start");
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelected)
        {
            //float speed = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        }
    }

    void OnMouseOver()
    {
        // button values are 0 for left button, 1 for right button, 2 for the middle button.
        if(Input.GetMouseButtonDown(0))
        {
            // Whatever you want it to do.
            isSelected = true;

        }
    }

    void OnMouseEnter()
    {
        isWithin = true;
        Debug.Log("Mouse enter: " + gameObject.name);
    }

    void OnMouseExit()
    {
        isWithin = false;
        Debug.Log("Mouse exit: " + gameObject.name);
    }
}