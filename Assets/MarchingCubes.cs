using UnityEngine;
using UnityEngine.Rendering;

public class MarchingCubes : MonoBehaviour
{
    public float cutoff = 0;
    public ComputeShader Shader;
    Chunk chunk;
    Mesh mesh;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    


    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = new Mesh();
        chunk = GetComponent<Chunk>();
        mesh.MarkDynamic();
    }

    struct Triangle 
    {
        Vector3 a;
        Vector3 b;
        Vector3 c;

        public Vector3 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    default:
                        return c;
                }
            }
        }
    }

    /// <summary>
    /// Generates a mesh from a map generated elsewhere
    /// </summary>
    /// <param name="map">3d render texture of halfs</param>
    /// <param name="size">size of the map</param>
    /// <returns>if the procedure produces a mesh</returns>
    public bool Run(RenderTexture map, int size)
    {
        

        //set stuff
        Shader.SetTexture(0,"points",map);
        Shader.SetInt("scale", size);
        Shader.SetFloat("_cutoff", cutoff);
        int maxTriCount = (size - 1) * (size - 1) * (size - 1);
        ComputeBuffer triBuffer = new ComputeBuffer(maxTriCount, 36,ComputeBufferType.Append);
        ComputeBuffer triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        triBuffer.SetCounterValue(0);
        Shader.SetBuffer(0, "Tris", triBuffer);
        Shader.Dispatch(0, Mathf.CeilToInt((float)(size+1) / 8), Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8));


        // Get number of triangles in the triangle buffer
        ComputeBuffer.CopyCount(triBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];

        if(numTris > 0)
        {
            Triangle[] tris = new Triangle[numTris];
            triBuffer.GetData(tris, 0, 0, numTris);

            Vector3[] vertices = new Vector3[numTris * 3];
            int[] triangles = new int[numTris * 3];

            for (int i = 0; i < numTris; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    triangles[i * 3 + j] = i * 3 + j;
                    vertices[i * 3 + j] = tris[i][j];
                }
            }


            mesh.Clear();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            chunk.state = ChunkState.meshUpdated;

            triBuffer.Dispose();
            triCountBuffer.Dispose();
            
            return true;
        }
        meshFilter.mesh = null;
        meshCollider.sharedMesh = null;
        triBuffer.Dispose();
        triCountBuffer.Dispose();
        chunk.state = ChunkState.physicsUpdated;
        return false;
    }
    public void PhysicsUpdate()
    {

        meshCollider.cookingOptions = 0;
        meshCollider.sharedMesh = mesh;
        chunk.state = ChunkState.physicsUpdated;
    }
    public void Unload()
    {
        meshFilter.mesh = null;
    }

}
