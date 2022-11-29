using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class HeightMapper : ITerrainAlgorithm
    {
        private MeshFilter filter;
        public HeightMapper(MeshFilter filer)
        {
            this.filter = filer;
        }

        public void Generate(Vector3 position, GenerationSettings settings)
        {
            /*
             * Algorithm to create the points and their heights.
             */
            List<Vector3> meshPoints = new List<Vector3>();
            List<Color> vertexColors = new List<Color>();
            Vector2 start = new Vector2(position.x - settings.resolution.x * settings.pointSeperation/2, position.z - settings.resolution.z * settings.pointSeperation / 2);
            for(int x = 0; x < settings.resolution.x; x++)
            {
                for(int y = 0; y < settings.resolution.y; y++)
                {
                    Vector2 xyPos = new Vector2(start.x + settings.pointSeperation * x, start.y + settings.pointSeperation * y);
                    Vector3 pos = GeneratePoint(xyPos, settings);
                    Color c = settings.HeightColorGradient.Evaluate(pos.y);
                    meshPoints.Add(pos);
                    vertexColors.Add(c);
                }
            }

            /*
             * Algorithm to create triangles in winding order.
             * len(indices) == ((resolution - 1) ^ 2) * 6
             */
            List<int> indices = new List<int>();
            int width = settings.resolution.x;
            for(int x = 0; x < settings.resolution.x - 1; x++)
            {
                for(int y = 0; y< settings.resolution.x - 1; y++)
                {
                    int vertex = x + width * y;
                    indices.Add(vertex + width + 1);
                    indices.Add(vertex + width);
                    indices.Add(vertex);

                    indices.Add(vertex);
                    indices.Add(vertex + 1);
                    indices.Add(vertex + width + 1);
                }
            }

            Mesh mesh = new Mesh();
            Debug.Log($"v: {meshPoints.Count}, t: {indices.Count}, c: {vertexColors.Count}");
            mesh.vertices = meshPoints.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.colors = vertexColors.ToArray();
            mesh.RecalculateNormals();
            filter.mesh = mesh;
        }

        public Vector3 GeneratePoint(Vector2 position, GenerationSettings settings)
        {
            return new Vector3(position.x, Perlin(position.x, position.y, settings.frequency, settings.seed) * settings.maxHeight, position.y);
        }

        public float Perlin(float posx, float posy, float frequency, float seed)
        {
            float perlin = Mathf.PerlinNoise((posx + seed) * frequency, (posy + seed) * frequency);
            return perlin;
        }
    }
}
