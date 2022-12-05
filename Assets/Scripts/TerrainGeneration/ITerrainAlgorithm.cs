using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version3
{
    public interface ITerrainAlgorithm
    {
        public IEnumerator Generate(Vector3Int chunk, MeshFilter filter, MeshCollider collider, bool signal = false);
        public void DrawBounds();
        public void CreateBounds(Material material);
    }
}
