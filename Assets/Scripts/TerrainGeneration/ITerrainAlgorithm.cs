using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITerrainAlgorithm
{
    public Mesh GenerateTerrain();
    public void ClearTerrain();
    public Mesh GenerateAndColor(Gradient gradient);
    public void UpdateMesh(Vector3 position);
}
