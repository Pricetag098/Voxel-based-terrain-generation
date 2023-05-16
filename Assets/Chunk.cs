using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
public class Chunk : MonoBehaviour
{
    //[HideInInspector]public Vector4[] map;
    MarchingCubes marchingCubes;
    public Vector3Int chunkIndex;
    public RenderTexture map;
    
    private void Awake()
    {
        marchingCubes = GetComponent<MarchingCubes>();
        
    }

    private void Start()
    {
        
    }

    public void SetMapAndPrune(int size)
    {
        if (!marchingCubes.Run(map, size))
            map = null;
    }
    public void UpdateMap(int size)
    {
        marchingCubes.Run(map, size);
            

    }

    

    void GetData(ComputeBuffer buffer, int size)
    {
        if (buffer == null)
            return;
        //map = new Vector4[size * size * size];
        //buffer.GetData(map);
        //await Task.CompletedTask;
    }
    

}
