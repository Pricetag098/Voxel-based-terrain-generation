using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [SerializeField]ComputeShader shader;
    public ChunkManager chunkManager;
    ComputeBuffer mapBuffer;
    int size;
    // Start is called before the first frame update
    void Start()
    {
        size = chunkManager.GetComponent<MapGenerator>().size;
        mapBuffer = new ComputeBuffer((size) * size * size, 4 * 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            
            if(Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Chunk hitChunk = hit.collider.gameObject.GetComponent<Chunk>();
                
                List<Chunk> chunks = new List<Chunk>();
                for(int x = -1; x < 1; x++)
                {
                    if (x + hitChunk.chunkIndex.x < 0 || x + hitChunk.chunkIndex.x >= size)
                        continue;
                    for (int y = -1; y < 1; y++)
                    {
                        if (y + hitChunk.chunkIndex.y < 0 || y + hitChunk.chunkIndex.y >= size)
                            continue;
                        for (int z = -1; z < 1; z++)
                        {
                            if (z + hitChunk.chunkIndex.z < 0 || z + hitChunk.chunkIndex.z >= size)
                                continue;
                            chunks.Add(chunkManager.GetChunk(hitChunk.chunkIndex + new Vector3Int(x, y, z)));
                        }
                    }
                }
                
                foreach(Chunk chunk in chunks)
                {
                    mapBuffer.SetData(chunk.map);
                    shader.SetBuffer(0, "Map", mapBuffer);
                    shader.SetVector("pos", hit.point - chunk.transform.position);
                    shader.SetInt("scale", size);
                    shader.Dispatch(0, Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8));
                    chunk.SetMap(mapBuffer, size);
                }
                
                
            }
        }
    }
}
