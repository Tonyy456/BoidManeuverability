using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class HeightMapper : ITerrainAlgorithm
    {
        private MeshFilter filter;
        private GenerationSettings settings;
        private Vector3 position;
        public HeightMapper(GenerationSettings settings, MeshFilter filer, Vector3 position)
        {
            this.settings = settings;
            this.filter = filer;
            this.position = position;
        }

        public IEnumerator Generate()
        {
            FlatMeshGenerator generator = new FlatMeshGenerator(settings.center, settings.resolution, settings.pointSeperation);
            var definition = generator.getMeshDefintion();
            Vector3[] vertices = addNoise(definition.vertices);
            int[] triangles = definition.triangles;

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            mesh.RecalculateNormals();
            mesh.Optimize();

            IMeshColorer colorer = new HeightMeshColorer(mesh, settings.HeightColorGradient);
            colorer.Color();

            filter.mesh = mesh;
            yield return null;
        }
        public void DrawBounds()
        {

        }

        private Vector3[] addNoise(Vector3[] points)
        {
            Vector3[] noise = new Vector3[points.Length];
            for(int i = 0; i < points.Length; i++)
            {
                Vector3 point_with_noise = points[i];
                point_with_noise.y = PerlinNoise.Noise2D(point_with_noise, settings.frequency, settings.seed);
                point_with_noise.y *= settings.maxHeight;
                noise[i] = point_with_noise;
            }
            return noise;
        }
    }
}
