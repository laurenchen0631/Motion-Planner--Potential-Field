using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class robotDetailScript : MonoBehaviour {

    public float[] configuration = new float[3];
    private const float UNIT = 1.0f / 16.0f;
    private List<Vector2> controlList = new List<Vector2>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 position = gameObject.transform.position;
        configuration[0] = position[0] / UNIT;
        configuration[1] = position[2] / UNIT;
        configuration[2] = gameObject.transform.rotation.eulerAngles[1];
	    //gameObject.transform
	}

    public void setControl(List<Vector2> controls)
    {
        controlList = new List<Vector2>(controls);
    }
}
