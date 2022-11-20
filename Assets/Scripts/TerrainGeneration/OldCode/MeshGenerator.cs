using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Version1
{
    public class MeshGenerator
    {
        private Mesh mesh;
        private Vector3[] vertices;
        private int[] triangles;

        private int meshWidth;
        private int meshHeight;
        private Vector3 center;
        private float pointSeperation;

        private const int VERTS_IN_QUAD = 6;

        public MeshGenerator(int width, int height, float pointSeperation, Vector3 center)
        {
            mesh = new Mesh();
            meshWidth = width;
            meshHeight = height;
            this.pointSeperation = pointSeperation;
            this.center = center;
            CreatePoints();
            CreateTriangles();
            UpdateMesh();
        }

        public Mesh GetMesh()
        {
            return mesh;
        }

        private void UpdateMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        private void CreatePoints()
        {
            Vector3 start = new Vector3(center.x - (meshWidth * pointSeperation) / 2, center.y, center.z - (meshHeight * pointSeperation) / 2);
            int numVertices = meshWidth * meshHeight;
            vertices = new Vector3[numVertices];

            //Set where the points are
            for (int x = 0; x < meshWidth; x++)
            {
                for (int z = 0; z < meshHeight; z++)
                {
                    vertices[x + meshWidth * z] = new Vector3(start.x + x * pointSeperation, start.y, start.z + z * pointSeperation);
                }
            }
        }

        private void CreateTriangles()
        {
            int numQuads = (meshWidth - 1) * (meshHeight - 1);
            triangles = new int[numQuads * VERTS_IN_QUAD];
            //Create triangles out of the points in vertices
            for (int x = 0; x < (meshWidth - 1); x++)
            {
                for (int y = 0; y < (meshHeight - 1); y++)
                {
                    //BottomLeft triangle of quad in correct winding order
                    int triangleI = x + (meshWidth - 1) * y; //vertex into triangle array
                    int verticesI = x + meshWidth * y; //vertex into triangle vertex array
                    triangles[VERTS_IN_QUAD * triangleI] = verticesI;
                    triangles[VERTS_IN_QUAD * triangleI + 1] = verticesI + meshWidth;
                    triangles[VERTS_IN_QUAD * triangleI + 2] = verticesI + meshWidth + 1;

                    //TopRight triangle of quad in correct winding order
                    triangles[VERTS_IN_QUAD * triangleI + 3] = verticesI + meshWidth + 1;
                    triangles[VERTS_IN_QUAD * triangleI + 4] = verticesI + 1;
                    triangles[VERTS_IN_QUAD * triangleI + 5] = verticesI;
                }
            }
        }
    }
}
