using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version1
{
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

        public void ColorTerrain(Gradient gradient)
        {
            GreenBrownColoring(gradient);
            UpdateMesh();
        }

        private void UpdateMesh()
        {
            mesh.RecalculateNormals();
            mesh.RecalculateUVDistributionMetrics();
        }

        private void GenerateUsingXSquared()
        {
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                Vector3 position = vertices[i];
                float perlin = GetPerlinValueAt(position.x, position.z);
                float newHeight = perlin * perlin * heightScalar;
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
                perlin = Mathf.Clamp(perlin, 0.01f, 1f);
                float newHeight = heightScalar / perlin;
                vertices[i].y = newHeight;
            }
            mesh.vertices = vertices;
        }

        private void GreenBrownColoring(Gradient gradient)
        {
            float maxHeight = float.MinValue;
            float minValue = float.MaxValue;
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                if (mesh.vertices[i].y > maxHeight)
                {
                    maxHeight = mesh.vertices[i].y;
                }
                if (mesh.vertices[i].y < minValue)
                {
                    minValue = mesh.vertices[i].y;
                }
            }

            List<Color> colors = new List<Color>();
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                float time = (maxHeight - mesh.vertices[i].y) / (maxHeight - minValue);
                Color c = gradient.Evaluate(time);
                colors.Add(c);
            }
            //mesh.SetColors(colors);
            mesh.colors = colors.ToArray();
        }

        private float GetPerlinValueAt(float x, float y)
        {
            float returnv = Mathf.Clamp(Mathf.PerlinNoise(((x + 10000f + seed) * frequency), ((y + 10000f + seed) * frequency)), 0.01f, 1f);
            return returnv;
        }
    }
}
