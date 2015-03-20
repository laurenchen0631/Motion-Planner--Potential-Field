using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class obstacleManagerScript : MonoBehaviour 
{
    private int numOfObstacle;
    private List<Obstacle> Obstacles = new List<Obstacle>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class Obstacle
{
    private int numOfPolygon;
    private List<Polygon> Polygons = new List<Polygon>();
    float[] configuration = new float[3];
}