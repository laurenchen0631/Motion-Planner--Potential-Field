using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class robotManagerScript : MonoBehaviour
{
    private int numOfRobot = 0;
    private List<Robot> robotList = new List<Robot>();
    //static public float[] goalConfig = new float[3] { 0.0f, 0.0f, 0.0f };
    public List<float[]> goalConfigList = new List<float[]>();
    Mesh mesh;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void addRobot(Robot newRobot)
    {
        numOfRobot++;
        robotList.Add(newRobot);
    }

    public void addGoal(float[] newGoal)
    {
        if (newGoal.Length == 3)
            goalConfigList.Add(newGoal);
    }

    static public void setRobot(int index, Robot newRobot)
    {

    }
}

public class Robot
{
    private int numOfPolygon;
    private List<Polygon> polygonList = new List<Polygon>();

    private int numOfControl;
    private List<Vector2> controlList = new List<Vector2>();

    float[] configuration = new float[3];

    public Robot()
    {
        numOfPolygon = 0;
        configuration = new float[] { 0.0f, 0.0f, 0.0f };
        numOfControl = 0;
    }

    public Robot(int nPolygons)
    {
        numOfPolygon = nPolygons;
        configuration = new float[] { 0.0f, 0.0f, 0.0f };
        numOfControl = 0;
    }

    public Robot(int nPolygons, Polygon[] polygons)
    {
        numOfPolygon = nPolygons;
        for (int i = 0; i < nPolygons; i++)
            polygonList.Add(polygons[i]);
        configuration = new float[] { 0.0f, 0.0f, 0.0f };
        numOfControl = 0;
    }

    public void addPolygon(Polygon newPolygon)
    {
        numOfPolygon++;
        polygonList.Add(newPolygon);
    }

    public void setConfiguration(float[] newConfig)
    {
        if (newConfig.Length != 3)
            return;

        configuration = newConfig;
    }

    public void setConfiguration(float x, float y, float theta)
    {
        configuration[0] = x;
        configuration[1] = y;
        configuration[2] = theta;
    }

    public float[] getConfiguration()
    {
        return configuration;
    }

    public void addControlPoint(Vector2 newPoint)
    {
        numOfControl++;
        controlList.Add(newPoint);
    }

    public void setControlPoint(int index, Vector2 newPoint)
    {
        controlList[index] = newPoint;
    }

    //public void setControlPoint(int index, float x, float y)
    //{
    //    controlPoint[0] = x;
    //    controlPoint[1] = y;
    //}

    public Vector2 getControlPoint(int index)
    {
        return controlList[index];
    }

    public void modifyPolygon(int index, Polygon newPolygon)
    {
        if (index >= 0 && index < numOfPolygon)
            polygonList[index] = newPolygon;
    }
}