using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(FixedList<>), true)]
public class FixedListDrawerUIE : PropertyDrawer
{
    private bool unfolded = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        unfolded = EditorGUI.BeginFoldoutHeaderGroup(position, unfolded, label.text);

        if (unfolded)
        {
            // Enter child, which is internalList.
            property.Next(true);
            int arrLen = property.arraySize;

            // Collapsable section

            position.xMin += 16;    // Some padding
            position.Translate(new(0, 24));

            for (int i = 0; i < arrLen; i++)
            {
                // Draw each of the elements in internalList.
                var elem = property.GetArrayElementAtIndex(i);
                elem.DrawSerializedProperty(ref position, new($"Element {i}"));
            }
        }

        EditorGUI.EndFoldoutHeaderGroup();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (unfolded)
        {
            float height = base.GetPropertyHeight(property, label);

            // Enter child
            property.Next(true);
            int arrLen = property.arraySize;

            for (int i = 0; i < arrLen; i++)
            {
                var elem = property.GetArrayElementAtIndex(i);
                height += EditorGUI.GetPropertyHeight(elem);
            }

            return height + 8;
        }

        return 18;
    }
}