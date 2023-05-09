using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MarchingCubes2 : MonoBehaviour
{
    public float cutoff = 0;
    public ComputeShader Shader;
    int size;
    Mesh mesh;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    // Start is called before the first frame update


    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    struct Triangle 
    {
        Vector3 a;
        Vector3 b;
        Vector3 c;

        public Vector3 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    default:
                        return c;
                }
            }
        }
    }


    public void run(ComputeBuffer map, int _size)
    {
        size = _size;

        //set stuff
        Shader.SetBuffer(0,"points",map);
        Shader.SetInt("scale", size);
        Shader.SetFloat("_cutoff", cutoff);
        int maxTriCount = (size - 1) * (size - 1) * (size - 1);
        ComputeBuffer triBuffer = new ComputeBuffer(maxTriCount, 36,ComputeBufferType.Append);
        ComputeBuffer triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        triBuffer.SetCounterValue(0);
        Shader.SetBuffer(0, "Tris", triBuffer);
        Shader.Dispatch(0, Mathf.CeilToInt((size+1) / 8), Mathf.CeilToInt((size + 1) / 8), Mathf.CeilToInt((size + 1) / 8));


        // Get number of triangles in the triangle buffer
        ComputeBuffer.CopyCount(triBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];

        //numTris = maxTriCount;
        Triangle[] tris = new Triangle[numTris];
        triBuffer.GetData(tris,0,0,numTris);

        Vector3[] vertices = new Vector3[numTris * 3];
        int[] triangles = new int[numTris * 3];
        
        //Debug.Log(triangles.Length);
        for(int i = 0; i < numTris; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                triangles[i * 3 + j] = i * 3 + j;
                vertices[i * 3 + j] = tris[i][j];
            }
        }
        
        mesh = new Mesh();

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        triBuffer.Dispose();
        triCountBuffer.Dispose();
        //map.Dispose();
    }
    
}
