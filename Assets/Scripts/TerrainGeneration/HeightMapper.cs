using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public class HeightMapper : ITerrainAlgorithm
    {
        private GenerationSettings _settings;
        public HeightMapper(GenerationSettings settings)
        {
            _settings = settings;
        }

        private float Perlin(float posx, float posy)
        {
            float perlin = Mathf.PerlinNoise((posx + _settings.seed + _settings.distance) * _settings.frequency, (posy + _settings.seed + _settings.distance) * _settings.frequency);
            return perlin;
        }
    }
}
