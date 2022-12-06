using UnityEngine;
/* 
 * Written By: Tony D'Alesandro
 */

public interface VertexStatusGenerator
{
    public bool[] getStatusForCube(int x, int y, int z);
}
