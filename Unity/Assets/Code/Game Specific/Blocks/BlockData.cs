using UnityEngine;
using System.Collections;

[System.Serializable]
public class BlockData 
{
    public enum BlockType
    {
        Normal,
        Unit
    }
    
    public Vector3 Scale,Position;
    public Quaternion Rot;
    public BlockType Type;
}
