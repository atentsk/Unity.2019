using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if (GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();

        GetComponent<MeshFilter>().mesh = createTriangle();

        var mat = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");

        GetComponent<MeshRenderer>().material = mat;
        //GetComponent<MeshRenderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Mesh createTriangle()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertex = new Vector3[3];
        Color[] colors = new Color[3];
        Vector2[] uvs = new Vector2[3];

        vertex[0] = Vector3.zero;
        colors[0] = Color.red;
        uvs[0] = Vector2.zero;

        vertex[1] = new Vector3(1, 0, 0);
        colors[1] = Color.green;
        uvs[1] = new Vector2(1, 0);

        vertex[2] = new Vector3(0, 1, 0);
        colors[2] = Color.blue;
        uvs[2] = new Vector2(0, 1);

        int[] tris = new int[3];
        tris[0] = 0;
        tris[1] = 2;
        tris[2] = 1;

        //Assign data to mesh
        mesh.vertices = vertex;
        mesh.colors = colors;
        mesh.uv = uvs;
        mesh.triangles = tris;

        //mesh.RecalculateNormals();

        mesh.name = "triangle";

        return mesh;
    }
    Mesh CreateCircle(float r_, int num, float start_degree, float seg_degree)
    {
        int x;
        Mesh mesh = new Mesh();

        //Vertices
        Vector3[] vertex = new Vector3[num + 2];
        //UVs
        Vector2[] uvs = new Vector2[num + 2];
        //Vector3 []normals = new Vector3[num+2];

        vertex[num + 1].x = 0;
        vertex[num + 1].y = 0;
        vertex[num + 1].z = 0;

        uvs[num + 1].x = 0;
        uvs[num + 1].y = 0;

        // Vertex
        for (x = 0; x <= num; x++)
        {
            vertex[x].x = r_ * Mathf.Cos(Mathf.PI * 2 * ((start_degree) + x * (seg_degree / num)));
            vertex[x].y = r_ * Mathf.Sin(Mathf.PI * 2 * ((start_degree) + x * (seg_degree / num)));
            vertex[x].z = 0;
        }

        // UVs
        for (x = 0; x <= num; x++)
        {
            float a_ = Mathf.PI * 2 * (x * (seg_degree / num));

            uvs[x].x = Mathf.Abs(Mathf.Cos(a_));
            uvs[x].y = Mathf.Abs(Mathf.Sin(a_));
        }

        //Triangles
        int[] tris = new int[3 * (num)];    //3 verts per triangle * num triangles
        int C1, C2, C3;
        {
            C1 = num + 1;
            C2 = 0;
            C3 = 1;
            for (x = 0; x < tris.Length; x += 3)
            {
                tris[x] = C1;
                tris[x + 1] = C3;
                tris[x + 2] = C2;

                C2++;
                C3++;
            }
        }

        //Assign data to mesh
        mesh.vertices = vertex;
        //mesh.colors = colors;
        //mesh.normals = normals;
        mesh.uv = uvs;
        //mesh.uv2 = uvs;
        mesh.triangles = tris;

        //Recalculations
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        //Name the mesh
        mesh.name = "MyMesh";

        //Return the mesh
        return mesh;
    }
}
