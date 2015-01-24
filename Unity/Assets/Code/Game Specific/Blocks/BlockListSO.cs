using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BlockListSO : EasyScriptableObject<BlockListSO>
{
    public List<BlockData> Blocks;
}
