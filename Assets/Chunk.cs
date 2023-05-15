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
    MarchingCubes2 marchingCubes;
    public Vector3Int chunkIndex;
    public RenderTexture map;
    
    private void Awake()
    {
        marchingCubes = GetComponent<MarchingCubes2>();
        
    }


    public void UpdateMap(int size)
    {
        
        marchingCubes.run(map, size);
        


        
        
    }

    

    void GetData(ComputeBuffer buffer, int size)
    {
        if (buffer == null)
            return;
        //map = new Vector4[size * size * size];
        //buffer.GetData(map);
        //await Task.CompletedTask;
    }

    struct GetDataJob : IJob 
    {
        public ComputeBuffer buffer;
        public int size;
        //public Chunk chunk;
        public NativeArray<Vector4> _map;
        public void Execute()
        {
            if (buffer == null)
                return;
            Vector4[] map = new Vector4[size * size * size];
            buffer.GetData(map);
            _map.CopyFrom(map);
        }
    }

}
