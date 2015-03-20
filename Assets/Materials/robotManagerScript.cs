using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class robotManagerScript : MonoBehaviour 
{
    private int numOfRobot;
    private List<Robot> Robots = new List<Robot>();
    public float[] startConfig = new float[3];
    public float[] goalConfig = new float[3];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class Robot
{
    private int numOfPolygon;
    private List<Polygon> Polygons = new List<Polygon>();
    float[] controlPoint = new float[2];
    float[] configuration = new float[3];
}