using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    Chunk[,,] chunks;
    public float offset;
    public Vector3 baseOffset;
    public int chunkSize;
    public Vector3Int size;
    MapGenerator mapGenerator;
    
    // Start is called before the first frame update
    async void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();
        chunks = new Chunk[size.x, size.y, size.z];
        mapGenerator.size = chunkSize;
        for(int x = 0; x < size.x; x++)
        {
            for( int y = 0; y < size.y; y++)
            {
                for(int z = 0; z < size.z; z++)
                {
                    await PlaceChunk(x, y, z);
                    
                }
            }
        }
        mapGenerator.DisposeBuffers();
    }

    async Task PlaceChunk(int x, int y, int z)
    {
        GameObject chunkGo = Instantiate(chunkPrefab, transform);
        chunkGo.transform.position = new Vector3(x, y, z) * offset + baseOffset;
        chunkGo.layer = gameObject.layer;
        Chunk chunk = chunkGo.GetComponent<Chunk>();
        chunk.chunkIndex = new Vector3Int(x, y, z);
        chunk.SetMap(mapGenerator.GetMap(new Vector3(x, y, z) * offset + baseOffset), chunkSize);
        chunks[x, y, z] = chunk;
        await Task.CompletedTask;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Chunk GetChunk(Vector3Int index)
    {
        return chunks[index.x,index.y,index.z];
    }
}
