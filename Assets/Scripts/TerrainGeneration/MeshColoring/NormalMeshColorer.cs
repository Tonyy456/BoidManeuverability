using UnityEngine;

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
