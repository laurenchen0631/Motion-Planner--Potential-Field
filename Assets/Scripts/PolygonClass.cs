using UnityEngine;
using System.Collections.Generic;

public class Polygon
{
    private int numOfVertices;
    private List<Vector2> Vertices = new List<Vector2>();
    public int NumVertices { get { return numOfVertices; } }
    public GameObject gameobject = new GameObject();
    public const float UNIT = 8.0f / 128.0f;

    public Polygon()
    {
        numOfVertices = 3;
        for (int i = 0; i < numOfVertices; i++)
            Vertices.Add(new Vector2());
        createObject();
    }

    public Polygon(int nVertice)
    {
        numOfVertices = nVertice;
        createObject();
    }

    public Polygon(int nVertice, Vector2[] vertex)
    {
        numOfVertices = nVertice;
        Debug.Log(numOfVertices);
        for (int i = 0; i < numOfVertices; i++)
            Vertices.Add(vertex[i]);
        for (int i = 0; i < numOfVertices; i++)
            Debug.Log(Vertices[i]);
        createObject();
    }

    public Polygon(int nVertice, float[][] vertex)
    {
        numOfVertices = nVertice;
        for (int i = 0; i < numOfVertices; i++)
            Vertices.Add(new Vector2(vertex[i][0], vertex[i][1]));
        createObject();
    }

    public Vector2 getVertex(int index)
    {
        return Vertices[index];
    }

    public void addVertex(Vector2 newVertex)
    {
        ++numOfVertices;
        Vertices.Add(newVertex);
    }

    public void removeVertex(int index)
    {
        
    }

    public void removeVertex(Vector2 point)
    {

    }

    public void modifyVertex(int index, Vector2 newVertex)
    {
        if (index >= 0 && index < numOfVertices)
            Vertices[index] = newVertex;
    } 

    private void sortVertices()
    {

    }

    private void createObject()
    {
        //Create a new gameobject for polygon
        gameobject.name = "Polygon";
        gameobject.layer = 2;

        //Add MeshFilter and MeshRenderer for gameobject
        gameobject.AddComponent<MeshFilter>();
        gameobject.AddComponent<MeshRenderer>();
        //Debug.Log("created");
    }

    public GameObject updateMesh(Color color)
    {
        if (gameobject == null)
        {
            Debug.Log("no mesh");
            return null;
        }
        Mesh mesh = gameobject.GetComponent<MeshFilter>().mesh;

        //Clean the mesh before update
        mesh.Clear();

        //Add vertices, normals, uv on the Mesh
        Vector3[] vertices = new Vector3[numOfVertices];
        Vector2[] uv = new Vector2[numOfVertices];
        for (int i = 0; i < numOfVertices; i++)
        {
            Vector2 vertex = Vertices[numOfVertices-1-i];
            vertices[i] = new Vector3(vertex.x, 0, vertex.y);
            uv[i] = new Vector2(vertex.x, vertex.y);
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        //Arrange triangles block on the Mesh
        int v2 = 1;
        int v3 = 2;
        int[] triangles = new int[3 * (numOfVertices - 2)];
        for (int i = 0; i < triangles.Length; i += 3)
        {
            triangles[i] = 0;
            triangles[i + 1] = v2;
            triangles[i + 2] = v3;
            v2++;
            v3++;
        }
        mesh.triangles = triangles;

        //set polygon color
        gameobject.GetComponent<MeshRenderer>().material.color = color;

        return gameobject;
    }

}
