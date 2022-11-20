using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version2
{
    public class HeightMapper : ITerrainAlgorithm
    {
        private int _resolution;
        private float _frequency;
        private float _maxHeight;
        private float _squareDistance;
        private float _seed;

        private FlatMeshGenerator meshGen;
        public HeightMapper(float squareDistance, int resolution, float frequency, float maxHeight, float seed = 0)
        {
            _frequency = frequency;
            _maxHeight = maxHeight;
            _squareDistance = squareDistance;
            _resolution = resolution;
            _seed = seed;
            meshGen = new FlatMeshGenerator();
        }
        private void UpdateMesh(Mesh m)
        {
            m.RecalculateNormals();
        }
        public Mesh GenerateTerrain()
        {
            Mesh mesh = meshGen.GenerateMesh(_squareDistance, _resolution, new Vector3(0f, 0f, 0f));
            MapTerrain(mesh);
            UpdateMesh(mesh);
            return mesh;
        }

        public void UpdateMesh(Vector3 position)
        {

        }

        public Mesh GenerateAndColor(Gradient gradient)
        {
            Mesh mesh = meshGen.GenerateMesh(_squareDistance, _resolution, new Vector3(0f, 0f, 0f));
            MapTerrainWithColor(mesh, gradient);
            UpdateMesh(mesh);
            return mesh;
        }

        private void MapTerrain(Mesh m)
        {
            Vector3[] vertices = m.vertices;
            for (int i = 0; i < m.vertexCount; i++)
            {
                Vector3 position = vertices[i];
                float perlin = Perlin(position.x, position.z);
                float newHeight = perlin * perlin * _maxHeight;
                vertices[i].y = newHeight;
            }
            m.vertices = vertices;
        }

        private void MapTerrainWithColor(Mesh m, Gradient gradient)
        {
            Vector3[] vertices = m.vertices;
            List<Color> colors = new List<Color>();
            for (int i = 0; i < m.vertexCount; i++)
            {
                Vector3 position = vertices[i];
                float perlin = Perlin(position.x, position.z);
                float newHeight = perlin * perlin * _maxHeight;
                colors.Add(gradient.Evaluate(newHeight / _maxHeight));
                vertices[i].y = newHeight;
            }
            m.vertices = vertices;
            m.colors = colors.ToArray();
        }

        private float Perlin(float posx, float posy)
        {
            float perlin = Mathf.PerlinNoise((posx + _seed + _squareDistance) * _frequency, (posy + _seed + _squareDistance) * _frequency);
            return perlin;
        }

        public void ClearTerrain() { }
    }
}
