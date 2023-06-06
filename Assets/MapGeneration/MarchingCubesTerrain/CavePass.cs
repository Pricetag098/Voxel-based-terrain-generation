
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "MapPasses/CavePass")]
public class CavePass : MapGeneratorPass
{
    public float freq, amp;
    public float la, per;
    public int octaves = 1;
    public float power;
    public override void Run(RenderTexture map, int size, Vector3 offset)
    {
        shader.SetTexture(0, "Map", map);
        shader.SetFloat("freq", freq);
        shader.SetFloat("amp", amp);
        shader.SetFloat("la", la);
        shader.SetVector("offset", offset);
        shader.SetFloat("per", per);
        shader.SetInt("octaves", octaves);
        shader.SetInt("scale", size);
        shader.SetFloat("power",power);
        shader.Dispatch(0, Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8), Mathf.CeilToInt((float)(size + 1) / 8));
    }
}
