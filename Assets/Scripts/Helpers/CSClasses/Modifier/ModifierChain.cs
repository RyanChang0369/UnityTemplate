using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifierChain
{
    public SortedDictionary<PriorityKey, Modifier> chain = new();

    /// <summary>
    /// Adds a modifier to the chain.
    /// </summary>
    /// <param name="key">Key to add.</param>
    /// <param name="modifier">Modifier to add.</param>
    /// <returns>True if key successfully added, false otherwise</returns>
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
    /// <returns></returns>
    public bool Remove(PriorityKey key)
    {
        return chain.Remove(key);
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