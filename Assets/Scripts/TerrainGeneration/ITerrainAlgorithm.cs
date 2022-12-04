using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public interface ITerrainAlgorithm
    {
        public IEnumerator Generate();
        public IEnumerator Generate(Vector3Int chunk, MeshFilter filter);
        public void DrawBounds();
    }
}
