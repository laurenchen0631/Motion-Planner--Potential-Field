using UnityEngine;
using System.Collections;

public class boundScript : MonoBehaviour 
{
    //private float UNIT = 8.0f / 128.0f;
    //private Vector3 lastPos;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "Robot")
        {
            other.gameObject.GetComponent<objectEditor>().isSelected = false;
        }
    }

    void OnTriggerStay(Collider other)
    {

    }
}
