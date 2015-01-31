using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPalette : EasyScriptableObject<ColorPalette>
{
    #region Class

    [System.Serializable]
    public class TeamColor
    {
        public List<Color> NormalBlockColor;
        public Color UnitSpawnBlockColor;
        public Color StartSpawnBlockSpawnColor;

        public TeamColor(Color startColor, int maxTones)
        {
            NormalBlockColor = new List<Color>();
            for (int i = 0; i < maxTones; i++)
                NormalBlockColor.Add(startColor);

            UnitSpawnBlockColor = startColor;
            StartSpawnBlockSpawnColor = startColor;
        }
    }

    #endregion

    #region Fields

    [Tooltip("The maximimum amount of color tones per Block Type per Team. Each Team needs this amount of colors in order to make the tone map work.")]
    public int MaxTones = 3;

    public List<TeamColor> TeamPalettes;

    #endregion

    #region Editor Functions

    public void Init()
    {
        TeamPalettes = new List<TeamColor>();

        AddTeamPalette(Color.green);
        AddTeamPalette(Color.blue);
        AddTeamPalette(Color.red);
    }

    public void AddTeamPalette(Color color)
    {
        TeamPalettes.Add(new TeamColor(color, MaxTones));
    }

    public void ResizeMaxTones()
    {
        for(int i = 0; i < TeamPalettes.Count; i++)
        {
            int currentTones = TeamPalettes[i].NormalBlockColor.Count;

            // Break out if already at correct size
            if (currentTones == MaxTones)
                continue;

            // Remove if there is to many tones
            if (currentTones > MaxTones)
                TeamPalettes[i].NormalBlockColor.RemoveRange(MaxTones - 1, currentTones - MaxTones);
            else
            {
                // Add if there is not enough tones (magenta if no color)
                Color colorToAdd = currentTones == 0 ? Color.magenta :TeamPalettes[i].NormalBlockColor[0];
                while (currentTones <= MaxTones)
                {
                    TeamPalettes[i].NormalBlockColor.Add(colorToAdd);
                    currentTones++;
                }
                   
            }
        }
    }

    #endregion

    #region GetColor

    public Color GetPaletteColor(Block block, int teamID)
    {
        switch (block.Type)
        {
            case BlockData.BlockType.Normal:
                return TeamPalettes[teamID].NormalBlockColor[block.ColorID];
            case BlockData.BlockType.StartSpawn:
                return TeamPalettes[teamID].StartSpawnBlockSpawnColor;
            case BlockData.BlockType.UnitSpawn:
                return TeamPalettes[teamID].UnitSpawnBlockColor;;
        }
        Debug.LogError("Error GetPaletteColor");
        return Color.cyan;
    }

    public Color GetPaletteColor(BlockFace face, int teamID)
    {
        return GetPaletteColor(face.Block, teamID);
    }

    #endregion
}
