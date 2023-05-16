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
    Vector3 randOffset;
    
    private void Start()
    {
        randOffset = Vector3.one;
        randOffset.y = 0;
        randOffset *= Random.value * 100000;
        
    }
    //public void GenMap()
    //{
    //    map = new Vector4[size * size * size];
    //    buffer = GetMap(localOffset);
    //    buffer.GetData(map);
    //}

    public static RenderTexture CreateRt(int _size)
    {
        RenderTexture rt = new RenderTexture(_size, _size, 0, RenderTextureFormat.RHalf);
        rt.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        rt.volumeDepth = _size;
        rt.enableRandomWrite = true;
        rt.Create();
        return rt;
    }

    public RenderTexture GetMap(Vector3 offset)
    {

        RenderTexture rt = CreateRt(size);

        map = new Vector4[size * size * size];
        int kernel =
        mapGenerator.FindKernel("GenMap");
        
        mapGenerator.SetTexture(kernel, "Map", rt);
        mapGenerator.SetFloat("freq", freq);
        mapGenerator.SetFloat("amp", amp);
        mapGenerator.SetFloat("la", la);
        mapGenerator.SetVector("offset", offset + randOffset);
        mapGenerator.SetFloat("per", per);
        mapGenerator.SetInt("octaves", octaves);
        //mapGenerator.SetFloat("cutoff", cutoff);
        mapGenerator.SetInt("scale", size);
        mapGenerator.Dispatch(kernel, Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8));

        return rt;
    }

    public void GenMesh()
    {
        
        MarchingCubes marchingCubes2 = GetComponent<MarchingCubes>();
        if(marchingCubes2 != null)
        {
            //marchingCubes2.run(buffer,size);
        }
        else
        {
            MarchingCubesOld marchingCubes = GetComponent<MarchingCubesOld>();
            if(marchingCubes != null)
            {
                //buffer.GetData(map);
                marchingCubes.run(map,size);
            }
        }
    }
    public void DisposeBuffers()
    {
        //buffer.Dispose();
    }
    
}
