using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class obstacleManagerScript : MonoBehaviour
{
    private int numOfObstacle               = 0;
    private List<Obstacle> obstacleList     = new List<Obstacle>();
    private float UNIT                      = 8.0f / 128.0f;

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

    public byte[,] initBitmap()
    {
        byte[,] bitmap = new byte[130, 130];

        for (int i = 0; i < 130; i++)
        {
            for (int j = 0; j < 130; j++)
            {
                RaycastHit hit;
                if (i == 0 | j == 0 | i == 129 | j == 129)
                    bitmap[i, j] = 255;
                else if (Physics.Raycast(new Vector3(UNIT / 2.0f + (j - 1) * UNIT, 2.0f, UNIT / 2.0f + (i - 1) * UNIT),
                     Vector3.down, out hit, 1.5f))
                {
                    if (hit.collider.gameObject.transform.parent.tag == "Obstacle")
                    {
                        bitmap[i, j] = 255;
                        //print("Hit " + hit.collider.name + " at (" + i + ", " + j + ")");
                    }
                }
                else
                {
                    bitmap[i, j] = 254;
                }
            }
        }

        return bitmap;
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

    private void ChangeLayers(Transform trans)
    {
        trans.gameObject.layer = LayerMask.NameToLayer("Obstacle");
        foreach (Transform child in trans)
        {
            ChangeLayers(child);
        }
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

    private void setupRigidbody()
    {
        Rigidbody rigid = gameobject.AddComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        rigid.mass = 1000;
        rigid.drag = 50;
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
        ChangeLayers(gameobject.transform);
        //setupCollider();
        setupRigidbody();
        
        applyTransform();
    }
}

