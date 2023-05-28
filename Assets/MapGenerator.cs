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
    Queue<RenderTexture> mapQueue = new Queue<RenderTexture>();
    
    private void Awake()
	{
        ChunkManager chunkManager = GetComponent<ChunkManager>();

        if(chunkManager != null)
		{
            size = chunkManager.chunkSize;

            // fill the map queue
            int rtCount = chunkManager.bounds.x * 2 * chunkManager.bounds.y * 2 * chunkManager.bounds.z * 2;
            for(int i = 0; i < rtCount; i++)
			{
                mapQueue.Enqueue(CreateRt(size));
			}
        }
        
    }
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
    public void ReturnMap(RenderTexture map)
    {
        mapQueue.Enqueue(map);
    }
    public RenderTexture GetMap(RenderTexture rt, Vector3 offset)
    {
        if (rt == null)
		{
            if (mapQueue.Count > 0)
            {
                if (rt == null) rt = mapQueue.Dequeue();
            }
			else
			{
                Debug.LogError("No Maps Left in Queue");
			}
        }
            
        
        foreach (MapGeneratorPass pass in mapGeneratorPasses)
        {
            pass.Run(rt, size,offset + randOffset);
        }
        

        return rt;
    }

    
    
    
}
