using System;
using UnityEngine;

/// <summary>
/// Attribute that allows a field to be enabled.
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
[Serializable]
public class ToggleActiveAttribute : PropertyAttribute
{
    /// <summary>
    /// The name of the boolean that activates the attribute.
    /// </summary>
    private readonly string toggler;

    public string ToggleName => toggler;

    /// <summary>
    /// Creates a toggle active attribute.
    /// </summary>
    /// <param name="toggler">The name of the boolean that activates the
    /// attribute.</param>
    public ToggleActiveAttribute(string toggler)
    {
        this.toggler = toggler;
    }
}