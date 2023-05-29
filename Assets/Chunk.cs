using UnityEngine;


public enum ChunkState {
    unloaded,
    placing,
    meshUpdated,
    physicsUpdated
}

public class Chunk : MonoBehaviour
{
    //[HideInInspector]public Vector4[] map;
    MarchingCubes marchingCubes;
    public Vector3Int chunkIndex;
    public RenderTexture map;
    public ChunkManager chunkManager;
    public ChunkState state;
    private void Awake()
    {
        marchingCubes = GetComponent<MarchingCubes>();
        
    }

    private void Start()
    {
        
    }

    public void SetMapAndPrune(int size)
    {
        if (!marchingCubes.Run(map, size))
		{
            chunkManager.mapGenerator.ReturnMap(map);
            map = null;
        }
            
    }
    public void UpdateMap(int size)
    {
        marchingCubes.Run(map, size);
    }

	public void UpdateCollider()
	{
		
	}


	void GetData(ComputeBuffer buffer, int size)
    {
        if (buffer == null)
            return;
        //map = new Vector4[size * size * size];
        //buffer.GetData(map);
        //await Task.CompletedTask;
    }
    public void PhysicsUpdate()
    {
        marchingCubes.PhysicsUpdate();
    }
    
    public void Unload()
    {
        state = ChunkState.unloaded;
        marchingCubes.Unload();
    }

}
