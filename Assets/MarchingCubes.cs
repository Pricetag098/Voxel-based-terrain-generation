using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes : MonoBehaviour
{
    
    public ComputeShader meshGenerator;
    public float cutoff,scale;
    Mesh mesh;
    MeshFilter meshFilter;
    Vector3[] verts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("AAAAA");

    }

    public void GenMesh(int size, RenderTexture map)
	{
        if(meshFilter == null)
        meshFilter = GetComponent<MeshFilter>();

        
        mesh = new Mesh();

        mesh.Clear();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        
        

        meshGenerator.SetFloat("cutoff", cutoff);
        meshGenerator.SetFloat("_scale", scale);
        meshGenerator.SetInt("scale", size);
        meshGenerator.SetTexture(0, "Map", map);

        ComputeBuffer vertexBuffer = new ComputeBuffer((int)Mathf.Pow(size, 3), 12);
        ComputeBuffer triBuffer = new ComputeBuffer((int)Mathf.Pow(size, 3) * 16, 4);
        meshGenerator.SetBuffer(0, "Vertices", vertexBuffer);
        meshGenerator.SetBuffer(0, "Tris", triBuffer);


        meshGenerator.Dispatch(0, Mathf.CeilToInt((float)size / 8), Mathf.CeilToInt((float)size / 8), Mathf.CeilToInt((float)size / 8));
        verts = new Vector3[(int)Mathf.Pow(size, 3)];
        int[] tris = new int[(int)Mathf.Pow(size-1, 3) * 16];
        vertexBuffer.GetData(verts);
        triBuffer.GetData(tris);
        mesh.vertices = verts;
        //mesh.triangles = tris;
        meshFilter.mesh = mesh;
        vertexBuffer.Release();
        triBuffer.Release();
    }
    void OnDrawGizmos()
    {
        Debug.Log("AAA");
        if (verts != null)
            return;
        Gizmos.color = Color.red;
        foreach(Vector3 i in verts)
        {
            Gizmos.DrawSphere(i, .1f);
        }
    }
}
