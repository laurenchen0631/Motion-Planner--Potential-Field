using UnityEngine;
using System.Collections;

public class boundScript : MonoBehaviour 
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "Robot")
            other.gameObject.GetComponent<objectEditor>().isSelected = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "Robot")
        {
            Debug.Log("stay");
            if(this.tag == "West Wall")
                other.gameObject.transform.Translate(0.001f, 0, 0, Space.World);
            else if (this.tag == "East Wall")
                other.gameObject.transform.Translate(-0.001f, 0, 0, Space.World);
            else if (this.tag == "South Wall")
                other.gameObject.transform.Translate(0, 0, 0.001f, Space.World);
            else if (this.tag == "North Wall")
                other.gameObject.transform.Translate(0, 0, -0.001f, Space.World);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
    }
}
