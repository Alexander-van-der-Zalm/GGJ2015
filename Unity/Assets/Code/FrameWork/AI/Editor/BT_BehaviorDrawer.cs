using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System.Linq;

//[CustomPropertyDrawer(typeof(BT_Behavior))]
public class BT_BehaviorDrawer : PropertyDrawer
{
    void OnGui(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var c = from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && t.IsSubclassOf(typeof(BT_Action))
                select t.Name.ToString();

        DebugHelper.LogList<string>(c.ToList());

        //string[] classes= 

        //EditorGUI.Popup();

        EditorGUI.EndProperty();
    }

}
