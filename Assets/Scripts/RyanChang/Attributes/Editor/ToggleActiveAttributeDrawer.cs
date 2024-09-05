using UnityEngine;
using UnityEditor;

/// <summary>
/// Drawer for <see cref="ToggleActiveAttribute"/>.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[CustomPropertyDrawer(typeof(ToggleActiveAttribute), true)]
public class ToggleActiveAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, 
        GUIContent label)
    {
        property.GetObjectFromReflection(out IRNGModel model);
        Debug.Log(model);
    }
}