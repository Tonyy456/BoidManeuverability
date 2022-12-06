using UnityEngine;

/*
 * Author: Anthony D'Alesandro
 * 
 * HeightMeshColorer - 
 * linearly maps the max height to the max value in the gradient
 * and the minimum height to the minimum value in the gradient
 */
public class HeightMeshColorer : IMeshColorer
{
    private Mesh mesh;
    private Gradient gradient;
    private (float min, float max) interval;
    public HeightMeshColorer(Mesh mesh, Gradient gradient, (float min, float max) interval)
    {
        this.mesh = mesh;
        this.gradient = gradient;
        this.interval = interval;
    }
    public void Color()
    {
        Color[] colors = new Color[mesh.normals.Length];
        Debug.Log(mesh);
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            //LERP
            float percent = (mesh.vertices[i].y - interval.min) / (interval.max - interval.min);
            Color c = gradient.Evaluate(percent);
            colors[i] = c;
        }
        mesh.SetColors(colors);
    }
}
