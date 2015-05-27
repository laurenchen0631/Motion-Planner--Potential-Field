using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class robotDetailScript : MonoBehaviour {

    public float[] configuration = new float[3];
    private const float UNIT = 1.0f / 16.0f;
    public List<Vector2> controlList = new List<Vector2>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Transform trans = gameObject.transform;
        configuration[0] = trans.position[0] / UNIT;
        configuration[1] = trans.position[2] / UNIT;
        configuration[2] = trans.rotation.eulerAngles[1];
	}

    public void setControls(List<Vector2> controls)
    {
        controlList = new List<Vector2>(controls);
    }

    public List<Vector2> getControls()
    {
        return controlList;
    }

    public int getNumControls()
    {
        return controlList.Count;
    }

    public Vector2 getControlConfig(int index) 
    {
        float sin = Mathf.Sin(configuration[2] * Mathf.Deg2Rad);
        float cos = Mathf.Cos(configuration[2] * Mathf.Deg2Rad);
        Vector2 control = controlList[index];

        return new Vector2(cos * control.x - sin * control.y, cos * control.y - sin * control.x);
    }
}
