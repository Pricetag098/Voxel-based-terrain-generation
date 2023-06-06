using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class CunkSaver : MonoBehaviour
{
    
    ComputeBuffer buffer;
    ChunkManager chunkManager;
    // Start is called before the first frame update
    void Start()
    {
        chunkManager = GetComponent<ChunkManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SaveMap(RenderTexture map)
    {
        

        NativeArray<byte> narray = new NativeArray<byte>((int)Mathf.Pow(chunkManager.chunkSize, 3) * 2, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

        // request the texture data back from the GPU:
        var request = AsyncGPUReadback.RequestIntoNativeArray(ref narray, buffer, (AsyncGPUReadbackRequest request) =>
        {
            if (!request.hasError)
            {
                //DO saving and stuff
                Debug.Log("Eat Shit");
            }
            narray.Dispose();
            
        });

    }
}
