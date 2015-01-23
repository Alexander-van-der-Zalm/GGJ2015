using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BasicUnit : MonoBehaviour
{
    private BlockFace CurrentFace;
    private Transform tr;

    public void Start()
    {
        tr = transform;
    }

    public void MoveUnit(int blockID,int blockFaceID)
    {
        // Update face state both faces
        // Change face
        CurrentFace.HasUnit = false;
        Block bl = BlockManager.Get(blockID);
        CurrentFace = BlockManager.Get(blockID).GetFace(blockFaceID);
        CurrentFace.HasUnit = true;

         // Translate (TELEPORT HACK)
        // Change to destination and walk
        tr.position = CurrentFace.transform.position;
        // Rotate
        tr.rotation = CurrentFace.Rotation;
    }
}
