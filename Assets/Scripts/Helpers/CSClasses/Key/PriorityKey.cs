using System;
using System.Collections.Generic;

[Serializable]
public class PriorityKey : IComparable<PriorityKey>
{
    public int priority;

    public int id;

    public string tag;

    /// <summary>
    /// Creates a new priority key.
    /// </summary>
    /// <param name="priority">Priority of the key. A lower value is a higher
    /// priority.</param>
    /// <param name="id">ID of the key.</param>
    /// <param name="tag">Optional tag to distinguish between different keys
    /// with the same ID</param>
    public PriorityKey(int priority, int id, string tag = "default")
    {
        this.priority = priority;
        this.id = id;
        this.tag = tag;
    }

    /// <summary>
    /// Creates a new priority key, using the 
    /// </summary>
    /// <param name="priority">Priority of the key. A lower value is a higher
    /// priority.</param>
    /// <param name="unityObject">Object used for ID.</param>
    /// <param name="tag">Optional tag to distinguish between different keys
    /// with the same ID</param>
    public PriorityKey(int priority, UnityEngine.Object unityObject,
        string tag = "default")
    {
        this.priority = priority;
        this.id = unityObject.GetInstanceID();
        this.tag = tag;
    }

    public int CompareTo(PriorityKey other)
    {
        if (priority == other.priority)
        {
            if (id == other.id)
                return tag.CompareTo(other.tag);
            
            return id.CompareTo(other.id);
        }
        else
            // Lower value == higher priority.
            return priority.CompareTo(other.priority);
    }

    public override string ToString()
    {
        return $"PriorityKey [{priority}] ({id}:{tag})";
    }
}