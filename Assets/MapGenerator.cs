using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int size;

    public float freq, amp;

    public float la, per;
    public int octaves = 1;
    public RenderTexture map;
    public ComputeShader mapGenerator;

    public void GenMap()
    {
        map = new RenderTexture(size, size, 0, RenderTextureFormat.RFloat);
        map.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        map.volumeDepth = size;

        map.enableRandomWrite = true;
        map.Create();
        int kernel = 
        mapGenerator.FindKernel("GenMap");
        mapGenerator.SetTexture(kernel, "Map", map);
        mapGenerator.SetFloat("freq", freq);
        mapGenerator.SetFloat("amp", amp);
        mapGenerator.SetFloat("la", la);
        mapGenerator.SetFloat("per", per);
        mapGenerator.SetInt("octaves", octaves);
        //mapGenerator.SetFloat("cutoff", cutoff);
        mapGenerator.SetInt("scale", size);
        mapGenerator.Dispatch(kernel, Mathf.CeilToInt(size+1 / 8), Mathf.CeilToInt(size+1 / 8), Mathf.CeilToInt(size+1 / 8));
        GetComponent<MarchingCubes>().GenMesh(size, map);
    }
    
}
