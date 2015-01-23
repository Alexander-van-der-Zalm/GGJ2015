using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AnimationSwap2Selector))]
public class AnimationSwap2SelectorEditor : EditorPlus
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AnimationSwap2Selector sel = (AnimationSwap2Selector)target;

        if (GUILayout.Button("SetAnimators"))
            sel.CollectionReference.SetAnimator(sel.Head, sel.Body, sel.HeadVariety, sel.BodyVariety);
    }
}

[CustomEditor(typeof(AnimationSwap2Collection))]
public class AnimSwap2CollectionEditor : EditorPlus 
{
    public override void OnInspectorGUI()
    {
        if(SavedFoldoutShared("OriginalGUI"))
        {
            EditorGUI.indentLevel++;
            base.OnInspectorGUI();
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }

        #region Prep

        AnimationSwap2Collection col = (AnimationSwap2Collection)target;
        //AnimatorOverrideController over = col.GetNewOverrideController();

        

        // Calculate maxWidth
        float maxWidth = GUI.skin.label.CalcSize(new GUIContent("PreviewSprite")).x;
        for (int i = 0; i < col.ClipNames.Count; i++)
        {
            //Debug.Log(col.ClipNames[i]);
            float curWidth = GUI.skin.label.CalcSize(new GUIContent(col.ClipNames[i])).x;
            if (curWidth > maxWidth)
                maxWidth = curWidth;
        }

        #endregion

        // Controller selector
        EditorGUILayout.BeginHorizontal();
        col.Controller = (RuntimeAnimatorController)EditorGUILayout.ObjectField("Controller",col.Controller, typeof(RuntimeAnimatorController),true);
        
        if(GUILayout.Button("ResetClipNames"))
            col.ResetClipNames();
        EditorGUILayout.EndHorizontal();

        if (col.ClipNames.Count == 0)
        {
            EditorGUILayout.LabelField("No clips, please reset");
            return;
        }

        #region Add & Save

        // New variety adding
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("NewBodyVariety"))
            col.AddNewBodyVariety();
        if (GUILayout.Button("NewHeadVariety"))
            col.AddNewHeadVariety();
        //if (GUILayout.Button("Save"))
        //    ScriptableObjectHelper.RefreshAsset(col);
        EditorGUILayout.EndHorizontal();

        #endregion

        #region Body

        if (SavedFoldoutShared("Body varieties", -1))
        {

            EditorGUI.indentLevel++;
            for (int i = 0; i < col.BodyVarieties.Count; i++)
            {
                if (SavedFoldoutShared("Body variety[" + i + "]", i))
                {
                    // PreviewSprite
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("PreviewSprite", GUILayout.Width(maxWidth + 5 + 10 * EditorGUI.indentLevel));
                    col.BodyVarieties[i].PreviewSprite = (Sprite)EditorGUILayout.ObjectField(col.BodyVarieties[i].PreviewSprite, typeof(Sprite), true);//, GUILayout.Width(maxWidth));
                    EditorGUILayout.EndHorizontal();

                    for (int j = 0; j < col.BodyVarieties[i].Animations.Count; j++)
                    {
                        col.BodyVarieties[i].Animations[j].OnGui(col.ClipNames[j], maxWidth);
                    }
                }
            }
            EditorGUI.indentLevel--;
        }

        #endregion

        #region Head

        if (SavedFoldoutShared("Head varieties", -1))
        {
            EditorGUI.indentLevel++;

            for (int i = 0; i < col.HeadVarieties.Count; i++)
            {
                if (SavedFoldoutShared("Head variety[" + i + "]", i))
                {
                    // PreviewSprite
                    //EditorGUILayout.LabelField("PreviewSprite", GUILayout.Width(maxWidth));
                    col.HeadVarieties[i].PreviewSprite = (Sprite)EditorGUILayout.ObjectField("PreviewSprite",
                                                            col.HeadVarieties[i].PreviewSprite, typeof(Sprite), true);//, GUILayout.Width(maxWidth));

                    for (int j = 0; j < col.HeadVarieties[i].Animations.Count; j++)
                    {
                        col.HeadVarieties[i].Animations[j].OnGui(col.ClipNames[j]);

                    }
                }
            }
            EditorGUI.indentLevel--;
        }

        #endregion

        if(GUI.changed)
            ScriptableObjectHelper.RefreshAsset(col);
    }
}
