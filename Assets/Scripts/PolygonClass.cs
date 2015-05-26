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
        //Debug.Log(numOfVertices);
        for (int i = 0; i < numOfVertices; i++)
            Vertices.Add(vertex[i]);
        //for (int i = 0; i < numOfVertices; i++)
        //    Debug.Log(Vertices[i]);
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
        Vector3[] normals = new Vector3[numOfVertices];
        Vector2[] uv = new Vector2[numOfVertices];
        for (int i = 0; i < numOfVertices; i++)
        {
            Vector2 vertex = Vertices[numOfVertices - 1 - i];
            vertices[i] = new Vector3(vertex.x, 0, vertex.y);
            normals[i] = new Vector3(0, 1.0f, 0);
            uv[i] = new Vector2(vertex.x, vertex.y);
        }
        mesh.vertices = vertices;
        mesh.uv = uv;

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

    public GameObject threeDMesh(Color color)
    {
        if (gameobject == null)
            return null;
        Mesh mesh = gameobject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        Vector3[] point = new Vector3[numOfVertices * 2];
        for (int i = 0; i < numOfVertices; i++)
        {
            point[i] = new Vector3(Vertices[numOfVertices - 1 - i].x, 1.0f, Vertices[numOfVertices - 1 - i].y);
            point[numOfVertices + i] = new Vector3(Vertices[numOfVertices - 1 - i].x, 0.0f, Vertices[numOfVertices - 1 - i].y);
            
        }
        #region Vertices and normals
        Vector3[] vertices = new Vector3[numOfVertices * 2 + 4 * numOfVertices];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        
        for (int i = 0; i < numOfVertices; i++)
        {
            //top and bottom plane
            vertices[i] = point[i];
            normals[i] = Vector3.up;
            vertices[i + numOfVertices] = point[2 * numOfVertices - 1 - i];
            normals[i + numOfVertices] = Vector3.down;

            //side plane
            int baseIndex = 2 * numOfVertices + 4 * i;
            vertices[baseIndex + 0] = point[i];
            vertices[baseIndex + 1] = point[i + numOfVertices];
            if (i == numOfVertices - 1)
            {
                vertices[baseIndex + 2] = point[numOfVertices];
                vertices[baseIndex + 3] = point[0];
            }
            else
            {
                vertices[baseIndex + 2] = point[i + numOfVertices + 1];
                vertices[baseIndex + 3] = point[i + 1];
            }

            Vector3 side1 = vertices[baseIndex + 1] - vertices[baseIndex];
            Vector3 side2 = vertices[baseIndex + 3] - vertices[baseIndex];
            Vector3 normale = Vector3.Cross(side1, side2);
            normals[baseIndex] = normale;
            normals[baseIndex + 1] = normale;
            normals[baseIndex + 2] = normale;
            normals[baseIndex + 3] = normale;
        }
        #endregion

        #region Triangles
        int numTriangles = 3 * (numOfVertices - 2);
        int[] triangles = new int[numTriangles * 2 + 6 * numOfVertices];
        int v2 = 1;
        int v3 = 2;

        //top and bottom plane
        for (int i = 0; i < numTriangles; i+=3)
        {
            triangles[i] = 0;
            triangles[i + 1] = v2;
            triangles[i + 2] = v3;

            triangles[i + numTriangles] = numOfVertices;
            triangles[i + 1 + numTriangles] = v2 + numOfVertices;
            triangles[i + 2 + numTriangles] = v3 + numOfVertices;

            v2++;
            v3++;

        }
        //side plane
        for (int i = 0; i < numOfVertices; i++)
        {
            int baseIndex = numTriangles * 2 + i * 6;
            int vertx = 2 * numOfVertices + i * 4;
            //Debug.Log(baseIndex);
            //Debug.Log(vertx);

            triangles[baseIndex] = triangles[baseIndex + 3] = vertx;
            triangles[baseIndex + 2] = triangles[baseIndex + 4] = vertx + 2;
            triangles[baseIndex + 1] = vertx + 1;
            triangles[baseIndex + 5] = vertx + 3;

        }
        
        #endregion
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.Optimize();

        gameobject.GetComponent<MeshRenderer>().material.color = color;

        return gameobject;
    }
}
