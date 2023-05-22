using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int size;

    
    public Vector3 localOffset;
    [SerializeReference] List<MapGeneratorPass> mapGeneratorPasses = new List<MapGeneratorPass>();
    
    Vector3 randOffset;
    
    private void Start()
    {
        randOffset = Vector3.one;
        randOffset.y = 0;
        randOffset *= Random.value * 100000;
        
    }
    

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
        foreach (MapGeneratorPass pass in mapGeneratorPasses)
        {
            pass.Run(rt, size,offset + randOffset);
        }
        

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
                //marchingCubes.run(map,size);
            }
        }
    }
    public void DisposeBuffers()
    {
        //buffer.Dispose();
    }
    
}
