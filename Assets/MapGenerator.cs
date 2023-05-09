using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int size;

    public float freq, amp;
    public Vector3 localOffset;
    public float la, per;
    public int octaves = 1;
    public Vector4[] map;
    public ComputeShader mapGenerator;
    ComputeBuffer buffer;
    
    private void Start()
    {
        buffer = new ComputeBuffer((size) * size * size, 4 * 4);
        
    }
    public void GenMap()
    {
        map = new Vector4[size * size * size];
        buffer = GetMap(localOffset);
        buffer.GetData(map);
    }

    public ComputeBuffer GetMap(Vector3 offset)
    {
        map = new Vector4[size * size * size];
        int kernel =
        mapGenerator.FindKernel("GenMap");
        buffer.SetData(map);
        mapGenerator.SetBuffer(kernel, "Map", buffer);
        mapGenerator.SetFloat("freq", freq);
        mapGenerator.SetFloat("amp", amp);
        mapGenerator.SetFloat("la", la);
        mapGenerator.SetVector("offset", offset);
        mapGenerator.SetFloat("per", per);
        mapGenerator.SetInt("octaves", octaves);
        //mapGenerator.SetFloat("cutoff", cutoff);
        mapGenerator.SetInt("scale", size);
        mapGenerator.Dispatch(kernel, Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8));

        return buffer;
    }

    public void GenMesh()
    {
        
        MarchingCubes2 marchingCubes2 = GetComponent<MarchingCubes2>();
        if(marchingCubes2 != null)
        {
            marchingCubes2.run(buffer,size);
        }
        else
        {
            MarchingCubes marchingCubes = GetComponent<MarchingCubes>();
            if(marchingCubes != null)
            {
                buffer.GetData(map);
                marchingCubes.run(map,size);
            }
        }
    }
    public void DisposeBuffers()
    {
        buffer.Dispose();
    }
    
}
