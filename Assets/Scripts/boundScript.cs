using UnityEngine;
using System.Collections;

public class boundScript : MonoBehaviour 
{

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
