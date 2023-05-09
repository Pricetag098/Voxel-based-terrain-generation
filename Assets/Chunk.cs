using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [HideInInspector]public Vector4[] map;
    MarchingCubes2 marchingCubes;
    public Vector3Int chunkIndex;
    private void Awake()
    {
        marchingCubes = GetComponent<MarchingCubes2>();
    }

    public async void SetMap(ComputeBuffer buffer, int size)
    {
        if (buffer == null)
            return;
        marchingCubes.run(buffer, size);
       
        await GetData(buffer,size);
        //buffer.Dispose();
    }
    async Task GetData(ComputeBuffer buffer, int size)
    {
        if (buffer == null)
            return;
        map = new Vector4[size * size * size];
        buffer.GetData(map);
        await Task.CompletedTask;
    }
}
