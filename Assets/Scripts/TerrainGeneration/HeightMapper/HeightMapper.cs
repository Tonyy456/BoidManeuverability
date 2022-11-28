using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class HeightMapper : ITerrainAlgorithm
    {
        private GenerationSettings _settings;
        private MeshFilter filter;
        public HeightMapper(GenerationSettings settings, MeshFilter filer)
        {
            _settings = settings;
            this.filter = filer;
        }

        public void Generate(Vector3 position)
        {
            /*
             * Algorithm to create the points and their heights.
             */
            List<Vector3> meshPoints = new List<Vector3>();
            List<Color> vertexColors = new List<Color>();
            Vector2 start = new Vector2(position.x - _settings.resolution.x * _settings.pointSeperation/2, position.z - _settings.resolution.z * _settings.pointSeperation / 2);
            for(int x = 0; x < _settings.resolution.x; x++)
            {
                for(int y = 0; y < _settings.resolution.y; y++)
                {
                    Vector2 xyPos = new Vector2(start.x + _settings.pointSeperation * x, start.y + _settings.pointSeperation * y);
                    Vector3 pos = GeneratePoint(xyPos);
                    Color c = _settings.HeightColorGradient.Evaluate(pos.y);
                    meshPoints.Add(pos);
                    vertexColors.Add(c);
                }
            }

            /*
             * Algorithm to create triangles in winding order.
             * len(indices) == ((resolution - 1) ^ 2) * 6
             */
            List<int> indices = new List<int>();
            int width = _settings.resolution.x;
            for(int x = 0; x < _settings.resolution.x - 1; x++)
            {
                for(int y = 0; y< _settings.resolution.x - 1; y++)
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

        public Vector3 GeneratePoint(Vector2 position)
        {
            return new Vector3(position.x, Perlin(position.x, position.y) * _settings.maxHeight, position.y);
        }

        public float Perlin(float posx, float posy)
        {
            float perlin = Mathf.PerlinNoise((posx + _settings.seed) * _settings.frequency, (posy + _settings.seed) * _settings.frequency);
            return perlin;
        }
    }
}
