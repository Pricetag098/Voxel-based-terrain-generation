using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [SerializeField]ComputeShader shader;
    public ChunkManager chunkManager;
    public float Distance;
    public float power = 1;
    public LayerMask map,mapEditCollider;
    
    int size;
    // Start is called before the first frame update
    void Start()
    {
        size = chunkManager.GetComponent<MapGenerator>().size;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            
            if(Physics.Raycast(transform.position, transform.forward, out hit,float.PositiveInfinity,map))
            {
                Chunk hitChunk = hit.collider.gameObject.GetComponent<Chunk>();
                
                List<Chunk> chunks = new List<Chunk>();
                Collider[] colliders = Physics.OverlapSphere(hit.point,Distance,mapEditCollider);
                foreach (Collider collider in colliders)
				{
                    
                    chunks.Add(collider.GetComponentInParent<Chunk>());
				}
                
                foreach(Chunk chunk in chunks)
                {
                    
                    shader.SetTexture(0, "Map", chunk.map);
                    shader.SetVector("pos", hit.point - chunk.transform.position);
                    shader.SetFloat("dist",Distance);
                    shader.SetFloat("power", power);
                    shader.SetInt("scale", size);
                    shader.Dispatch(0, Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8));
                    chunk.UpdateMap(size);
                }
                
                
            }
        }
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, float.PositiveInfinity, map))
            {
                Chunk hitChunk = hit.collider.gameObject.GetComponent<Chunk>();

                List<Chunk> chunks = new List<Chunk>();
                Collider[] colliders = Physics.OverlapSphere(hit.point, Distance, mapEditCollider);
                foreach (Collider collider in colliders)
                {

                    chunks.Add(collider.GetComponentInParent<Chunk>());
                }

                foreach (Chunk chunk in chunks)
                {
                    shader.SetTexture(0, "Map", chunk.map);
                    shader.SetVector("pos", hit.point - chunk.transform.position);
                    shader.SetFloat("dist", Distance);
                    shader.SetFloat("power", -power);
                    shader.SetInt("scale", size);
                    shader.Dispatch(0, Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8));
                    chunk.UpdateMap(size);
                }


            }
        }
    }

    
}
