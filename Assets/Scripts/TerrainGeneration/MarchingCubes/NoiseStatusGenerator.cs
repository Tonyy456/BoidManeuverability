using UnityEngine;

 /* 
  * Written By: Tony D'Alesandro
  */
public class NoiseStatusGenerator : VertexStatusGenerator
{
    private GridCubeIndexer indexer;
    private float frequency;
    private float seed;
    private float surface;
    public NoiseStatusGenerator(GridGraph graph, float frequency, float surface, float seed)
    {
        indexer = new GridCubeIndexer(graph);
        this.frequency = frequency;
        this.seed = seed;
        this.surface = surface;
    }
    public bool[] getStatusForCube(int x, int y, int z)
    {
        Vector3[] vertices = indexer.getVerticesForCube(x, y, z);
        bool[] result = new bool[vertices.Length];
        for(int i = 0; i < vertices.Length; i++)
        {
            float surfaceValue = PerlinNoise.Noise3D(vertices[i], frequency);
            if (surfaceValue < surface)
                result[i] = true;
            else
                result[i] = false;
        }
        return result;
    }
}
