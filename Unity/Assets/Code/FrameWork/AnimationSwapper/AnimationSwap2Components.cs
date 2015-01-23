using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AS_AnimList
{
    [SerializeField]
    public List<AnimationClip> Animations;

    public AS_AnimList()
    {
        Animations = new List<AnimationClip>();
        Animations.Add(null);
    }

    public void OnGui(string label = "")
    {
        label += "[";
        List<int> indicesToDelete = new List<int>();
        bool add = false;
        for (int i = 0; i < Animations.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            Animations[i] = (AnimationClip)EditorGUILayout.ObjectField(label + i + "]", Animations[i], typeof(AnimationClip), true);
            
            if(i!=0 && GUILayout.Button("-", GUILayout.Width(30)))
            {
                indicesToDelete.Add(i);
            }

            if (i == 0 && GUILayout.Button("+", GUILayout.Width(30)))
            {
                add = true;
            }
            EditorGUILayout.EndHorizontal();
        }

        for (int i = indicesToDelete.Count-1; i >= 0; i--)
        {
            Animations.RemoveAt(indicesToDelete[i]);
        }
            

        if (add)
            Animations.Add(null);
            
    }
}

[System.Serializable]
public class AS_AnimListCollection
{
    [SerializeField]
    public Sprite PreviewSprite;

    [SerializeField]
    public List<AS_AnimList> Animations;

    public AS_AnimListCollection()
    {
        Animations = new List<AS_AnimList>();
    }
}

[System.Serializable]
public class AS_AnimIndex
{
    [SerializeField]
    public AnimationClip Animation;

    [SerializeField]
    public int Index;

    public void OnGui(string label = "", float labelWidth = 100.0f)
    {
        EditorGUILayout.BeginHorizontal();
        if(!label.Equals(string.Empty))
        {
            //using (new FixedWidthLabel(label))
            //;
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth + 5 +// actual label width
                10 * EditorGUI.indentLevel));
                Animation = (AnimationClip)EditorGUILayout.ObjectField(Animation, typeof(AnimationClip), true);
        }
        else
            Animation = (AnimationClip)EditorGUILayout.ObjectField(Animation, typeof(AnimationClip), true);
        
        using (new FixedWidthLabel("Index:"))
            Index = EditorGUILayout.IntField(Index, GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();
    }
}

[System.Serializable]
public class AS_AnimIndexCollection
{
    [SerializeField]
    public Sprite PreviewSprite;

    [SerializeField]
    public List<AS_AnimIndex> Animations;
    

    public AS_AnimIndexCollection()
    {
        Animations = new List<AS_AnimIndex>();
    }
}
