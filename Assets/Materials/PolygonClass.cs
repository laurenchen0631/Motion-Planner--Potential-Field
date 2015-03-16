using UnityEngine;
using System.Collections.Generic;

public class Polygon
{
    private int numOfVertices;
    private List<Vector2> Vertices = new List<Vector2>();
    public int NumVertices { get { return numOfVertices; } }

    Polygon()
    {
        numOfVertices = 3;
        for (int i = 0; i < numOfVertices; i++)
            Vertices.Add(new Vector2());
    }

    //Polygon(int nVertice)
    //{
    //    numOfVertices = nVertice;
    //    for (int i = 0; i < numOfVertices; i++)
    //        Vertices.Add(new UnityEngine.Vector2());
    //}

    Polygon(int nVertice,Vector2[] vertex)
    {
        numOfVertices = nVertice;
        for (int i = 0; i < numOfVertices; i++)
            Vertices.Add(vertex[i]);
    }

    Polygon(int nVertice, float[][] vertex)
    {
        numOfVertices = nVertice;
        for (int i = 0; i < numOfVertices; i++)
            Vertices.Add(new Vector2(vertex[i][0], vertex[i][1]));
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

    public void modifyVertex(int index,Vector2 newVertex)
    {
        Vertices[index] = newVertex;
    }
}
