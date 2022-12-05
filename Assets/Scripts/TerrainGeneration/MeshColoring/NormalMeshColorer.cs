using UnityEngine;

/*
 * NormalMeshColors takes a gradient and based on the mesh normals, it linearly
 * evaluates the angle between the normal and Vector3.up on the interval
 * [0,180] and maps it to [0,1] and evaluates it on the gradient.
 */
public class NormalMeshColorer : IMeshColorer
{
    private Mesh mesh;
    private Gradient gradient;
    public NormalMeshColorer(Mesh mesh, Gradient gradient)
    {
        this.mesh = mesh;
        this.gradient = gradient;
    }
    public void Color()
    {
        Vector3 up = Vector3.up;
        Color[] colors = new Color[mesh.normals.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            float percent = Vector3.Angle(Vector3.up, mesh.normals[i]) / 180f;
            Color c = gradient.Evaluate(percent);
            colors[i] = c;
        }
        mesh.colors = colors;
    }
}
