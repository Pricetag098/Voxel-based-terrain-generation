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
    public float MaxEditRange = 100;
    public float MinEditRange = 1.5f;
    
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
            EditMap(power,MinEditRange);
        }
        if (Input.GetMouseButton(1))
        {
            EditMap(-power,0);
        }
    }

    void EditMap(float power,float minDist)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + transform.forward * minDist, transform.forward, out hit, MaxEditRange, map))
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
                
                RenderTexture map = chunk.map;
                if (map == null)
                {
                    map = chunkManager.GetMap(chunk.chunkIndex);
                    chunk.map = map;
                    
                }
                shader.SetTexture(0, "Map", map);
                shader.SetVector("pos", hit.point - chunk.transform.position);
                shader.SetFloat("dist", Distance);
                shader.SetFloat("power", power * Time.deltaTime);
                shader.SetInt("scale", size);
                shader.Dispatch(0, Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8));
                chunk.UpdateMap(size);
            }


        }
    }
}
