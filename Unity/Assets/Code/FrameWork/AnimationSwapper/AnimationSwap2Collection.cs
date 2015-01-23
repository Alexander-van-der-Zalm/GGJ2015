using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class AnimationSwap2Collection : EasyScriptableObject<AnimationSwap2Collection>
{
    #region Fields

    //[SerializeField]
    //public List<string> AnimationNames;
    //[SerializeField]
    //public RuntimeAnimationController Controller; 

    [SerializeField]
    public RuntimeAnimatorController Controller;

    [SerializeField]
    public List<AS_AnimIndexCollection> BodyVarieties;

    [SerializeField]
    public List<AS_AnimListCollection> HeadVarieties;

    [SerializeField,HideInInspector]
    private List<string> clipNames;

    public List<string> ClipNames 
    { 
        get 
        {
            // Reset if there is no clip
            if(clipNames == null)
            {
                AnimatorOverrideController ctr = GetNewOverrideController();
                clipNames = ctr.clips.Select(k => k.originalClip.name).Distinct().ToList();
                ctr = null;
            }
            return clipNames;// != null ? clipNames : clipNames = GetNewOverrideController().clips.Select(k => k.originalClip.name).Distinct().ToList(); 
        } 
    }

    //TODO
    //[SerializeField,HideInInspector]
    //m_overrideController =

    #endregion

    #region Constructor

    [MenuItem("CustomTools/Create new AnimationSwapCollection")]
    public static void CreateAnimationSwapCollection()
    {
        //string path = AssetDatabase.GenerateUniqueAssetPath("Assets/");
        //EasyScriptableObjectHelper
        //AnimationSwap2Collection.CreateObjAndAsset(path);
        //AnimationSwap2Collection col = AnimationSwap2Collection.Create();
        ScriptableObjectHelper.SaveAssetAutoNaming(AnimationSwap2Collection.Create());
    }

    public override void Init(HideFlags newHideFlag = HideFlags.None)
    {
        base.Init(newHideFlag);
        BodyVarieties = new List<AS_AnimIndexCollection>();
        HeadVarieties = new List<AS_AnimListCollection>();
    }

    #endregion

    public AnimatorOverrideController GetNewOverrideController()
    {
        AnimatorOverrideController controller = new AnimatorOverrideController();
        AnimatorOverrideController old = (AnimatorOverrideController)controller.runtimeAnimatorController;
        old = null;
        controller.runtimeAnimatorController = Controller;
        return controller;
    }

    public void SetAnimator(Animator head, Animator body,  int headIndex,int bodyIndex)
    {
        AnimatorOverrideController headOverride = GetNewOverrideController();
        AnimatorOverrideController bodyOverride = GetNewOverrideController();

        AnimationClipPair[] headAnims = headOverride.clips;
        AnimationClipPair[] bodyAnims = bodyOverride.clips;

        #region Check if indices are legal
        if (headIndex >= HeadVarieties.Count || bodyIndex >= BodyVarieties.Count)
        {
            Debug.LogException(new System.IndexOutOfRangeException("AnimationSwap2Collection.SetAnimator body or head index out of range"));
        }
        #endregion

        // Set preview sprites
        head.GetComponent<SpriteRenderer>().sprite = HeadVarieties[headIndex].PreviewSprite;
        body.GetComponent<SpriteRenderer>().sprite = BodyVarieties[bodyIndex].PreviewSprite;

        //Loop to set all the animations
        for (int i = 0; i < ClipNames.Count; i++)
        {
            //Debug.Log(headOverride.clips[i].originalClip.name);

            // Get the right variety
            AS_AnimIndex bodyVariety = BodyVarieties[bodyIndex].Animations[i];
            AS_AnimList headVariety = HeadVarieties[headIndex].Animations[i];

            #region Check if the bodyVarieties head index is legal
            if (bodyVariety.Index >= headVariety.Animations.Count)
                Debug.LogException(new System.IndexOutOfRangeException("AnimationSwap2Collection.SetAnimator body index is illegal head index"));
            #endregion

            // Set the head based on the bodyIIndex
            if (headVariety.Animations[bodyVariety.Index] != null)
            {
                headAnims[i].overrideClip = headVariety.Animations[bodyVariety.Index];
                //Debug.Log(headAnims[i].overrideClip.name + " is now: " + headVariety.Animations[bodyVariety.Index].name);
            }
            
            // Set the body
            if (bodyVariety.Animation != null)
            {
                bodyAnims[i].overrideClip = bodyVariety.Animation;
                //Debug.Log(bodyAnims[i].overrideClip.name + " is now: " + bodyVariety.Animation.name);
            }
        }


        headOverride.clips = headAnims;
        bodyOverride.clips = bodyAnims;

        // Set all the overrided animations to the animators
        head.runtimeAnimatorController = headOverride;
        body.runtimeAnimatorController = bodyOverride;
    }

    public void AddNewBodyVariety()
    {
        //AnimatorOverrideController controller = GetNewOverrideController();
        
        AS_AnimIndexCollection newBody = new AS_AnimIndexCollection();

        for (int i = 0; i < ClipNames.Count; i++)
        {
            newBody.Animations.Add(new AS_AnimIndex());
        }

        BodyVarieties.Add(newBody);
    }

    public void AddNewHeadVariety()
    {
        //AnimatorOverrideController controller = GetNewOverrideController();
        
        AS_AnimListCollection newHead = new AS_AnimListCollection();

        for (int i = 0; i < ClipNames.Count; i++)
        {
            newHead.Animations.Add(new AS_AnimList());
        }

        HeadVarieties.Add(newHead);
    }

    //public void 

    public void ResetClipNames()
    {
        clipNames = null;
        
        for (int i = 0; i < ClipNames.Count; i++)
        {
            //Debug.Log(ClipNames[i]);
            Debug.Log("TODO need to add extra clips if needed");
        }
    }
}
