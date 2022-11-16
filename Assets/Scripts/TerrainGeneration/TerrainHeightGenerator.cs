using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenerationType
{
    XSquared,
    InverseX
}

public enum TerrainColoringType
{
    BrownToGreen
}
public class TerrainHeightGenerator
{
    private Mesh mesh;
    private float seed;
    private float frequency;
    private float heightScalar;
    public TerrainHeightGenerator(Mesh mesh, float heightScalar, float frequency = 0.1f, float seed = -1)
    {
        this.mesh = mesh;
        this.frequency = frequency;
        this.heightScalar = heightScalar;
        if (seed < 0)
            this.seed = Random.Range(0, (float)10e10);
        else
            this.seed = seed;
    }

    public Mesh GetMesh()
    {
        return mesh;
    }

    public void GenerateHeight(GenerationType type)
    {
        switch (type)
        {
            case GenerationType.XSquared:
                GenerateUsingXSquared();
                break;
            case GenerationType.InverseX:
                GenerateUsingInverseX();
                break;
            default:
                GenerateUsingXSquared();
                break;
        }
        UpdateMesh();
    }

    public void ColorTerrain(TerrainColoringType type)
    {
        switch (type)
        {
            case TerrainColoringType.BrownToGreen:

                break;
            default:
                break;
        }
    }

    private void UpdateMesh()
    {
        mesh.RecalculateNormals();
    }

    private void GenerateUsingXSquared()
    {
        Vector3[] vertices = mesh.vertices;
        for(int i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 position = vertices[i];
            float perlin = GetPerlinValueAt(position.x, position.z);
            float newHeight = perlin * perlin *  heightScalar;
            vertices[i].y = newHeight;
        }
        mesh.vertices = vertices;
    }

    private void GenerateUsingInverseX()
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 position = vertices[i];
            float perlin = GetPerlinValueAt(position.x, position.z);
            float newHeight = heightScalar/perlin;
            vertices[i].y = newHeight;
        }
        mesh.vertices = vertices;
    }

    private void GreenBrownColoring()
    {
        float maxHeight = float.MaxValue;
        float minValue = float.MinValue;
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            if (mesh.vertices[i].y < maxHeight)
            {
                maxHeight = mesh.vertices[i].y;
            }
            if (mesh.vertices[i].y > minValue)
            {
                minValue = mesh.vertices[i].y;
            }
        }

        for(int i = 0; i < mesh.vertexCount; i++)
        {
            //color
        }
    }

    private float GetPerlinValueAt(float x, float y)
    {
        float returnv =  Mathf.Clamp(Mathf.PerlinNoise(((x + 10000f) * frequency), ((y + 10000f) * frequency)), 0.01f ,1f);
        return returnv;
    }
}
