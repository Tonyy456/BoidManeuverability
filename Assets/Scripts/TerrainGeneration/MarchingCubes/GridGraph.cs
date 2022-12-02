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
    private Vector3 startPoint; //the point furthest in the negative direction
    private Vector3 center { get; set; }
    private float pointSeperation { get; set; }
    private Vector3 size { get; set; }
    public Vector3Int Resolution { get; set; }

    //define all vertices and edges

    public Vector3[,,] vertices { get; set; }
    public Vector3[,,] Xparallel { get; set; }
    public Vector3[,,] Yparallel { get; set; }
    public Vector3[,,] Zparallel { get; set; }

    private Dictionary<Vector3, int> meshIndex;
    private int meshVertexCount = 0;

    public GridGraph(Vector3 graphCenter, Vector3Int res,  float seperation)
    {
        Resolution = res;
        center = graphCenter;
        pointSeperation = seperation;
        size = new Vector3(
            (res.x - 1) * seperation,
            (res.y - 1) * seperation,
            (res.z - 1) * seperation);
        startPoint = new Vector3(
            -size.x / 2 + graphCenter.x,
            -size.y / 2 + graphCenter.y,
            -size.z / 2 + graphCenter.z);

        GenerateVertices();
        GenerateEdges();
    }

    /*
     * Generates the vertices
     */
    private void GenerateVertices()
    {
        vertices = new Vector3[Resolution.x, Resolution.y, Resolution.z];
        for (int x = 0; x < Resolution.x; x++) {
            for (int y = 0; y < Resolution.y; y++) {
                for (int z = 0; z < Resolution.z; z++) {
                    Vector3 vertexPosition = new Vector3(
                        startPoint.x + pointSeperation * x, 
                        startPoint.y + pointSeperation * y, 
                        startPoint.z + pointSeperation * z
                        );
                    vertices[x, y, z] = vertexPosition;
                }
            }
        }
    }

    /*
     * Fills edges into 3d arrays. 
     * 
     * Triple for loop for each array because dimensions are of each array 
     * are different. Edge defined as the midpoint between the two vertices.
     * 
     * Dictionary created so each edge point is assigned a unique index.
     */
    private void GenerateEdges()
    {
        if (vertices == null) throw new System.Exception("Generating edges before vertices generated");

        meshIndex = new Dictionary<Vector3, int>();
        Xparallel = new Vector3[Resolution.x - 1, Resolution.y, Resolution.z];
        Yparallel = new Vector3[Resolution.x, Resolution.y - 1, Resolution.z];
        Zparallel = new Vector3[Resolution.x, Resolution.y, Resolution.z - 1];
        int index = 0;

        //create x parallel edges
        for (int x = 0; x < Resolution.x - 1; x++) {
            for (int y = 0; y < Resolution.y; y++) {
                for (int z = 0; z < Resolution.z; z++) {
                    Vector3 position = ((vertices[x, y, z] + vertices[x + 1, y, z]) / 2f);
                    Xparallel[x, y, z] = position;
                    meshIndex.Add(position, index++);
                }
            }
        }

        //create y parallel edges
        for (int x = 0; x < Resolution.x; x++)  {
            for (int y = 0; y < Resolution.y - 1; y++) {
                for (int z = 0; z < Resolution.z; z++) {
                    Vector3 position = ((vertices[x, y, z] + vertices[x, y + 1, z]) / 2f);
                    Yparallel[x, y, z] = position;
                    meshIndex.Add(position, index++);
                }
            }
        }

        //Create z parallel edges
        for (int x = 0; x < Resolution.x; x++)  {
            for (int y = 0; y < Resolution.y; y++) {
                for (int z = 0; z < Resolution.z - 1; z++) {
                    Vector3 position = ((vertices[x, y, z] + vertices[x, y, z + 1]) / 2f);
                    Zparallel[x, y, z] = position;
                    meshIndex.Add(position, index++);
                }
            }
        }
        meshVertexCount = index;
    }

    /*
     * Use edge index dictionary to populate an array of Vector3 representing
     * the array for the mesh.
     */
    public Vector3[] GetMeshIndicies()
    {
        Vector3[] vertices = new Vector3[meshVertexCount];
        foreach(var pair in meshIndex)
            vertices[pair.Value] = pair.Key;
        return vertices;
    }

    /*
     * Gets mesh index based on the Vector3 position of an edge.
     */
    public int GetMeshIndex(Vector3 edgePosition)
    {
        if(meshIndex.TryGetValue(edgePosition, out var index))
        { 
            return index;
        }
        throw new System.Exception("Edge is unknown in a cube");
    }

    /*
     * Draws the bounds of the entire grid for one frame.
     */
    public void DrawBounds(Color c)
    {
        Debug.DrawLine(vertices[0, 0, 0], vertices[0, Resolution.y - 1, 0], c);
        Debug.DrawLine(vertices[0, 0, 0], vertices[Resolution.x - 1, 0, 0], c);
        Debug.DrawLine(vertices[0, 0, 0], vertices[0, 0, Resolution.z - 1], c);

        Debug.DrawLine(vertices[Resolution.x - 1, 0, Resolution.z - 1], vertices[0, 0, Resolution.z - 1], c);
        Debug.DrawLine(vertices[Resolution.x - 1, 0, Resolution.z - 1], vertices[Resolution.x - 1, Resolution.y - 1, Resolution.z - 1], c);
        Debug.DrawLine(vertices[Resolution.x - 1, 0, Resolution.z - 1], vertices[Resolution.x - 1, 0, 0], c);

        Debug.DrawLine(vertices[0, Resolution.y - 1, Resolution.z - 1], vertices[Resolution.x - 1, Resolution.y - 1, Resolution.z - 1], c);
        Debug.DrawLine(vertices[0, Resolution.y - 1, Resolution.z - 1], vertices[0, 0, Resolution.z - 1], c);
        Debug.DrawLine(vertices[0, Resolution.y - 1, Resolution.z - 1], vertices[0, Resolution.y - 1, 0], c);

        Debug.DrawLine(vertices[Resolution.x - 1, Resolution.y - 1, 0], vertices[0, Resolution.y - 1, 0], c);
        Debug.DrawLine(vertices[Resolution.x - 1, Resolution.y - 1, 0], vertices[Resolution.x - 1, 0, 0], c);
        Debug.DrawLine(vertices[Resolution.x - 1, Resolution.y - 1, 0], vertices[Resolution.x - 1, Resolution.y - 1, Resolution.z - 1], c);
    }

    public List<Vector3> getEdges() => throw new System.NotImplementedException();
    public List<Vertex> getVertices() => throw new System.NotImplementedException();
}
