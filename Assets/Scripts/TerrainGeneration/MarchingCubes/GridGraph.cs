using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * A specific graph implementation for a 3d array of points.
 * 
 * vertices are created as vector3 positions in space
 * 
 * edges are created as midpoints between vertices represented as vector3.
 * edges are seperated into a list based on what axis they are parallel to.
 * the size of each list is the same as the resolution but one less in the 
 * dimension that edge is parallel to.
 * 
 * potential code refactoring idea for the edges to make the code shorter
 * and simpiler is to create a dictionary based on what axis the edge is parallel
 * to and then the corresponding Vector3Int index. See below
 * 
 * public Dictionary<(int axis, Vector3 index), Vector3> edges = new Dictionary<(int, Vector3), Vector3>();
 */
public class GridGraph
{

    //define variables related to the space this graph takes
    private Vector3 corner;
    private Vector3 cubeSize;
    private Vector3 graphCenter;
    private float seperation;
    private Vector3 graphSize;
    public Vector3Int resolution { get; set; }

    //define all vertices and edges
    public Cube[,,] subCubes;

    private Dictionary<Vector3, int> meshVertices = new Dictionary<Vector3, int>();
    private int numVertices = 0;
    public GridGraph(Vector3 graphCenter, Vector3Int res,  float seperation)
    {
        resolution = res;
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
        //Vector3 center000 = new Vector3();
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

    public List<Vector3> getEdges() => throw new System.NotImplementedException();
    public List<Vertex> getVertices() => throw new System.NotImplementedException();
}
