using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class robotManagerScript : MonoBehaviour
{
    private int             numOfRobot      = 0;
    private List<Robot>     robotList       = new List<Robot>();
    public List<float[]>    goalConfigList  = new List<float[]>();

    // Update is called once per frame
    void Update()
    {
    }

    public void drawRobots()
    {
        for (int i = 0; i < numOfRobot; i++)
            robotList[i].Draw(i);
    }

    public void addRobot(Robot newRobot)
    {
        numOfRobot++;
        robotList.Add(newRobot);
        newRobot.gameobject.transform.parent = this.gameObject.transform;
    }

    public void addGoal(float[] newGoal)
    {
        if (newGoal.Length == 3)
            goalConfigList.Add(newGoal);
    }
}

public class Robot
{
    private int             numOfPolygon    = 0;
    private List<Polygon>   polygonList     = new List<Polygon>();

    private int             numOfControl    = 0;
    private List<Vector2>   controlList     = new List<Vector2>();

    public float[]          configuration   = new float[3] { 0.0f, 0.0f, 0.0f };

    public GameObject       gameobject      = new GameObject();
    private float           UNIT            = 8.0f / 128.0f;

    public Robot()
    {
        setupGameobject();
    }

    public Robot(int nPolygons, Polygon[] polygons)
    {
        setupGameobject();
        for (int i = 0; i < nPolygons; i++)
            addPolygon(polygons[i]);
    }

    private void setupGameobject()
    {
        gameobject.name = "Robot";
        gameobject.tag = "Robot";
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

    public void addControlPoint(Vector2 newPoint)
    {
        numOfControl++;
        controlList.Add(newPoint);
    }

    public void setControlPoint(int index, Vector2 newPoint)
    {
        controlList[index] = newPoint;
    }

    public Vector2 getControlPoint(int index)
    {
        return controlList[index];
    }

    public void modifyPolygon(int index, Polygon newPolygon)
    {
        if (index >= 0 && index < numOfPolygon)
            polygonList[index] = newPolygon;
    }

    public void applyTransform()
    {
        gameobject.transform.localScale = new Vector3(UNIT, UNIT, UNIT);
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

    public void Draw(int index)
    {
        for (int i = 0; i < numOfPolygon; i++)
            polygonList[i].updateMesh(Color.red);

        gameobject.name = "Robot " + index.ToString();
        setupCollider();
        applyTransform();
    }
}