using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * A specific graph implementation for a 3d array of points.
 * 
 * all vertices and edges can be obtained from the definition of the cube.
 * 
 * meshVertices is used for creating a mesh so you know exactly what index
 * to map a triangle edge to.
 */
 /* 
  * Written By: Tony D'Alesandro
  */
public class GridGraph
{

    //define variables related to the space this graph takes
    private Vector3 corner;
    private Vector3 cubeSize;
    private Vector3 graphCenter;
    private float seperation;
    private Vector3 graphSize;

    public Vector3Int resolution { get; set; } //points in each dimension
    public Cube[,,] subCubes;

    private Dictionary<Vector3, int> meshVertices = new Dictionary<Vector3, int>();
    private int numVertices = 0;
    public GridGraph(Vector3 graphCenter, Vector3Int res,  float seperation)
    {
        this.resolution = res;
        this.graphCenter = graphCenter;
        this.seperation = seperation;

        Vector3 negCorner = -1 * (((Vector3)res - new Vector3(1, 1, 1)) * (seperation / 2f));
        negCorner += this.graphCenter;
        cubeSize = new Vector3(seperation, seperation, seperation);
        corner = negCorner;

        GenerateCubes();
    }

    private void GenerateCubes()
    {
        Vector3 center000 = corner + cubeSize / 2f;
        Vector3Int dim = new Vector3Int(resolution.x - 1, resolution.y - 1, resolution.z - 1);
        subCubes = new Cube[dim.x, dim.y, dim.z];
        for (int x = 0; x < dim.x; x++)
        {
            for (int y = 0; y < dim.y; y++)
            {
                for (int z = 0; z < dim.z; z++)
                {
                    Vector3 center = center000;
                    center += (Vector3.Scale(new Vector3(x, y, z), cubeSize));
                    float height = seperation;
                    subCubes[x, y, z] = new Cube(center, height);
                }
            }
        }

        foreach (Cube c in subCubes)
        {
            foreach(Vector3 edge in c.getEdgesForCube())
            {
                if (meshVertices.ContainsKey(edge)) continue;
                meshVertices.Add(edge, numVertices++);
            }
        }
    }

    /*
     * Use edge index dictionary to populate an array of Vector3 representing
     * the array for the mesh.
     */
    public Vector3[] GetMeshIndicies()
    {
        Vector3[] vertices = new Vector3[numVertices];
        foreach(var pair in meshVertices)
        {
            vertices[pair.Value] = pair.Key;
        }
        return vertices;
    }

    /*
     * Gets mesh index based on the Vector3 position of an edge.
     */
    public int GetMeshIndex(Vector3 edgePosition)
    {
        if(meshVertices.TryGetValue(edgePosition, out var index))
        { 
            return index;
        }
        throw new System.Exception("Edge is unknown in a cube");
    }
}
