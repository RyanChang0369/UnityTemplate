using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnityDictionary<,>), true)]
public class UnityDictionaryPropertyDrawer : PropertyDrawer
{
    #region Constants
    private readonly float errorBoxHeight = EditorGUIUtility.singleLineHeight * 2;

    private const float LIST_ERROR_SPACING = 5;
    #endregion

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            // First, load the unityDictionary. This enables us to check for
            // errors.
            property.GetObjectFromReflection(out IUnityDictionary unityDict);
            UnityDictionaryErrorCode ec = unityDict.CalculateErrorCode();

            // Drop into the first child, which will be the keyValuePairs.
            property.Next(true);

            // Then, draw only the keyValuePairs.
            EditorGUI.PropertyField(position, property, label, true);

            if (ec.HasFlag(UnityDictionaryErrorCode.DuplicateKeys))
            {
                position = ShowDictErrCode(
                    position, property, label,
                    "Duplicate keys detected in the Unity Dictionary. " +
                    "Duplicate keys are not allowed in dictionaries."
                );
            }

            if (ec.HasFlag(UnityDictionaryErrorCode.NullKeys))
            {
                position = ShowDictErrCode(
                    position, property, label,
                    "Null keys detected in the Unity Dictionary. " +
                    "Null keys are not allowed in dictionaries."
                );
            }
        }
        catch (System.ArgumentNullException)
        {
            base.OnGUI(position, property, label);
            return;
        }
    }

    private Rect ShowDictErrCode(Rect position, SerializedProperty property,
        GUIContent label, string errBoxMsg)
    {
        position.y += EditorGUI.GetPropertyHeight(property, label)
                            + LIST_ERROR_SPACING;
        EditorGUI.HelpBox(
            new(position.x, position.y, position.width, errorBoxHeight),
            errBoxMsg,
            MessageType.Error
        );
        position.y += errorBoxHeight;
        
        return position;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // First, load the unityDictionary. This enables us to check for
        // errors.
        property.GetObjectFromReflection(out IUnityDictionary unityDict);
        UnityDictionaryErrorCode ec = unityDict.CalculateErrorCode();

        float height = 0;

        if (ec != UnityDictionaryErrorCode.None)
        {
            height += LIST_ERROR_SPACING * 2;
        }

        if (ec == UnityDictionaryErrorCode.DuplicateKeys)
        {
            height += errorBoxHeight;
        }

        if (ec == UnityDictionaryErrorCode.NullKeys)
        {
            height += errorBoxHeight;
        }

        // Then drop into the first child, which will be the keyValuePairs.
        property.Next(true);

        // Then get height.
        height += EditorGUI.GetPropertyHeight(property);
        return height;
    }
}