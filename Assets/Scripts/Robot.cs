using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot
{
    private int             numOfPolygon    = 0;
    private List<Polygon>   polygonList     = new List<Polygon>();

    private int             numOfControl    = 0;
    private List<Vector2>   controlList     = new List<Vector2>();

    private Configuration   configuration   = new Configuration();

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
        gameobject.layer = LayerMask.NameToLayer("Robot"); ;
        gameobject.AddComponent<objectEditor>();
    }

    public void addPolygon(Polygon newPolygon)
    {
        numOfPolygon++;
        polygonList.Add(newPolygon);
        newPolygon.gameobject.transform.parent = this.gameobject.transform;
        newPolygon.gameobject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public Configuration getConfiguration()
    {
        return gameobject.GetComponent<robotDetailScript>().configuration;
    }

    public void setConfiguration(float[] config)
    {
        configuration.setConfig(config[0], config[1], config[2]);
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

    public List<Vector2> getControls()
    {
        return controlList;
    }

    public void modifyPolygon(int index, Polygon newPolygon)
    {
        if (index >= 0 && index < numOfPolygon)
            polygonList[index] = newPolygon;
    }

    public void applyTransform()
    {
        gameobject.transform.localScale = new Vector3(UNIT, 1, UNIT);
        gameobject.transform.position = new Vector3(configuration.x * UNIT, 0, configuration.y * UNIT);
        gameobject.transform.Rotate(Vector3.up * -configuration.theta);
    }

    private void ChangeLayers(Transform trans)
    {
        trans.gameObject.layer = LayerMask.NameToLayer("Robot");
        foreach (Transform child in trans)
        {
            ChangeLayers(child);
        }
    }

    private void setupRigidbody()
    {
        Rigidbody rigid = gameobject.AddComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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

    public void Draw3D(int index)
    {
        for (int i = 0; i < numOfPolygon; i++)
            polygonList[i].threeDMesh(Color.red);

        gameobject.name = "Robot " + index.ToString();
        robotDetailScript script = gameobject.AddComponent<robotDetailScript>();
        script.setControls(controlList);
        ChangeLayers(gameobject.transform);
        setupRigidbody();
        applyTransform();
    }

    public void Draw2D(int index)
    {
        for (int i = 0; i < numOfPolygon; i++)
            polygonList[i].updateMesh(Color.blue);

        gameobject.name = "Robot " + index.ToString();
        setupCollider();
        applyTransform();
    }
}

public class Configuration
{
    public float x;
    public float y;
    public float theta;

    public Configuration()
    {
        x = 0;
        y = 0;
        theta = 0;
    }

    public Configuration(Configuration copy)
    {
        this.x = copy.x;
        this.y = copy.y;
        this.theta = copy.theta;
    }

    public Configuration(float x, float y, float theta)
    {
        setConfig(x, y, theta);
    }

    public void setConfig(float x, float y, float theta)
    {
        this.x = x;
        this.y = y;
        this.theta = theta;
    }

    public float distanceTo(Configuration b)
    {
        return Mathf.Sqrt(Mathf.Pow(this.x - b.x, 2) + Mathf.Pow(this.y - b.y, 2));
    }

    public string ToString()
    {
        return "The Configuration ( " + x + ", " + y + ", " + theta + " )"; 
    }
}