using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class MarchingSquares : ITerrainAlgorithm
    {
        private GenerationSettings _settings;
        private MeshFilter _filter;
        public MarchingSquares(GenerationSettings settings, MeshFilter filter)
        {
            _settings = settings;
            _filter = filter;
        }
        public void Generate(Vector3 position)
        {

        }

        /*
         * https://answers.unity.com/questions/938178/3d-perlin-noise.html
         */

        public float PerlinNoise3D(float x, float y, float z)
        {
            y += 1;
            z += 2;
            float xy = _perlin3DFixed(x, y);
            float xz = _perlin3DFixed(x, z);
            float yz = _perlin3DFixed(y, z);
            float yx = _perlin3DFixed(y, x);
            float zx = _perlin3DFixed(z, x);
            float zy = _perlin3DFixed(z, y);
            return xy * xz * yz * yx * zx * zy;
        }
        public float _perlin3DFixed(float a, float b)
        {
            return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a * _settings.frequency, b * _settings.frequency));
        }

        /*
         * values go:
         * 111,
         * 11-1,
         * 1-11,
         * -111,
         * -11-1,
         * 1-1-1,
         * -1-1-1,
         * -1-11
         */
        public Mesh GenerateForSingleSquare(Dictionary<Vector3, float> pointToNoise)
        {
            return null;
        }

    }
}
