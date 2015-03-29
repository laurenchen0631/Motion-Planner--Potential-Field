using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class obstacleManagerScript : MonoBehaviour
{
    private int numOfObstacle = 0;
    private List<Obstacle> obstacleList = new List<Obstacle>();
    
    // Use this for initialization
    void Start()
    {
        numOfObstacle = 0;
        obstacleList = new List<Obstacle>();
    }

    // Update is called once per frame 
    void Update()
    {
        for(int i=0;i<numOfObstacle;i++)
        {
            obstacleList[i].Draw(i);
        }
    }

    public void addObstacle(Obstacle newObstacle)
    {
        numOfObstacle++;
        obstacleList.Add(newObstacle);
        newObstacle.gameobject.transform.parent = this.gameObject.transform;
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

    public GameObject gameobject = new GameObject();

    public Obstacle()
    {
        numOfPolygon = 0;
        configuration = new float[] { 0.0f, 0.0f, 0.0f };
        gameobject.name = "Obstacle";
    }

    public Obstacle(int nPolygons)
    {
        numOfPolygon = nPolygons;
        configuration = new float[] { 0.0f, 0.0f, 0.0f };
        gameobject.name = "Obstacle";

    }

    public Obstacle(int nPolygons, Polygon[] polygons)
    {
        numOfPolygon = nPolygons;
        for (int i = 0; i < nPolygons; i++)
            polygonList.Add(polygons[i]);
        configuration = new float[] { 0.0f, 0.0f, 0.0f };
        gameobject.name = "Obstacle";

    }

    public void addPolygon(Polygon newPolygon)
    {
        numOfPolygon++;
        polygonList.Add(newPolygon);

        newPolygon.gameobject.transform.parent = this.gameobject.transform;
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

    public void Draw(int index)
    {
        for (int i = 0; i < numOfPolygon;i++ )
            polygonList[i].draw(Color.black);

        //gameobject.transform.Translate(configuration[0] * 0.2f, 0, configuration[1] * 0.2f);
            
    }
}