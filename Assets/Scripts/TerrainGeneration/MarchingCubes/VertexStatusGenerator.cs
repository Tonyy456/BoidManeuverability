using UnityEngine;

public interface VertexStatusGenerator
{
    public bool[] getStatusForCube(int x, int y, int z);
}
