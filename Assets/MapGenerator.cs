using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int size;

    public float freq, amp;
    public Vector3 offset;
    public float la, per;
    public int octaves = 1;
    public float[] map;
    public ComputeShader mapGenerator;

    public void GenMap()
    {
        //map = new RenderTexture(size, size, 0, RenderTextureFormat.RFloat);
        //map.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        //map.volumeDepth = size;

        //map.enableRandomWrite = true;
        //map.Create();
        map = new float[size * size * size];
        int kernel = 
        mapGenerator.FindKernel("GenMap");
        //mapGenerator.SetTexture(kernel, "Map", map);
        ComputeBuffer buffer = new ComputeBuffer(map.Length, 4);
        buffer.SetData(map);
        mapGenerator.SetBuffer(kernel,"Map", buffer);
        mapGenerator.SetFloat("freq", freq);
        mapGenerator.SetFloat("amp", amp);
        mapGenerator.SetFloat("la", la);
        mapGenerator.SetVector("offset", offset);
        mapGenerator.SetFloat("per", per);
        mapGenerator.SetInt("octaves", octaves);
        //mapGenerator.SetFloat("cutoff", cutoff);
        mapGenerator.SetInt("scale", size);
        mapGenerator.Dispatch(kernel, Mathf.CeilToInt((size * size * size) / 256),1,1);
        buffer.GetData(map);
        //GetComponent<MarchingCubes>().GenMesh(size, map);
        buffer.Release();
    }

    public void GenMesh()
    {
        MarchingCubes marchingCubes = GetComponent<MarchingCubes>();
        if(marchingCubes != null)
        {
            marchingCubes.run(map,size);
        }
    }
    
}
