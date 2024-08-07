using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CustomPropertyDrawer(typeof(StaticKeyedDictionary<,>), true)]
public class StaticKeyedDictionaryPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property,
        GUIContent label)
    {
        // position.Translate(RNGExt.RandomVector2(5));
        try
        {
            // Fix the enum dictionary if needed.
            property.GetObjectFromReflection(out IStaticKeyedDictionary dict);
            dict.AssignEditorDictionary(property.serializedObject.targetObject);

            // Check if the base property is expanded or not. If not, don't draw
            // anything.
            property.isExpanded = EditorGUI.Foldout(
                position,
                property.isExpanded,
                label.text,
                EditorStyles.foldoutHeader);

            if (property.isExpanded)
            {
                // Collapsable section
                EditorGUI.indentLevel++;

                // First drop into the first child, which will be editorDict,
                // then drop into the second child, which will be keyValuePairs.
                property = property.FindPropertyRelative(
                    "editorDict.keyValuePairs"
                );
                Assert.IsNotNull(property, "Could not locate editorDict.keyValuePairs");

                foreach (SerializedProperty child in property)
                {
                    // Drop into the Key field and get the static key value.
                    var key = child.FindPropertyRelative("key");
                    // child.Next(true);
                    var keyVal = key.GetObjectFromReflection();
                    string keyName = dict.LabelFromKey(keyVal);

                    // Drop into the Value field and draw that property.
                    if (key.Next(false))
                    {
                        EditorGUILayout.PropertyField(
                            key,
                            new GUIContent(keyName),
                            GUILayout.ExpandHeight(true),
                            GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        Debug.LogWarning("Could not serialize value! " +
                            "Make sure the value is marked Serializable.");
                        EditorGUILayout.LabelField(new GUIContent(keyName));
                    }
                }

                // Exit collapsible section.
                EditorGUI.indentLevel--;
            }
        }
        catch (System.ArgumentException)
        {
            base.OnGUI(position, property, label);
            return;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // First drop into the first child, which will be the UnityDict.
        property.Next(true);

        // Then get height.
        var height = EditorGUI.GetPropertyHeight(property, label);
        return height;
    }
}
