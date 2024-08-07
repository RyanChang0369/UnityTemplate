using System;
using UnityEngine;

/// <summary>
/// Represents a dictionary of animation/model bones of a skinned mesh renderer.
/// Use this to easily map bones to some other value, and have the mapping be
/// editable in the unity editor.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[Serializable]
public class BonesDictionary<TValue> :
    StaticKeyedDictionary<Transform, TValue>
{
    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;

    #region StaticKeyedDictionary Implementation
    public override bool AssignEditorDictionary(UnityEngine.Object targetObject)
    {
        bool noChanges = true;

        if (!skinnedMesh)
        {
            if (targetObject is Component component)
            {
                component.RequireComponent(out skinnedMesh);
            }
            else
            {
                throw new InvalidOperationException(
                    $"{this} must be attached to a Component");
            }
        }

        foreach (var bone in skinnedMesh.bones)
        {
            if (!ContainsKey(bone))
            {
                // No key. Fix it.
                editorDict[bone] = default;
                noChanges = false;
            }
        }

        return noChanges;
    }

    public override string LabelFromKey(Transform key) => key.gameObject.name;
    #endregion
}