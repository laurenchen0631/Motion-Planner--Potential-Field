using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class robotDetailScript : MonoBehaviour {

    public Configuration configuration = new Configuration();
    private const float UNIT = 1.0f / 16.0f;
    public List<Vector2> controlList = new List<Vector2>();
	
	// Update is called once per frame
	void Update ()
    {
        Transform trans = gameObject.transform;
        configuration.x = trans.position[0] / UNIT;
        configuration.y = trans.position[2] / UNIT;
        configuration.theta = trans.rotation.eulerAngles[1];
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

    public Vector2 getControlPos(int index, bool global) 
    {
        if (index >= getNumControls())
            return new Vector2();

        float sin = Mathf.Sin(configuration.theta * Mathf.Deg2Rad);
        float cos = Mathf.Cos(configuration.theta * Mathf.Deg2Rad);
        Vector2 control = controlList[index];
        Vector2 controlConfig = new Vector2(cos * control.x - sin * control.y, cos * control.y - sin * control.x);

        if (global)
            return new Vector2(configuration.x + controlConfig.x, configuration.y + controlConfig.y);
        else
            return controlConfig;
    }
}
