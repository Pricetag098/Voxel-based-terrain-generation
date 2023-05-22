using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;

    Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();
    Dictionary<Vector3Int, RenderTexture> maps = new Dictionary<Vector3Int, RenderTexture>();
    public float offset;
    public Vector3 baseOffset;
    public int chunkSize;
    public Vector3Int bounds;
    MapGenerator mapGenerator;

    public GameObject player;

    List<Chunk> chunkPool = new List<Chunk>();
    // Start is called before the first frame update
    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();
        //chunks = new Chunk[size.x, size.y, size.z];
        mapGenerator.size = chunkSize;
        for(int x = 0; x < bounds.x; x++)
        {
            for( int y = 0; y < bounds.y; y++)
            {
                for(int z = 0; z < bounds.z; z++)
                {
                    GameObject chunkGo = Instantiate(chunkPrefab, transform);
                    chunkGo.layer = gameObject.layer;
                    
                    Chunk chunk = chunkGo.GetComponent<Chunk>();
                    PlaceChunk(chunk, x, y, z);
                    
                }
            }
        }
        mapGenerator.DisposeBuffers();
    }

    async Task PlaceChunk(Chunk chunk, int x, int y, int z)
    {
        chunk.transform.position = new Vector3(x, y, z) * offset + baseOffset;

        chunk.chunkIndex = new Vector3Int(x, y, z);
        if (maps.ContainsKey(chunk.chunkIndex))
        {
            if(maps[chunk.chunkIndex] == null)
            {
                chunk.map = GetMap(new Vector3(x, y, z));
                maps[chunk.chunkIndex] = chunk.map;
            }
            chunk.map = maps[chunk.chunkIndex];
            
        }
        else
        {
            chunk.map = GetMap(new Vector3(x, y, z));
            maps.Add(chunk.chunkIndex, chunk.map);
        }
        chunks.Add(chunk.chunkIndex, chunk);
        chunk.SetMapAndPrune(chunkSize);
        
        await Task.CompletedTask;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int playerChunkCoord = new Vector3Int(
            Mathf.FloorToInt(player.transform.position.x / offset),
            Mathf.FloorToInt(player.transform.position.y / offset),
            Mathf.FloorToInt(player.transform.position.z / offset));
        int maxX = playerChunkCoord.x + bounds.x / 2;
        int minX = playerChunkCoord.x - bounds.x / 2;
        Debug.Log(minX);
        for(int y = playerChunkCoord.y - bounds.y/2; y < playerChunkCoord.y + bounds.y / 2; y++)
        {
            for (int z = playerChunkCoord.z - bounds.z / 2; z < playerChunkCoord.z + bounds.z / 2; z++)
            {
                Vector3Int chunkCoord = new Vector3Int(minX, y, z);
                if (chunks.ContainsKey(chunkCoord))
                {
                    UnloadChunk(chunks[chunkCoord]);
                }
                else
                {
                    Debug.Log("A");
                }
            }
        }
    }

    void UnloadChunk(Chunk chunk)
    {
        
        chunkPool.Add(chunk);
        chunks.Remove(chunk.chunkIndex);
        chunk.gameObject.SetActive(false);
    }

    public RenderTexture GetMap(Vector3 pos)
    {
        return mapGenerator.GetMap(pos * offset + baseOffset);
    }
}
