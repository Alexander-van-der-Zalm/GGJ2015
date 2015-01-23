using UnityEngine;
using System.Collections;

[System.Serializable] public class UDictionaryStringFloat : UDictionary<string,float>{}

[System.Serializable] public class UDictionaryStringSerializableObject : UDictionary<string, SerializableObject> { }

[System.Serializable] public class UDictionaryStringBool : UDictionary<string, bool> { }

[System.Serializable] public class UDictionaryIntBT_Status : UDictionary<int, BT_Behavior.Status> { }

[System.Serializable] public class UDictionaryIntBT_TreeNode : UDictionary<int, BT_TreeNode> { }