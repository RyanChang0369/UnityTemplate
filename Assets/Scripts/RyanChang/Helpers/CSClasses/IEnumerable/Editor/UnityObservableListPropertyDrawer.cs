using UnityEngine;
using UnityEditor;
using System.Collections.Specialized;

[CustomPropertyDrawer(typeof(UnityObservableList<>))]
public class UnityObservableListPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property,
        GUIContent label)
    {
        // Get the internal list.
        property = property.FindPropertyRelative("internalList");

        // Check if changes have been made to the list.
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property, label);

        if (EditorGUI.EndChangeCheck())
        {
            // Change detected.
            property.GetObjectFromReflection(
                out INotifyCollectionChanged notifier
            );
        }
    }
}