using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class obstacleManagerScript : MonoBehaviour
{
    private int numOfObstacle               = 0;
    private List<Obstacle> obstacleList     = new List<Obstacle>();

    // Update is called once per frame 
    void Update()
    {
       
    }

    public void drawObstacles()
    {
        for (int i = 0; i < numOfObstacle; i++)
            obstacleList[i].Draw(i);
    }

    public void addObstacle(Obstacle newObstacle)
    {
        numOfObstacle++;
        obstacleList.Add(newObstacle);
        newObstacle.gameobject.transform.parent = this.gameObject.transform;
    }

    static public void setObstacle(int index, Obstacle newObstacle)
    {

    }
}

public class Obstacle
{
    private int             numOfPolygon    = 0;
    private List<Polygon>   polygonList     = new List<Polygon>();
    public float[]          configuration   = new float[3] { 0.0f, 0.0f, 0.0f };
    public GameObject       gameobject      = new GameObject();
    private float           UNIT            = 8.0f / 128.0f;

    public int              NumPolygon      { get { return numOfPolygon; } }

    public Obstacle()
    {
        setupGameobject();
    }

    public Obstacle(int nPolygons, Polygon[] polygons)
    {
        setupGameobject();
        for (int i = 0; i < nPolygons; i++)
            addPolygon(polygons[i]);
    }

    private void setupGameobject()
    {
        gameobject.name = "Obstacle";
        gameobject.tag = "Obstacle";
        gameobject.layer = LayerMask.NameToLayer("Obstacle"); ;
        gameobject.AddComponent<objectEditor>();
    }

    public void addPolygon(Polygon newPolygon)
    {
        numOfPolygon++;
        polygonList.Add(newPolygon);
        newPolygon.gameobject.transform.parent = this.gameobject.transform;
        newPolygon.gameobject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void updateConfiguration()
    {
        configuration[0] = gameobject.transform.position.x / UNIT;
        configuration[1] = gameobject.transform.position.y / UNIT;
        configuration[2] = 360 - gameobject.transform.rotation.y;
    }

    public void applyTransform()
    {
        gameobject.transform.localScale = new Vector3(UNIT, 1, UNIT);
        gameobject.transform.position = new Vector3(configuration[0] * UNIT, 0, configuration[1] * UNIT);
        gameobject.transform.Rotate(Vector3.up * -configuration[2]);
    }

    private void setupCollider()
    {
        MeshCollider collider = this.gameobject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        MeshFilter[] meshFilters = this.gameobject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.worldToLocalMatrix;
            i++;
        }
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        collider.sharedMesh = mesh;
    }

    public void modifyPolygon(int index, Polygon newPolygon)
    {
        if (index >= 0 && index < numOfPolygon)
            polygonList[index] = newPolygon;
    }

    public void Draw(int index)
    {
        for (int i = 0; i < numOfPolygon; i++)
            polygonList[i].threeDMesh(Color.black);

        gameobject.name = "Obstacle " + index.ToString();
        setupCollider();
        applyTransform();
    }
}