using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [SerializeField]ComputeShader shader;
    public ChunkManager chunkManager;
    public float Distance;
    public LayerMask map;
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
                Collider[] colliders = Physics.OverlapSphere(hit.point,Distance,map);
                foreach (Collider collider in colliders)
				{
                    chunks.Add(collider.GetComponent<Chunk>());
				}
                
                foreach(Chunk chunk in chunks)
                {
                    mapBuffer.SetData(chunk.map);
                    shader.SetBuffer(0, "Map", mapBuffer);
                    shader.SetVector("pos", hit.point - chunk.transform.position);
                    shader.SetFloat("dist",Distance);
                    shader.SetInt("scale", size);
                    shader.Dispatch(0, Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8));
                    chunk.SetMap(mapBuffer, size);
                }
                
                
            }
        }
    }
}
