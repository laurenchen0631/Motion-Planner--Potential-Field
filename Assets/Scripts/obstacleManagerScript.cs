using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class obstacleManagerScript : MonoBehaviour
{
    static private int numOfObstacle = 0;
    static private List<Obstacle> obstacleList = new List<Obstacle>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    static public void addObstacle(Obstacle newObstacle)
    {
        numOfObstacle++;
        obstacleList.Add(newObstacle);
    }

    static public void setObstacle(int index,Obstacle newObstacle)
    {

    }

}

public class Obstacle
{
    private int numOfPolygon;
    public int NumPolygon { get { return numOfPolygon; } }

    private List<Polygon> polygonList = new List<Polygon>();
    float[] configuration = new float[3];

    public Obstacle()
    {
        numOfPolygon = 0;
        configuration = new float[] { 0.0f, 0.0f, 0.0f };
    }

    public Obstacle(int nPolygons)
    {
        numOfPolygon = nPolygons;
        configuration = new float[] { 0.0f, 0.0f, 0.0f };
    }

    public Obstacle(int nPolygons, Polygon[] polygons)
    {
        numOfPolygon = nPolygons;
        for (int i = 0; i < nPolygons; i++)
            polygonList.Add(polygons[i]);
        configuration = new float[] { 0.0f, 0.0f, 0.0f };
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

    public void modifyPolygon(int index, Polygon newPolygon)
    {
        if (index >= 0 && index < numOfPolygon)
            polygonList[index] = newPolygon;
    }

}