using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a chain of <see cref="Modifier"/>s. Allows one float to be modified
/// by an arbitrary number of modifiers.
///
/// <br/>
///
/// USAGE: To add to the chain, use <see cref="Add(PriorityKey, Modifier)"/>.
/// <see cref="PriorityKey"/> determines the order the <see cref="Modifier"/>s
/// are executed, with lower values of <see cref="PriorityKey.priority"/>
/// denoting higher priority.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
/// <seealso cref="PriorityKey"/>
[System.Serializable]
public class ModifierChain
{
    /// <summary>
    /// The internal modifier chain used in modifying some value.
    /// </summary>
    private SortedDictionary<PriorityKey, Modifier> chain = new();

    /// <summary>
    /// Adds a modifier to the chain.
    /// </summary>
    /// <param name="key">Key to add.</param>
    /// <param name="modifier">Modifier to add.</param>
    /// <returns>True if key successfully added, false otherwise.</returns>
    public bool Add(PriorityKey key, Modifier modifier)
    {
        if (chain.ContainsKey(key))
            return false;
        
        chain.Add(key, modifier);
        return true;
    }

    /// <summary>
    /// Removes a modifier from the chain.
    /// </summary>
    /// <param name="key">Key to remove.</param>
    /// <returns>True on successful removal, false otherwise.</returns>
    public bool Remove(PriorityKey key)
    {
        return chain.Remove(key);
    }

    /// <summary>
    /// Clears the list of modifiers.
    /// </summary>
    public void Clear()
    {
        chain.Clear();
    }

    /// <summary>
    /// Modifies the input using the chain.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public float Modify(float input)
    {
        foreach (var pair in chain)
        {
            input = pair.Value.Modify(input);
        }

        return input;
    }
}