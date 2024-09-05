using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;
using System.Reflection;
using System.Linq;

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
        string toggleName = ((ToggleActiveAttribute)attribute).ToggleName;
        Type type = property.GetObjectFromReflection(
            property.propertyPath.SpliceWords(0..^1, '.')
        ).GetType();

        MemberInfo[] members = type.GetMember(
            toggleName,
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.Static
        );

        if (members.OneAndOnly(out MemberInfo info))
        {

        }
    }
}