using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapGeneratorPass : ScriptableObject
{
   public ComputeShader shader;
    public virtual void Run(RenderTexture map, int size, Vector3 offset)
    {

    }
}
