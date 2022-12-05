using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class HeightMapper : ITerrainAlgorithm
    {
        private GenerationSettings settings;
        public HeightMapper(GenerationSettings settings)
        {
            this.settings = settings;
        }

        public void DrawBounds() { }
        public void CreateBounds(Material material) { }

        public IEnumerator Generate(Vector3Int chunk, MeshFilter filter, bool signal = false)
        {
            FlatMeshGenerator generator = new FlatMeshGenerator(settings.chunkCenter(chunk), settings.cubesPerChunk + new Vector3Int(1,1,1), settings.edgeLen);
            var definition = generator.getMeshDefintion();
            Vector3[] vertices = addNoise(definition.vertices);
            int[] triangles = definition.triangles;

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            PostMeshCreation(mesh);

            filter.mesh = mesh;
            yield return null;
        }

        private void PostMeshCreation(Mesh mesh)
        {
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            mesh.RecalculateNormals();
            mesh.Optimize();

            IMeshColorer colorer = new HeightMeshColorer(mesh, settings.ColorGradient);
            colorer.Color();
        }

        private Vector3[] addNoise(Vector3[] points)
        {
            Vector3[] noise = new Vector3[points.Length];
            for(int i = 0; i < points.Length; i++)
            {
                Vector3 point_with_noise = points[i];
                point_with_noise.y = PerlinNoise.Noise2D(point_with_noise, settings.frequency, settings.seed);
                point_with_noise.y *= settings.heightMultipler;
                noise[i] = point_with_noise;
            }
            return noise;
        }
    }
}
