using UnityEngine;
using System.Collections;

public class LevelBuildingCommands
{
    public static void AddBlock(BlockFace face)
    {
        Debug.Log("Create new block: [" + face.Block.ID + "," + face.ID + "]");
        BlockManager.Add(face.Block.ID, face.ID);
    }

    public static void ChangeBlockToNormal(BlockFace face)
    {
        //if (face.Block.Type != BlockData.BlockType.Normal)
        //    face.Block.TeamID = 0;
        //else
        //    face.Block.TeamID = (face.Block.TeamID + 1) % GameManagement.Block.Pallet.TeamPalettes.Count;

        face.Block.Type = BlockData.BlockType.Normal;
        // Loop through tones
        ChangeTone(face);
        face.Block.SetCurrentTeamColor();
    }

    public static void ChangeBlockToUnitSpawn(BlockFace face)
    {
        if (face.Block.Type != BlockData.BlockType.UnitSpawn)
            face.Block.TeamID = 0;
        else
            face.Block.TeamID = (face.Block.TeamID + 1) % GameManagement.Block.Pallet.TeamPalettes.Count;
        
        face.Block.Type = BlockData.BlockType.UnitSpawn;
       
        face.Block.SetCurrentTeamColor();
    }

    public static void ChangeBlockToStartSpawn(BlockFace face)
    {
        face.Block.Type = BlockData.BlockType.StartSpawn;
        face.Block.TeamID = (face.Block.TeamID + 1) % GameManagement.Block.Pallet.TeamPalettes.Count;
        face.Block.SetCurrentTeamColor();
    }

    public static void ChangeSpawnFace(BlockFace face)
    {
        face.Block.SpawnFaceID = face.ID;
    }

    public static void ChangeTone(BlockFace face)
    {
        face.Block.ColorID = (face.Block.ColorID + 1) % GameManagement.Block.Pallet.MaxTones;

        face.Block.SetCurrentTeamColor();
    }
}
