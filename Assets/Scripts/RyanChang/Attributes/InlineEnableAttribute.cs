using System;
using UnityEngine;

/// <summary>
/// Inlines a boolean field with another field (the field the attribute is
/// attached to.)
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class InlineEnableAttributeAttribute : Attribute
{
    #region Fields
    /// <summary>
    /// The name of the boolean (a field, property, or parameterless
    /// function).
    /// </summary>
    [Tooltip("The name of the boolean (a field, property, or parameterless " +
        "function).")]
    public string enablerName;

    #endregion

    #region Constructors
    public InlineEnableAttributeAttribute(string enablerName)
    {
        this.enablerName = enablerName;
    }
    #endregion
}