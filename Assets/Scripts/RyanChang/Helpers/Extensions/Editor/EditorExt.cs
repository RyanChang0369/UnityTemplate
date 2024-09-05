using System.Collections;
using IOPath = System.IO.Path;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;

/// <summary>
/// Contains classes pertaining to editor stuff.
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public static class EditorExt
{
    #region Numbers
    /// <summary>
    /// Equal to <see cref="EditorGUIUtility.singleLineHeight"/> +
    /// <see cref="EditorGUIUtility.standardVerticalSpacing"/>.
    /// </summary>
    public static float SpacedLineHeight => EditorGUIUtility.singleLineHeight
        + EditorGUIUtility.standardVerticalSpacing;
    #endregion

    #region Reflection
    /// <summary>
    /// Gets the proper object using reflection.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="propertyPath">The custom property path to search for the
    /// object.</param>
    /// <exception cref="NullReferenceException">
    /// If one or more of the properties (except for the last, target property)
    /// in the path is null.
    /// </exception>
    public static object GetObjectFromReflection(
        this SerializedProperty property, string propertyPath)
    {
        // Get the target object and type. These will be modified when we are
        // traversing the path.
        object targetObject = property.serializedObject.targetObject;
        Type targetType = targetObject.GetType();

        string fullPath = propertyPath;
        fullPath = fullPath.Replace("Array.data[", "[");

        string[] path = fullPath.Split('.');

        // Path traversal.
        foreach (var name in path)
        {
            if (targetObject == null)
                throw new NullReferenceException(
                    $"Trying to obtain field {name} from null reference. " +
                    $"Full path is {fullPath}. Target type is {targetType}."
                );

            // Check if there's a bracket. If so, that means that we have an
            // array.
            int lBracket = name.IndexOf('[');
            int rBracket = name.IndexOf(']');
            if (lBracket >= 0 && rBracket >= 0)
            {
                // In an array
                // This is an array element we are looking for.
                int index = int.Parse(name[lBracket..rBracket]);

                // Iterate through the enumerable, until index is reached.
                foreach (object thing in (IEnumerable)targetObject)
                {
                    if (index <= 0)
                    {
                        targetObject = thing;
                        targetType = thing.GetType();
                        break;
                    }

                    index--;
                }
            }
            else if (lBracket.Sign() != rBracket.Sign())
            {

            }
            else
            {
                // Go through all inherited classes as well.
                // For each name in the path, get the reflection.
                FieldInfo field = targetType.GetField(
                    name,
                    BindingFlags.NonPublic | BindingFlags.Public |
                    BindingFlags.Instance | BindingFlags.FlattenHierarchy
                ) ?? throw new NullReferenceException(
                    $"Field is null for {name}"
                );

                targetType = field.FieldType;
                targetObject = field.GetValue(targetObject);
            }
        }

        return targetObject;
    }

    /// <inheritdoc cref="GetObjectFromReflection(SerializedProperty)"/>
    public static object GetObjectFromReflection(
        this SerializedProperty property) =>
        GetObjectFromReflection(property, property.propertyPath);

    /// <param name="obj">The object to assign the value to.</param>
    /// <inheritdoc cref="GetObjectFromReflection(SerializedProperty)"/>
    public static void GetObjectFromReflection<T>(
        this SerializedProperty property, out T obj) =>
        obj = (T)property.GetObjectFromReflection();

    /// <param name="obj">The object to assign the value to.</param>
    /// <inheritdoc cref="GetObjectFromReflection(SerializedProperty)"/>
    public static void GetObjectFromReflection<T>(
        this SerializedProperty property, string propertyPath, out T obj) =>
        obj = (T)property.GetObjectFromReflection(propertyPath);

    /// <summary>
    /// Calls <see cref="SerializedProperty.Next(bool)"/> until a child is found
    /// of type <paramref name="type"/>. This is useful if your property is
    /// nested somewhere.
    /// </summary>
    /// <param name="property">The serialized property to search. If it returns
    /// true, then this will be a serialized property of type <paramref
    /// name="type"/>. Otherwise, it is reset.</param>
    /// <param name="type">The type to look for.</param>
    /// <param name="visibleOnly">If true, then calls <see
    /// cref="SerializedProperty.NextVisible(bool)"/> instead.</param>
    /// <return>True on success, false otherwise.</return>
    public static bool SeekToType(this SerializedProperty property,
        Type type, bool visibleOnly = false)
    {
        do
        {
            if (property.type == type.Name)
                return true;
        }
        while (visibleOnly ? property.NextVisible(true) : property.Next(true));

        property.Reset();
        return false;
    }

    /// <inheritdoc cref="SeekToType(SerializedProperty, Type, bool)"/>
    /// <typeparam name="T">The type.</typeparam>
    public static bool SeekToType<T>(this SerializedProperty property,
        bool visibleOnly = false) =>
        SeekToType(property, typeof(T), visibleOnly);

    /// <summary>
    /// Gets a member by its name from some <paramref name="type"/>, using
    /// whatever means nessisary.
    /// </summary>
    /// <param name="memberName"></param>
    /// <returns>True if some member was found, false otherwise.</returns>
    public static bool ForceGetMember(this Type type, string memberName,
        out object memberData)
    {
        // Part 1: Look for public instance members.
        MemberInfo[] publicMembers = type.GetMember(
            memberName,
            BindingFlags.Public | BindingFlags.Instance
        );

        if (publicMembers.NotEmpty())
        {
            memberData = publicMembers[0];
        }

        MemberInfo[] privateMembers = type.GetMember(
            memberName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (privateMembers.NotEmpty())
        {
            memberData = privateMembers[0];
        }

        throw new NotImplementedException();
    }
    #endregion

    #region Drawing
    /// <summary>
    /// Draws a bolded title at position.
    /// </summary>
    /// <param name="position">Where to draw this.</param>
    /// <param name="label">Label to use as a title.</param>
    public static void TitleLabelField(ref Rect position, GUIContent label)
    {
        position.height = 24;
        EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
        position.TranslateY(position.height);
    }

    /// <summary>
    /// Draws a label at position.
    /// </summary>
    /// <inheritdoc cref="TitleLabelField(ref Rect, GUIContent)"/>
    public static void LabelField(ref Rect position, GUIContent label)
    {
        position.height = 24;
        EditorGUI.LabelField(position, label);
        position.TranslateY(position.height);
    }

    /// <summary>
    /// Draws the serialized property. Tries to look for custom property
    /// drawers.
    /// </summary>
    /// <param name="property">Property to draw.</param>
    /// <param name="position">Where to draw the property.</param>
    /// <param name="label">Label to use with custom property drawers, if found
    /// </param>
    public static void PropertyField(this SerializedProperty property,
        ref Rect position, GUIContent label)
    {
        // Try to find property drawer, if we can.
        PropertyDrawer drawer = PropertyDrawerFinder.FindDrawer(property);

        if (drawer != null)
        {
            // Found a custom property drawer. Use found drawer.
            position.height = drawer.GetPropertyHeight(property, label);
            EditorGUI.BeginProperty(position, label, property);
            drawer.OnGUI(position, property, label);
            EditorGUI.EndProperty();
        }
        else
        {
            // Did not find a custom property drawer. Use property field.
            position.height = EditorGUI.GetPropertyHeight(property);
            EditorGUI.PropertyField(position, property, true);
        }

        position.TranslateY(position.height);
    }

    /// <summary>
    /// Draws the serialized property. Tries to look for custom property
    /// drawers.
    /// </summary>
    /// <param name="property">Property to draw.</param>
    /// <param name="position">Where to draw the property.</param>
    public static void PropertyField(this SerializedProperty property,
        ref Rect position)
    {
        property.PropertyField(ref position, GUIContent.none);
    }

    /// <summary>
    /// Returns the height of the serialized property, trying to look for any
    /// custom height getters. If not found, returns defaultHeight.
    /// </summary>
    /// <param name="property">Property to get the height from.</param>
    /// <param name="label">The label.</param>
    /// <param name="defaultHeight">The default height
    /// (base.GetPropertyHeight(property, label);).</param>
    /// <returns></returns>
    public static float GetSerializedPropertyHeight(
        this SerializedProperty property, GUIContent label, float defaultHeight)
    {
        // Try to find property drawer, if we can.
        PropertyDrawer drawer = PropertyDrawerFinder.FindDrawer(property);

        if (drawer != null)
        {
            // Found a custom property drawer. Use found drawer.
            return drawer.GetPropertyHeight(property, label);
        }
        else
        {
            // Did not find a custom property drawer. Use default.
            return defaultHeight;
        }
    }

    /// <summary>
    /// Alias for <see cref="EditorGUI.GetPropertyHeight(SerializedProperty)"/>
    /// </summary>
    public static float GetPropertyHeight(this SerializedProperty property,
        GUIContent label = null, bool includeChildren = true) =>
        EditorGUI.GetPropertyHeight(property, label, includeChildren);

    /// <summary>
    /// Retrieves all immediate children of <paramref name="property"/>.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public static IEnumerable<SerializedProperty> GetImmediateChildren(
        this SerializedProperty property, bool visibleOnly = true)
    {
        if (property.hasVisibleChildren)
        {
            var end = property.GetEndProperty();
            Next(property, visibleOnly, true);

            while (!SerializedProperty.EqualContents(property, end))
            {
                yield return property;

                Next(property, visibleOnly, false);
            }
        }
    }

    public static void Next(this SerializedProperty property,
        bool visibleOnly, bool enterChildren)
    {
        if (visibleOnly)
            property.NextVisible(enterChildren);
        else
            property.Next(enterChildren);
    }
    #endregion

    #region IO
    /// <summary>
    /// Creates and saves a scriptable object to the disk.
    /// </summary>
    /// <typeparam name="T">Type of scriptable object to create.</typeparam>
    /// <param name="path">Path to save the file to.</param>
    /// <returns>The created scriptable object.</returns>
    public static T CreateAndSaveScriptableObject<T>(
        string path = "Assets/ScriptableObjects") where T : ScriptableObject
    {
        var so = ScriptableObject.CreateInstance<T>();

        Directory.CreateDirectory(path);

        string fn = IOPath.ChangeExtension(typeof(T).ToString(), ".asset");
        AssetDatabase.CreateAsset(so, IOPath.Combine(path, fn));
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = so;
        return so;
    }
    #endregion
}
