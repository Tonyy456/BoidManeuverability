using UnityEngine;

public class HeightMeshColorer : IMeshColorer
{
    private Mesh mesh;
    private Gradient gradient;
    public HeightMeshColorer(Mesh mesh, Gradient gradient)
    {
        this.mesh = mesh;
        this.gradient = gradient;
    }
    public void Color()
    {
        Color[] colors = new Color[mesh.normals.Length];
        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            Vector3 point = mesh.vertices[i];
            if (point.y > maxHeight) maxHeight = point.y;
            if (point.y < minHeight) minHeight = point.y;
        }

        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            //LERP
            float percent = (mesh.vertices[i].y - minHeight) / (maxHeight - minHeight);
            Color c = gradient.Evaluate(percent);
            colors[i] = c;
        }
        mesh.colors = colors;
    }
}
