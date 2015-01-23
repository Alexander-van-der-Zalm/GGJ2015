using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
//public struct AnimatorParameter
//{
//    public enum APType
//    {
//        Bool,
//        Trigger,
//        Int,
//        Float
//    }

//    public APType Type;
//    public string ParameterName;
//}

[System.Serializable]
public class AnimatorCollectionWrapper 
{
    [SerializeField]
    protected List<Animator> animators;

    public AnimatorCollectionWrapper(GameObject rootGO)
    {
        animators = rootGO.GetComponentsInChildren<Animator>().ToList();
    }

    #region Get Set

    #region Float

    public float GetFloat(string param)
    {
        return animators.First().GetFloat(param);
    }

    public void SetFloat(string param, float value)
    {
        for (int i = 0; i < animators.Count; i++)
        {
            animators[i].SetFloat(param, value);
        }
            
    }

    #endregion

    #region Bool

    public bool GetBool(string param)
    {
        return animators.First().GetBool(param);
    }

    public void SetBool(string param, bool value)
    {
        for (int i = 0; i < animators.Count; i++)
            animators[i].SetBool(param, value);
    }

    #endregion

    #region Trigger

    public void SetTrigger(string param)
    {
        for (int i = 0; i < animators.Count; i++)
            animators[i].SetTrigger(param);
    }

    #endregion

    #region Int

    public int GetInt(string param)
    {
        return animators.First().GetInteger(param);
    }

    public void SetInt(string param, int value)
    {
        for (int i = 0; i < animators.Count; i++)
            animators[i].SetInteger(param, value);
    }

    #endregion

    #endregion
}
