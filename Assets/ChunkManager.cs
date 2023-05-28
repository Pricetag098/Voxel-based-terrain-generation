using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public bool staticMap = false;
    [SerializeField] GameObject chunkPrefab;
    
    Dictionary<Vector3Int, Chunk> existingChunks = new Dictionary<Vector3Int, Chunk>();
    Dictionary<Vector3Int, RenderTexture> maps = new Dictionary<Vector3Int, RenderTexture>();
    public float offset;
    public Vector3 baseOffset;
    public int chunkSize;
    public Vector3Int bounds;
    [HideInInspector]
    public MapGenerator mapGenerator;

    public GameObject player;
    List<Chunk> chunks = new List<Chunk>();
    Queue<Chunk> chunkPool = new Queue<Chunk>();
    
    // Start is called before the first frame update
    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();
        //chunks = new Chunk[size.x, size.y, size.z];
        
        for(int x = -bounds.x; x < bounds.x; x++)
        {
            for( int y = -bounds.y; y < bounds.y; y++)
            {
                for(int z = -bounds.z; z < bounds.z; z++)
                {
                    GameObject chunkGo = Instantiate(chunkPrefab, transform);
                    chunkGo.layer = gameObject.layer;
                    
                    Chunk chunk = chunkGo.GetComponent<Chunk>();
                    chunk.chunkManager = this;
                    PlaceChunk(chunk, new Vector3Int(x,y,z));
                    
                }
            }
        }
        
    }

    void PlaceChunk(Chunk chunk, Vector3Int index)
    {
        chunk.transform.position = (Vector3)index * offset + baseOffset;
        
        chunk.chunkIndex = index;
        chunk.map = GetMap(chunk.map, chunk.chunkIndex);
        //if (maps.ContainsKey(chunk.chunkIndex))
        //{
        //    if(maps[chunk.chunkIndex] == null)
        //    {
        //        chunk.map = GetMap((Vector3)index);
        //        maps[chunk.chunkIndex] = chunk.map;
        //    }
        //    chunk.map = maps[chunk.chunkIndex];

        //}
        //else
        //{
        //    chunk.map = GetMap((Vector3)index);
        //    maps.Add(chunk.chunkIndex, chunk.map);
        //}
        if(chunk.map != null)
		{
            chunks.Add(chunk);
            existingChunks.Add(chunk.chunkIndex, chunk);
            chunk.SetMapAndPrune(chunkSize);
        }
		else
		{
            chunkPool.Enqueue(chunk);
		}
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
		if (!staticMap)
		{
            Vector3Int playerChunkCoord = new Vector3Int(
            Mathf.FloorToInt(player.transform.position.x / offset),
            Mathf.FloorToInt(player.transform.position.y / offset),
            Mathf.FloorToInt(player.transform.position.z / offset));
            int maxX = playerChunkCoord.x + bounds.x;
            int minX = playerChunkCoord.x - bounds.x;
            int minY = playerChunkCoord.y - bounds.y;
            int maxY = playerChunkCoord.y + bounds.y;
            int maxZ = playerChunkCoord.z + bounds.z;
            int minZ = playerChunkCoord.z - bounds.z;
            //unload all out of bounds chunks
            for (int i = chunks.Count - 1; i >= 0; i--)
            {
                Chunk chunk = chunks[i];
                if (
                    chunk.chunkIndex.x < minX ||
                    chunk.chunkIndex.x > maxX ||
                    chunk.chunkIndex.y < minY ||
                    chunk.chunkIndex.y > maxY ||
                    chunk.chunkIndex.z < minZ ||
                    chunk.chunkIndex.z > maxZ)
                {
                    UnloadChunk(chunk);
                    chunks.Remove(chunk);
                }
            }

            for (int x = -bounds.x; x < bounds.x; x++)
            {
                for (int y = -bounds.y; y < bounds.y; y++)
                {
                    for (int z = -bounds.z; z < bounds.z; z++)
                    {
                        Vector3Int chunkCoord = new Vector3Int(x, y, z) + playerChunkCoord;
                        if (existingChunks.ContainsKey(chunkCoord))
                        {
                            continue;
                        }
                        if (chunkPool.Count > 0)
                            PlaceChunk(chunkPool.Dequeue(), chunkCoord);
                    }
                }
            }
        }
        
    }

    void UnloadChunk(Chunk chunk)
    {
        
        chunkPool.Enqueue(chunk);
        existingChunks.Remove(chunk.chunkIndex);
        //chunk.gameObject.SetActive(false);
    }

    public RenderTexture GetMap(RenderTexture map, Vector3 pos)
    {
        return mapGenerator.GetMap(map, pos * offset + baseOffset);
    }
}
