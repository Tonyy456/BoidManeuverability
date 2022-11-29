using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public interface ITerrainAlgorithm
    {
        public void Generate(Vector3 position, GenerationSettings settings);
    }
}
