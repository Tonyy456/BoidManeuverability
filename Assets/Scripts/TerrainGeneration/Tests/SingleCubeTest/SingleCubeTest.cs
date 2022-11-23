using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Version3;
using UnityEditor;


[CustomEditor(typeof(SingleCubeTest))]
public class customButton2 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SingleCubeTest myScript = (SingleCubeTest)target;
        if (GUILayout.Button("Load"))
        {
            myScript.Reload();
        }
    }
}
public class SingleCubeTest : MonoBehaviour
{
    [SerializeField] private MeshFilter filter;
    [SerializeField] private float frequency;
    private MarchingCubes cubes;
    public void Start()
    {
        cubes = new MarchingCubes(new Vector3(), 6, 2);
        cubes.March(filter, frequency);
        cubes.ShowVertices();
    }

    public void Reload()
    {
        cubes.MassiveMarch(filter);
    }
    /*
    [SerializeField] private GenerationSettings settings;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;
    [SerializeField] private MeshFilter filter;

    TriangulationCube cube;
    List<AVertex> vertices = new List<AVertex>();
    public void Start()
    {
        TestCube();
    }

    public void Update()
    {
        int[] ons = new int[8];
        foreach (var v in vertices)
        {
            ons[v.index] = v.IsOn;
        }
        cube.SetVertices(ons);
        if(cube.needsUpdated)
        {
            filter.mesh = cube.CreateMesh();
        }
    }
    public void TestCube()
    {
        cube = new TriangulationCube(new Vector3(0,0,0), 4f);

        int[] isOns = new int[8];
        for(int i = 0; i < 8; i++)
        {
            Vector3 location = cube.getVertexLocation(i);
            GameObject go =  createVertex(i, location, 0);
            isOns[i] = go.GetComponent<AVertex>().IsOn;
        }
        cube.SetVertices(isOns);

        filter.mesh = cube.CreateMesh();
    }

    public GameObject createVertex(int index, Vector3 position, int vertexValue)
    {
        GameObject go = Instantiate(prefab);
        go.transform.position = position;
        go.transform.parent = this.parent;
        var ver = go.GetComponent<AVertex>();
        this.vertices.Add(ver);
        ver.index = index;
        return go;
    }
    */
}
