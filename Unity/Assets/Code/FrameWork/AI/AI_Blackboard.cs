using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AI_Blackboard : EasyScriptableObject<AI_Blackboard>
{
    #region Fields

    public string Name = "AI BlackBoard";

    // Serializable dictionaries
    [SerializeField]
    private UDictionaryStringSerializableObject objectPool;
    [SerializeField]
    private UDictionaryStringBool isVariableObject;

    #endregion

    #region Properties

    public UDictionaryStringSerializableObject ObjectPool 
    { 
        get { return objectPool == null? objectPool = new UDictionaryStringSerializableObject() : objectPool; } 
        private set { objectPool = value; } 
    }


    public UDictionaryStringBool IsVariableObject 
    {
        get { return isVariableObject == null ? isVariableObject = new UDictionaryStringBool() : isVariableObject; } 
        private set { isVariableObject = value; } 
    }

    #endregion

    #region Constructor

    public void Clear()
    {
        ObjectPool.Clear();
        IsVariableObject.Clear();
    }

    public override void Init(HideFlags newHideFlag = HideFlags.None)
    {
        base.Init(newHideFlag);
    } 

    #endregion

    #region Property

    // Default getObject (runs in trouble when it doesnt exist)
    public object this[string name]
    {
        get { return GetObject(name); }
        set { SetObject(name, value); }
    }

    #endregion

    #region Get

    public object GetObject(string name)
    {      
        if (!ObjectPool.ContainsKey(name))
            return DoesNotContainKey<object>(name);

        return ObjectPool[name].Object;
    }

    public object GetObjectOrSetDefault(string name, object newDefault)
    {       
        if (ObjectPool.ContainsKey(name))
            return ObjectPool[name].Object;

        SetObject(name,newDefault);
        return newDefault;
    }

    #endregion

    #region Helpers

    private T DoesNotContainKey<T>(string name)
    {
        Debug.Log("AI_Blackboard.GetObject(" + name + ") does not exist in dictionary. Creating and saving default.");
        return default(T);
    }

    #endregion

    #region Set

    /// <summary>
    /// Null makes it a variable type in the editorInspector
    /// </summary>
    public void SetObject(string name, object obj)
    {
        // Null makes it a variable type in the editorInspector
        if (!IsVariableObject.ContainsKey(name))
        {
            if (obj == null)
                IsVariableObject[name] = true;
            else
                IsVariableObject[name] = false;
        }

        // Set the object
        ObjectPool[name] = new SerializableObject() { Object = obj };
    }

    #endregion

    public void ChangeValues(AI_Blackboard bb)
    {
        //this = AI_Blackboard.Create();
        ObjectPool = bb.ObjectPool;
        IsVariableObject = bb.IsVariableObject;
    }

    public static AI_Blackboard CreateShallowCopy(AI_Blackboard bb)
    {
        AI_Blackboard newBB = AI_Blackboard.Create();
        newBB.ObjectPool.ShallowCopyIn(bb.ObjectPool);
        newBB.IsVariableObject.ShallowCopyIn(bb.IsVariableObject);
        newBB.Name = bb.Name;
        return newBB;
    }

    //internal static AI_Blackboard CreateBlackboard()
    //{
    //    throw new System.NotImplementedException();
    //}
}
